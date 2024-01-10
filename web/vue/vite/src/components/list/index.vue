<template>
  <div v-loading="loading">
    <template v-if="config">
      <el-card style="height: 100%">
        <div style="height: 100%; display: flex; flex-direction: column">
          <app-form
            ref="queryFormRef"
            v-model="queryModel"
            inline
            mode="query"
            :schema="querySchema"
            label-position="left"
            :hide-button="true"
          />
          <div v-if="tableButtons.length" style="padding-bottom: 20px">
            <template v-for="item in tableButtons" :key="item.path">
              <el-button
                :type="getButtonType(item.meta?.path)"
                @click="
                  click(
                    item,
                    listModel.filter((o) => o.checked),
                  )
                "
              >
                <template #default>
                  <svg-icon v-if="item.meta?.icon" :name="item.meta?.icon" />
                  <span v-if="item.meta?.title">{{ $t(item.meta?.title) }}</span>
                </template>
              </el-button>
            </template>
            <el-button type="primary" style="float: right" @click="print">{{ $t('print') }}</el-button>
            <el-button type="primary" style="float: right" @click="drawerVisible = true">{{ $t('filter') }}</el-button>
            <el-button type="primary" style="float: right" @click="reset">{{ $t('reset') }}</el-button>
          </div>
          <div style="flex: 1; overflow: hidden">
            <el-auto-resizer>
              <template #default="{ height, width }">
                <el-table-v2
                  id="table"
                  v-model:sort-state="sortModel"
                  :columns="columns"
                  :data="listModel"
                  :width="width"
                  :height="height"
                  fixed
                  @column-sort="onSort"
                />
              </template>
            </el-auto-resizer>
          </div>
          <el-pagination
            v-model:currentPage="pageModel.pageIndex"
            v-model:page-size="pageModel.pageSize"
            :small="appStore.settings.size === 'small'"
            :total="pageModel.total"
            :page-sizes="pageModel.sizeList"
            :background="true"
            layout="total, sizes, prev, pager, next, jumper"
            style="margin-top: 20px"
            @size-change="onPageSizeChange"
            @current-change="onPageIndexChange"
          />
        </div>
      </el-card>
    </template>
    <el-drawer v-model="drawerVisible" :title="$t('filter')" size="auto" destroy-on-close>
      <template v-for="(item, i) in columns" :key="item">
        <div
          v-if="item.title"
          style="display: flex; align-items: center"
          :draggable="item.draggable"
          @dragstart.stop="dragStartIndex = i"
          @dragenter="$event.preventDefault()"
          @dragover="$event.preventDefault()"
          @drop.prevent="drop($event, i)"
        >
          <el-icon :style="{ cursor: item.draggable ? 'pointer' : 'not-allowed' }" style="margin-right: 5px">
            <i class="i-ri-drag-move-fill" />
          </el-icon>
          <el-checkbox v-model="item.show" :label="item.title" @change="filterChange(item)" />
        </div>
      </template>
      <template #footer>
        <el-button type="primary" @click="selectAll">
          {{ $t('selectAll') }}
        </el-button>
        <el-button type="primary" @click="invertSelect">
          {{ $t('invertSelection') }}
        </el-button>
        <el-button type="primary" @click="resetColumns">{{ $t('reset') }}</el-button>
      </template>
    </el-drawer>
    <el-dialog v-if="dialogVisible" v-model="dialogVisible" :title="$t(dialogSchema.title)" destroy-on-close>
      <el-button
        link
        style="height: 50px"
        v-if="dialogSchema.action === 'import' && buttons.find((o) => o.meta?.command === 'import-template')"
        type="primary"
        @click="click(buttons.find((o) => o.meta?.command === 'import-template'))"
      >
        {{ $t('importTemplate') }}
      </el-button>
      <app-form
        ref="dialogFormRef"
        v-model="dialogModel"
        :schema="dialogSchema"
        label-position="left"
        :hide-button="true"
        :mode="dialogSchema.action"
        @success="success"
      />
      <template #footer>
        <span class="dialog-footer">
          <el-button type="primary" @click="dialogConfirm(dialogSchema.action)">
            {{ $t('confirm') }}
          </el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script lang="jsx" setup>
  import { dayjs, ElMessage, ElMessageBox } from 'element-plus';
  import { onMounted, ref, unref } from 'vue';
  import { useI18n } from 'vue-i18n';
  import { useRoute } from 'vue-router';

  import AppForm from '@/components/form/index.vue';
  import SvgIcon from '@/components/icon/index.vue';
  import { useAppStore } from '@/store/index.js';
  import { listToTree, log, schemaToModel, downloadFile } from '@/utils/index.js';
  import request from '@/utils/request.js';
  import printJS from 'print-js';
  import html2canvas from 'html2canvas';

  const props = defineProps({
    config: {
      type: Object,
      default: null,
    },
    routeValue: {
      type: Array,
      default: null,
    },
  });

  const { t } = useI18n();
  const route = useRoute();
  const appStore = useAppStore();

  const loading = ref(true);
  const queryFormRef = ref(null);
  const dialogFormRef = ref(null);
  const drawerVisible = ref(false);
  const dialogVisible = ref(false);
  const dialogModel = ref(null);
  const dialogSchema = ref(null);
  const buttons = ref((props.routeValue ?? route).meta?.buttons ?? []);
  const tableButtons = ref(
    buttons.value?.filter((o) => o.meta?.buttonType === 'table' && o.meta.command !== 'import-template'),
  );
  const querySchema = ref(props.config.properties.query);
  const queryModel = ref(schemaToModel(querySchema.value));
  const listSchema = ref(props.config.properties.list);
  const listModel = ref([]);
  const columns = ref([]);
  const originalColumns = ref([]);
  const dragStartIndex = ref(null);
  const sortModel = ref(
    (() => {
      const result = {};
      (queryModel.value.sorting ?? '')
        .split(',')
        .map((o) => o.trim())
        .filter((o) => o)
        .forEach((o) => {
          const [columnName, direction = 'asc'] = o.split(' ');
          result[columnName] = direction;
        });
      return result;
    })(),
  );
  const pageModel = ref({
    sizeList: [10, 100, 1000, 10000],
    pageIndex: 1,
    pageSize: 10,
    total: 0,
  });

  const drop = (e, index) => {
    if (dragStartIndex.value !== index) {
      const source = columns.value[dragStartIndex.value];
      columns.value.splice(dragStartIndex.value, 1);
      columns.value.splice(index, 0, source);
    }
  };

  const filterChange = (item) => {
    item.hidden = !item.show;
  };

  const selectAll = () => {
    columns.value.forEach((o) => {
      o.hidden = false;
      o.show = !o.hidden;
    });
  };

  const invertSelect = () => {
    columns.value.forEach((o) => {
      o.hidden = !o.hidden;
      o.show = !o.hidden;
    });
  };

  const resetColumns = () => {
    if (originalColumns.value) {
      columns.value = [...originalColumns.value];
      columns.value.forEach((o) => {
        o.hidden = false;
        o.show = !o.hidden;
      });
    }
  };

  const buildQuery = () => {
    const data = {
      pageIndex: pageModel.value.pageIndex,
      pageSize: pageModel.value.pageSize,
      sorting: Object.entries(sortModel.value)
        .map(({ key, order }) => `${key} ${order}`)
        .join(','),
    };
    Object.entries(unref(queryModel)).forEach(([key, value]) => {
      if (value !== null) {
        data[key] = value;
      }
    });
    return data;
  };

  const load = async () => {
    loading.value = true;
    try {
      const data = buildQuery();
      const method = buttons.value.find((button) => button.meta?.command === 'search')?.meta?.apiMethod ?? 'POST';
      const url = buttons.value.find((button) => button.meta?.command === 'search')?.meta?.apiUrl;
      const result = await request(method, url, data);
      if (result.ok) {
        const { items, pageIndex, pageSize, totalCount } = result.data;
        listModel.value = listSchema.value.isTree ? listToTree(items) : items;
        pageModel.value.total = totalCount;
        pageModel.value.pageIndex = pageIndex;
        pageModel.value.pageSize = pageSize;
        const rowNumberWidth = `${pageIndex * pageSize}`.length * 8 + 16;
        const rowNumberRow = columns.value.find((o) => o.key === 'rowNumber');
        rowNumberRow.width = rowNumberWidth > rowNumberRow.width ? rowNumberWidth : rowNumberRow.width;
      } else {
        if (result.code === 500) {
          await ElMessageBox.confirm(result.message, t('tip'), {
            type: 'warning',
          });
        }
      }
    } catch (e) {
      log(e);
    } finally {
      loading.value = false;
    }
  };

  const reload = async () => {
    pageModel.value.pageIndex = 1;
    await load();
  };

  const reset = async () => {
    queryFormRef.value.reset();
    await reload();
  };

  const onPageIndexChange = async () => {
    await load();
  };

  const onPageSizeChange = async () => {
    await reload();
  };

  const onSort = async ({ key, order }) => {
    if (!order) {
      sortModel.value[key] = 'asc';
    } else if (sortModel.value[key] === 'asc') {
      sortModel.value[key] = order;
    } else {
      delete sortModel.value[key];
    }

    await load();
  };

  const click = async (button, rows) => {
    const { command } = button.meta;
    if (command === 'search') {
      await load();
      return;
    }
    if (command === 'import-template') {
      loading.value = true;
      try {
        const method = button.meta?.apiMethod ?? 'POST';
        const url = button.meta?.apiUrl;
        const result = await request(method, url);
        if (result.ok) {
          dialogVisible.value = false;
          downloadFile(result.data.file, result.data.name);
        } else {
          if (result.code === 500) {
            await ElMessageBox.confirm(result.message, t('tip'), {
              type: 'warning',
            });
          }
        }
      } catch (e) {
        log(e);
      } finally {
        loading.value = false;
      }
    }
    if (command === 'delete') {
      const data = rows.filter((o) => o.checked).map((o) => o.id);
      if (data.length === 0) {
        return;
      }
      try {
        loading.value = true;
        await ElMessageBox.confirm(t('confirmDelete', [data.length]), t('warning'), {
          confirmButtonText: t('confirm'),
          cancelButtonText: t('cancel'),
          type: 'warning',
        });
        const method = button.meta?.apiMethod ?? 'POST';
        const url = button.meta?.apiUrl;
        const result = await request(method, url, data);
        if (result.ok) {
          await reload();
        }
      } catch (error) {
        if (error === 'cancel') {
          ElMessage({
            type: 'info',
            message: 'Cancel',
          });
        }
      } finally {
        loading.value = false;
      }
      return;
    }
    if (
      command === 'create' ||
      command === 'update' ||
      command === 'import' ||
      command === 'export' ||
      command === 'details'
    ) {
      const schema = props.config.properties[command];
      schema.action = command;
      schema.method = button.meta?.apiMethod;
      schema.url = button.meta?.apiUrl;
      dialogSchema.value = schema;
      if (command === 'details' || command === 'update') {
        const detailsButton = buttons.value.find((button) => button.meta?.command === 'details');
        const method = detailsButton?.meta?.apiMethod ?? 'POST';
        const url = detailsButton?.meta?.apiUrl;
        const data = rows[0].id;
        try {
          loading.value = true;
          const result = await request(method, url, data);
          if (result.ok) {
            dialogModel.value = result.data;
          } else {
            if (result.code === 500) {
              await ElMessageBox.confirm(result.message, t('tip'), {
                type: 'warning',
              });
            }
          }
        } finally {
          loading.value = false;
        }
      } else {
        dialogModel.value = schemaToModel(schema);
      }
      dialogVisible.value = true;
    }
    console.log(button);
    console.log(rows);
  };

  const dialogConfirm = async (action) => {
    console.log(dialogFormRef.value);
    if (action === 'details') {
      dialogVisible.value = false;
      return;
    }
    if (action === 'create' || action === 'update') {
      await dialogFormRef.value.submit();
      return;
    }
    if (action === 'export' || action === 'import') {
      loading.value = true;
      try {
        let data = null;
        if (action === 'export') {
          data = buildQuery();
          Object.assign(data, dialogModel.value);
        } else {
          data = new FormData();
          Object.keys(dialogModel.value).forEach((key) => {
            const schema = dialogSchema.value.properties[key];
            if (schema?.type === 'array') {
              dialogModel.value[key].forEach((value) => {
                data.append(key, schema.input === 'file' ? value.raw : value);
              });
            } else {
              const value = dialogModel.value[key];
              data.append(key, schema.input === 'file' ? value.raw : value);
            }
          });
        }
        const button = buttons.value.find((o) => o.meta?.command === action);
        const method = button.meta?.apiMethod ?? 'POST';
        const url = button.meta?.apiUrl;
        const result = await request(method, url, data);
        if (result.ok) {
          dialogVisible.value = false;
          if (action === 'export') {
            downloadFile(result.data.file, result.data.name);
          } else {
            await reload();
          }
        } else {
          if (result.code === 500) {
            await ElMessageBox.confirm(result.message, t('tip'), {
              type: 'warning',
            });
          }
        }
      } catch (e) {
        log(e);
      } finally {
        loading.value = false;
      }
    }
  };

  const success = async (data) => {
    dialogVisible.value = false;
    await reload();
  };

  const print = () => {
    html2canvas(document.getElementById('table')).then((canvas) => {
      const img = canvas.toDataURL('image/png', 1);
      printJS(img, 'image');
    });
  };

  const initColumns = () => {
    const result = [
      {
        key: 'checked',
        dataKey: 'checked',
        width: 44,
        fixed: 'left',
        cellRenderer: (prop) => {
          const onChange = (value) => {
            prop.rowData.checked = value;
          };
          return <el-checkbox modelValue={prop.rowData.checked} onChange={onChange} />;
        },
        headerCellRenderer: () => {
          const rawData = unref(listModel);
          const onChange = (value) => {
            listModel.value = rawData.map((row) => {
              row.checked = value;
              return row;
            });
          };
          const allSelected = rawData.every((row) => row.checked);
          const containsChecked = rawData.some((row) => row.checked);
          return (
            <el-checkbox modelValue={allSelected} indeterminate={containsChecked && !allSelected} onChange={onChange} />
          );
        },
      },
      {
        key: 'rowNumber',
        dataKey: 'rowNumber',
        title: t('rowNumber'),
        fixed: 'left',
        width: 44,
        hidden: false,
        cellRenderer: (prop) => {
          return <>{(pageModel.value.pageIndex - 1) * pageModel.value.pageSize + prop.rowIndex + 1}</>;
        },
      },
    ];
    Object.keys(listSchema.value.properties).forEach((key) => {
      const property = listSchema.value.properties[key];
      if (property.hidden) {
        return;
      }
      const column = {
        key,
        dataKey: key,
        title: t(property?.title ?? key),
        width: property?.width ?? 120,
        hidden: property.hidden,
        sortable: property?.sortable ?? true,
        draggable: true,
      };
      if (property.input === 'datetime') {
        column.cellRenderer = ({ cellData }) => {
          return <span>{dayjs(cellData).format(property.format ?? 'YYYY-MM-DD HH:mm:ss')}</span>;
        };
      }
      if (property.type === 'boolean') {
        column.cellRenderer = ({ cellData }) => {
          return (
            <span>
              <el-checkbox disabled vModel={cellData} />
            </span>
          );
        };
      }
      result.push(column);
    });
    const rowButtons = buttons.value?.filter((o) => o.meta?.buttonType === 'row');
    const width =
      rowButtons
        .map((o) => o.meta?.width)
        .filter((o) => o)
        .reduce((a, b) => a + b, null) ?? 130;
    if (rowButtons.length) {
      result.push({
        key: 'operations',
        width,
        fixed: 'right',
        title: t('operations'),
        cellRenderer: (prop) => {
          return (
            <>
              {rowButtons.map((o) => {
                return (
                  <el-button type="primary" text={true} onClick={() => click(o, [prop.rowData])}>
                    {t(o.meta.title)}
                  </el-button>
                );
              })}
            </>
          );
        },
      });
    }
    result.forEach((o) => {
      o.show = !o.hidden;
    });
    return result;
  };

  const getButtonType = (action) => {
    switch (action) {
      case 'search':
        return 'primary';
      case 'create':
        return 'primary';
      case 'delete':
        return 'danger';
      case 'edit':
        return 'primary';
      case 'import':
        return 'success';
      case 'export':
        return 'success';
      default:
        return 'primary';
    }
  };

  onMounted(async () => {
    await load();
  });

  //
  columns.value = initColumns();
  originalColumns.value = [...columns.value];
</script>
