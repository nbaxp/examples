<template>
  <div v-loading="loading">
    <template v-if="config">
      <el-card style="height: 100%">
        <div style="height: 100%; display: flex; flex-direction: column">
          <app-form v-model="queryModel" inline mode="query" :schema="querySchema" label-position="left" />
          <div style="flex: 1; overflow: hidden">
            <el-auto-resizer>
              <template #default="{ height, width }">
                <el-table-v2 :columns="columns" :data="listModel" :width="width" :height="height" fixed />
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
  </div>
</template>

<script lang="jsx" setup>
  import { dayjs, ElMessageBox } from 'element-plus';
  import { computed, onMounted, ref, unref } from 'vue';
  import { useI18n } from 'vue-i18n';

  import AppForm from '@/components/form/index.vue';
  import { useAppStore } from '@/store/index.js';
  import { listToTree, log, schemaToModel } from '@/utils/index.js';
  import request from '@/utils/request.js';

  const props = defineProps({
    config: {
      type: Object,
      default: null,
    },
  });

  const { t } = useI18n();
  const appStore = useAppStore();

  const loading = ref(true);
  const querySchema = ref(props.config.properties.query);
  const listSchema = ref(props.config.properties.list);
  const editSchema = ref(props.config.properties.edit);
  const queryModel = ref(schemaToModel(querySchema.value));
  const listModel = ref([]);
  const columns = ref([]);
  const pageModel = ref({
    sizeList: [10, 100, 1000, 10000],
    pageIndex: 1,
    pageSize: 10,
    total: 0,
  });

  const getColumns = () => {
    return Object.keys(listSchema.value.properties).map((key) => {
      const property = listSchema.value.properties[key];
      const column = {
        key,
        dataKey: key,
        title: t(property?.title ?? key),
        width: property?.width ?? 120,
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
      return column;
    });
  };

  columns.value = [
    {
      key: 'selection',
      width: 50,
      cellRenderer: ({ rowData }) => {
        const onChange = (value) => {
          rowData.checked = value;
        };
        return <ElCheckbox value={rowData.checked} onChange={onChange} />;
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
        return <ElCheckbox value={allSelected} intermediate={containsChecked && !allSelected} onChange={onChange} />;
      },
    },
  ].concat(getColumns());

  const buildQuery = () => {
    const data = {
      pageIndex: pageModel.value.pageIndex,
      pageSize: pageModel.value.pageSize,
      ...queryModel.value,
    };
    return data;
  };

  const load = async () => {
    loading.value = true;
    try {
      const data = buildQuery();
      const result = await request(querySchema.value.method, querySchema.value.api, data);
      if (result.ok) {
        const { items, pageIndex, pageSize, totalCount } = result.data;
        listModel.value = listSchema.value.isTree ? listToTree(items) : items;
        pageModel.value.total = totalCount;
        pageModel.value.pageIndex = pageIndex;
        pageModel.value.pageSize = pageSize;
      } else {
        await ElMessageBox.confirm(result.message, '提示', {
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
  const onPageIndexChange = async () => {
    await load();
  };
  const onPageSizeChange = async () => {
    await reload();
  };
  onMounted(async () => {
    await load();
  });
</script>
