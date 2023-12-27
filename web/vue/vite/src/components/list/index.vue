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
            <template v-for="item in tableButtons" :key="item.action">
              <el-button
                :type="getButtonType(item.meta?.action)"
                @click="
                  click(
                    item,
                    listModel.filter((o) => o.checked),
                  )
                "
              >
                <template #default>
                  <svg-icon v-if="item.meta.icon" :name="item.meta.icon" />
                  <span>{{ $t(item.meta?.title) }}</span>
                </template>
              </el-button>
            </template>
            <el-button type="primary" style="float: right" @click="drawerVisible = true">{{ $t('Filter') }}</el-button>
            <el-button type="info" style="float: right" @click="reset">{{ $t('Reset') }}</el-button>
          </div>
          <div style="flex: 1; overflow: hidden">
            <el-auto-resizer>
              <template #default="{ height, width }">
                <el-table-v2
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
    <el-drawer v-model="drawerVisible" :title="$t('Filter')" size="auto" destroy-on-close>
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
          {{ $t('Select All') }}
        </el-button>
        <el-button type="primary" @click="invertSelect">
          {{ $t('Invert Selection') }}
        </el-button>
        <el-button type="primary" @click="resetColumns">{{ $t('Reset') }}</el-button>
      </template>
    </el-drawer>
    <el-dialog v-if="dialogVisible" v-model="dialogVisible" :title="$t(dialogSchema.title)" destroy-on-close>
      <app-form
        ref="dialogFormRef"
        v-model="dialogModel"
        :schema="dialogSchema"
        label-position="left"
        :hide-button="true"
      />
      <template #footer>
        <span class="dialog-footer">
          <el-button type="primary" @click="dialogConfirm(dialogSchema.action)">
            {{ $t('Confirm') }}
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
  import { listToTree, log, schemaToModel } from '@/utils/index.js';
  import request from '@/utils/request.js';

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
  const tableButtons = ref(buttons.value?.filter((o) => o.meta?.position !== 'row'));
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
      const result = await request(querySchema.value.method, querySchema.value.url, data);
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
        await ElMessageBox.confirm(result.message, t('Tip'), {
          type: 'warning',
        });
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
    const { action } = button.meta;
    if (
      action === 'create' ||
      action === 'update' ||
      action === 'import' ||
      action === 'export' ||
      action === 'detail'
    ) {
      const schema = props.config.properties[action];
      schema.action = action;
      dialogSchema.value = schema;
      dialogModel.value = schemaToModel(schema);
      dialogVisible.value = true;
    } else if (action === 'search') {
      await load();
    } else if (button.meta.action === 'delete') {
      const count = listModel.value.filter((o) => o.checked).length;
      if (count === 0) {
        return;
      }
      try {
        loading.value = true;
        await ElMessageBox.confirm(t('Confirm Delete', { count }), t('Warning'), {
          confirmButtonText: t('Confirm'),
          cancelButtonText: t('Cancel'),
          type: 'warning',
        });
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
    }
    console.log(button);
    console.log(rows);
  };

  const dialogConfirm = async (action) => {
    console.log(action);
    loading.value = true;
    try {
      // if (action === 'import') {
      // }
      const data = buildQuery();
      Object.assign(data, dialogModel.value);
      const result = await request(dialogSchema.value.method, dialogSchema.value.url, data);
      if (result.ok) {
        console.log(result.data);
      } else {
        await ElMessageBox.alert(result.message, t('Tip'), {
          type: 'warning',
        });
      }
    } catch (e) {
      log(e);
    } finally {
      loading.value = false;
    }
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
        title: t('Row Number'),
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
      const column = {
        key,
        dataKey: key,
        title: t(property?.title),
        width: property?.width ?? 120,
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
    const rowButtons = buttons.value?.filter((o) => o.meta?.position === 'row');
    const width =
      rowButtons
        .map((o) => o.meta?.width)
        .filter((o) => o)
        .reduce((a, b) => a + b, null) ?? 120;
    if (rowButtons.length) {
      result.push({
        key: 'operations',
        width,
        fixed: 'right',
        title: t('Operations'),
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
