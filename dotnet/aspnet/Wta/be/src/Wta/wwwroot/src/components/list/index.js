import AppFormInput from '@/components/form/form-input.js';
import AppForm from '@/components/form/index.js';
import SvgIcon from '@/components/icon/index.js';
import useExport from '@/models/export.js';
import useImport from '@/models/import.js';
import { useAppStore, useTokenStore } from '@/store/index.js';
import request, { getUrl } from '@/utils/request.js';
import { schemaToModel, toQuerySchema } from '@/utils/schema.js';
import { useCssVar } from '@vueuse/core';
import { ElMessage, ElMessageBox } from 'element-plus';
import * as jsondiffpatch from 'jsondiffpatch';
import { camelCase, capitalize } from 'lodash';
import { downloadFile, format, importFunction } from 'utils';
import html, { getProp, delay, listToTree } from 'utils';
import { computed, nextTick, onMounted, reactive, ref, unref, watch } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRoute, useRouter } from 'vue-router';

export default {
  name: 'AppList',
  components: {
    AppForm,
    AppFormInput,
    SvgIcon,
  },
  template: html`<div class="pb-5" v-loading="loading" element-loading-text="Loading...">
  <el-card style="position: relative;">
    <div
      @click="()=>queryFormFold=!queryFormFold"
      class="cursor-pointer"
      style="display:inline-block;position: absolute;top:20px;right:20px;"
    >
      <span style="line-height: 2em">
        <el-icon>
          <ep-arrow-up v-if="!queryFormFold" />
          <ep-arrow-down v-else />
        </el-icon>
      </span>
    </div>
    <!--查询表单-->
    <app-form
      ref="queryFormRef"
      inline
      mode="query"
      :schema="querySchema"
      v-model="queryModel"
      @submit="load"
      :hideButton="true"
      :isQueryForm="true"
      class="query"
      label-position="right"
      :style="queryStyle"
    />
    <el-row class="flex justify-between">
      <div>
        <template v-for="item in buttons">
          <el-button
            v-if="!item.meta.hidden&&item.meta.buttonType==='table'"
            @click="buttonClick(item,selectedRows)"
            :class="item.meta.htmlClass??'el-button--primary'"
            v-show="!item.meta.show||item.meta.show(selectedRows,queryModel)"
            :disabled="getButtonDisabled(item)"
            class="mb-5 mr-3"
            style="margin-left:0;"
          >
            <el-icon><svg-icon :name="item.meta.icon??item.meta.command??item.path" /></el-icon>
            <span>{{item.meta.title}}</span>
          </el-button>
        </template>
        <el-button v-if="false" @click="buttonClick('filter',selectedRows)">
          <el-icon><ep-filter /></el-icon>
          <span>{{$t('筛选')}}</span>
        </el-button>
        <slot name="tableButtons" :rows="selectedRows"></slot>
      </div>
      <div>
        <el-button
          @click="buttonClick(buttons.find(o=>o.meta.command==='search'),selectedRows)"
          class="el-button--primary mb-5"
        >
          查询
        </el-button>
        <el-button @click="reset" class="mb-5 ml-3">重置</el-button>
      </div>
    </el-row>
    <!--列表-->
    <el-table
      :key="tableKey"
      ref="tableRef"
      :tree-props="treeProps"
      :data="tableData"
      @selection-change="handleSelectionChange"
      @sort-change="sortChange"
      :header-cell-class-name="getClass"
      row-key="id"
      table-layout="auto"
      border
      fit
    >
      <el-table-column v-if="!schema.disableSelection" fixed="left" type="selection" :selectable="schema.selectable" />
      <el-table-column type="index" :label="$t('rowIndex')">
        <template #default="scope">
          {{ (pageModel.pageIndex - 1) * pageModel.pageSize + scope.$index + 1 }}
        </template>
      </el-table-column>
      <template v-for="(item,key) in schema.properties">
        <template v-if="item.navigation">
          <el-table-column :prop="key" :label="item.title">
            <template #default="scope">{{getProp(scope.row,item.navigation)}}</template>
          </el-table-column>
        </template>
        <template v-else-if="item.oneToMany">
          <el-table-column :prop="key" :label="item.title">
            <template #default="scope">
              <el-link type="primary" @click="showList({[key]:scope.row[key]},item.oneToMany,item.config)">
                <app-form-input mode="details" :schema="item" :prop="key" v-model="scope.row" />
              </el-link>
            </template>
          </el-table-column>
        </template>
        <template v-else-if="item.link">
          <el-table-column :prop="key" :label="item.title">
            <template #default="scope">
              <el-link type="primary" @click="buttonClick({path:key},[scope.row])">
                {{scope.row[key]}}
              </el-link>
            </template>
          </el-table-column>
        </template>
        <template v-else-if="item.type!=='object'&&!item.meta.hidden">
          <template v-if="!item.meta.hideForList&&showColumn(item,key)">
            <el-table-column
              :prop="key"
              :sortable="schema.meta?.isTree?false:'custom'"
              :sort-orders="['descending', 'ascending', null]"
            >
              <template #header="scope">{{item.title}}</template>
              <template #default="scope">
                <app-form-input mode="details" :schema="item" :prop="key" v-model="scope.row" />
              </template>
            </el-table-column>
          </template>
        </template>
        <template v-if="item.type==='object'">
          <template v-for="(item2,key2) in item['properties']">
            <el-table-column :prop="key+'.'+key2">
              <template #header="scope">{{item2.title}}</template>
              <template #default="scope">
                <template v-if="scope.row[key]">
                  <app-form-input mode="details" :schema="item2" :prop="key2" v-model="scope.row[key]" />
                </template>
              </template>
            </el-table-column>
          </template>
        </template>
      </template>
      <slot name="columns"></slot>
      <el-table-column fixed="right">
        <template #header>
          <el-button @click="filterDrawer = true">
            {{$t('operations')}}
            <el-icon class="el-icon--right"><ep-filter /></el-icon>
          </el-button>
        </template>
        <template #default="scope">
          <div class="flex">
            <template v-for="item in buttons">
              <el-button
                :class="item.meta.htmlClass??'is-plan'"
                v-if="!item.meta.hidden&&item.meta.buttonType==='row'"
                v-show="!item.meta.show||item.meta.show(scope.row,queryModel)"
                @click="buttonClick(item,[scope.row])"
                :disabled="item.meta.disabled && item.meta.disabled(scope.row)"
              >
                <el-icon><svg-icon :name="item.meta.icon??item.meta.command??item.path" /></el-icon>
                <span>{{item.meta.title}}</span>
              </el-button>
            </template>
            <slot name="rowButtons" :rows="[scope.row]"></slot>
          </div>
        </template>
      </el-table-column>
    </el-table>
    <!--分页-->
    <el-pagination
      v-if="tableData.length>pageModel.pageSize"
      :size="appStore.size"
      v-model:currentPage="pageModel.pageIndex"
      v-model:page-size="pageModel.pageSize"
      :total="pageModel.total"
      :page-sizes="pageModel.sizeList"
      :background="true"
      layout="total, sizes, prev, pager, next, jumper"
      @size-change="onPageSizeChange"
      @current-change="onPageIndexChange"
      class="pt-5"
    />
  </el-card>
</div>
<!--列筛选抽屉-->
<el-drawer v-model="filterDrawer" destroy-on-close @close="tableRef.doLayout()">
  <template #header><span class="el-dialog__title">{{$t('filter')}}</span></template>
  <el-scrollbar>
    <el-row>
      <el-col>
        <el-form inline>
          <div>
            <el-button type="primary" @click="columns.forEach(o=>o.checked=true)">
              {{$t('selectAll')}}
            </el-button>
            <el-button type="primary" @click="columns.forEach(o=>o.checked=!o.checked)">
              {{$t('selectInverse')}}
            </el-button>
            <el-button type="primary" @click="resetColumns">{{ $t('重置') }}</el-button>
          </div>
          <div v-for="item in columns" style="display:inline-block;padding:10px;width:50%;">
            <el-checkbox v-model="item.checked" :label="item.title" size="large" />
          </div>
        </el-form>
      </el-col>
    </el-row>
  </el-scrollbar>
  <template #footer>
    <span class="dialog-footer">
      <el-button type="primary" @click="filterDrawer=false">{{$t('confirm')}}</el-button>
    </span>
  </template>
</el-drawer>
<!--通用对话框-->
<el-dialog
  v-model="dialogVisible"
  align-center
  append-to-body
  destroy-on-close
  :close-on-click-modal="false"
  style="max-height:100%;width:700px;"
>
  <template #header><span class="el-dialog__title">{{editFormTitle}}</span></template>
  <template #footer>
    <span class="dialog-footer">
      <el-button type="primary" @click="editFormSubmit">{{$t('confirm')}}</el-button>
    </span>
  </template>
  <el-row v-loading="editFormloading">
    <el-col>
      <el-scrollbar>
        <template v-if="editFormModel==='import'">
          <template v-for="item in buttons">
            <el-button
              v-if="item.meta.command==='import-template'"
              @click="buttonClick(item,selectedRows)"
              :class="item.meta.htmlClass??'el-button--primary'"
              v-show="!item.meta.show||item.meta.show(selectedRows,queryModel)"
              :disabled="getButtonDisabled(item)"
              class="mb-5 mr-3"
              style="margin-left:0;"
            >
              <el-icon><svg-icon :name="item.meta.icon??item.meta.command??item.path" /></el-icon>
              <span>{{item.meta.title}}</span>
            </el-button>
          </template>
        </template>
        <app-form
          :disabled="editFormMode==='details'"
          :mode="editFormMode"
          ref="editFormRef"
          inline
          label-position="right"
          :hideButton="true"
          :schema="editFormSchema"
          v-model="editFormModel"
          style="height:100%;"
        />
      </el-scrollbar>
    </el-col>
  </el-row>
</el-dialog>`,
  props: ['schema'],
  emits: ['command'],
  setup(props, context) {
    // 初始化
    const appStore = useAppStore();
    const tokenStore = useTokenStore();
    const loading = ref(true);
    // 分页
    const pageModel = reactive({
      sizeList: [10, 100, 1000],
      pageIndex: 1,
      pageSize: 10,
      total: 0,
    });
    const treeProps = reactive({
      children: 'children',
    });
    const querySchema = ref(toQuerySchema(props.schema));
    const queryModel = ref(schemaToModel(querySchema.value, true));
    const tableKey = ref(false);
    const tableRef = ref(null);
    const columns = ref([]);
    const filterDrawer = ref(false);
    const subDrawer = ref(false);
    const subListQuery = ref(props.query ?? {});
    const selectedRows = ref([]);
    const dialogVisible = ref(false);
    const route = useRoute();
    const router = useRouter();
    const { t } = useI18n();
    // 注释一下代码暂停权限验证
    // const buttons = ref(props.buttons ?? route.meta.children.filter((o) => o.meta.hasPermission));
    // 添加下行代码暂停权限验证
    const buttons = ref(props.schema.meta?.buttons ?? route.meta.children);
    const sortColumns = ref(new Map());
    const tableSchema = ref({});
    const tableData = ref([]);
    const editFormRef = ref(null);
    const exportFormRef = ref(null);
    const exportModel = ref({});
    const importFormRef = ref(null);
    const importModel = ref(null);
    const editFormloading = ref(false);
    const editFormMode = ref(null);
    const editFormTitle = ref('');
    const editFormSchema = ref(null);
    const editFormModel = ref(null);
    const editFormButton = ref(null);
    // watch(queryModel.value, async (value, oldValue, a) => {
    //   if (props.schema.autoSubmit) {
    //     await load();
    //   }
    // });
    const onClick = async (method, confirMmessage = '确认操作吗？', reload = true) => {
      try {
        if (confirMmessage) {
          await ElMessageBox.confirm(confirMmessage, '提示', {
            type: 'warning',
          });
        }
        loading.value = true;
        let result = null;
        if (method.constructor.name === 'AsyncFunction') {
          result = await method();
        } else {
          result = method();
        }
        if (!result.error && reload) {
          pageModel.pageIndex = 1;
          await load();
        }
      } catch (error) {
        if (error === 'cancel') {
          ElMessage({
            type: 'info',
            message: '操作取消',
          });
        }
      } finally {
        loading.value = false;
      }
    };
    const getSortModel = (model) => {
      const orders = (props.schema.meta?.sorting ?? '')
        .split(',')
        .map((o) => o.trim())
        .filter((o) => o)
        .map((o) => ({
          prop: camelCase(o.split(' ')[0]),
          order: `${o.split(' ').filter((o) => o)[1] ?? 'asc'}ending`,
        }));
      for (const o of orders) {
        sortColumns.value.set(o.prop, o.order);
      }
    };
    const getColumns = (schema) => {
      const result = [];
      const propertyNames = Object.keys(schema.properties ?? {});
      for (const propertyName of propertyNames) {
        const property = schema.properties[propertyName];
        if (!property.meta?.hidden && !property.meta.hideForList) {
          result.push({
            name: propertyName,
            title: property.title,
            checked: true,
          });
        }
      }
      return result;
    };
    const getClass = ({ row, column }) => {
      if (column.property) {
        column.order = sortColumns.value.get(column.property)?.toLowerCase() ?? '';
      }
    };
    const sortChange = async ({ column, prop, order }) => {
      if (order === null) {
        sortColumns.value.delete(prop);
      } else {
        sortColumns.value.set(prop, order);
      }
      // queryModel.value.sorting = Array.from(sortColumns.value)
      //   .map((o) => capitalize() + (o[1] === 'ascending' ? '' : ' DESC'))
      //   .join(',');
      await load();
    };
    const showColumn = (item, prop) => {
      return columns.value.some((o) => o.name === prop && o.checked);
    };
    const handleSelectionChange = (rows) => {
      selectedRows.value = rows;
    };
    const load = async () => {
      loading.value = true;
      try {
        const button = props.schema.meta?.buttons.find((o) => o.meta.command === 'search');
        const url = `/${button.meta.url}`;
        const method = button.meta.method;
        const postData = buildQuery();
        const data = (await request(method, url, postData)).data;
        if (data.error) {
          await ElMessageBox.confirm(data.error.data?.message ?? data.error.message ?? data.error.code, '提示', {
            type: 'error',
          });
        }
        let items = data.data.items;
        if (props.schema.meta.isTree) {
          items = listToTree(items);
        }
        tableData.value = items;
        pageModel.total = data.data.items.length;
        //data.value = listData;
        tableKey.value = !tableKey.value;
        // nextTick(() => {
        //   tableRef.value.doLayout();
        // });
      } catch (error) {
        console.log(error);
      } finally {
        await delay();
        loading.value = false;
      }
    };
    const reload = async () => {
      pageModel.pageIndex = 1;
      await load();
    };
    const onPageIndexChange = async () => await load();
    const onPageSizeChange = async () => {
      await reload();
    };
    const buttonClick = async (item, rows) => {
      editFormMode.value = item.meta.command;
      const showForm = ['create', 'update', 'details', 'import', 'export'].some((o) => o === item.meta.command);
      try {
        if (showForm) {
          editFormButton.value = item;
          editFormloading.value = true;
          editFormSchema.value = props.schema;
        }
        if (editFormMode.value === 'search') {
          await load();
        } else if (editFormMode.value === 'create') {
          editFormModel.value = schemaToModel(editFormSchema.value);
        } else if (editFormMode.value === 'details' || editFormMode.value === 'update') {
          const detailsButton = props.schema.meta.buttons.find((o) => o.meta.command === 'details');
          const url = `/${detailsButton?.meta?.url}`;
          const method = detailsButton.meta.method ?? 'POST';
          const result = await request(method, url, rows[0].id);
          editFormModel.value = result.data.data;
        } else if (editFormMode.value === 'import') {
          editFormSchema.value = useImport();
          editFormModel.value = schemaToModel(editFormSchema.value);
        } else if (editFormMode.value === 'export') {
          editFormSchema.value = useExport();
          editFormModel.value = schemaToModel(editFormSchema.value);
        } else if (
          editFormMode.value === 'delete' ||
          editFormMode.value === 'archive' ||
          editFormMode.value === 'unarchive'
        ) {
          try {
            await ElMessageBox.confirm(format(`确认${t(item.meta.command)}选中的%s行数据吗？`, rows.length), '提示', {
              type: 'warning',
            });
            loading.value = true;
            const url = `/${item?.meta?.url}`;
            const method = item.meta.method ?? 'POST';
            const data = rows.map((o) => o.id);
            const result = await request(method, url, data);
            if (!result.error) {
              pageModel.pageIndex = 1;
              await reload();
            } else {
              ElMessageBox.alert(result.message, '提示', { type: 'error' });
            }
          } catch (error) {
            if (error === 'cancel') {
              ElMessage({
                type: 'info',
                message: '操作取消',
              });
            }
          } finally {
            loading.value = false;
          }
        } else {
          context.emit('command', item, rows, load, showList);
        }
        if (showForm) {
          editFormTitle.value = `${t(editFormMode.value)}${editFormSchema.value?.title}`;
          dialogVisible.value = true;
        }
      } catch (error) {
        console.log(error);
      } finally {
        if (showForm) {
          await delay();
          editFormloading.value = false;
        }
      }
    };
    const editFormSubmit = async () => {
      if (editFormMode.value === 'create' || editFormMode.value === 'update') {
        try {
          // await editFormRef.value.submit();
          // return;
          const valid = await editFormRef.value.validate();
          if (valid) {
            await onClick(
              async () => {
                const button = props.schema.meta.buttons.find((o) => o.meta.command === editFormMode.value);
                const url = `/${button?.meta?.url}`;
                const method = button.meta.method ?? 'POST';
                const data = JSON.parse(JSON.stringify(editFormModel.value));
                const result = await request(method, url, data);
                if (!result.error) {
                  dialogVisible.value = false;
                  editFormMode.value = null;
                  await reload();
                } else {
                  if (result.code === 400) {
                    const modelErrors = JSON.parse(JSON.stringify(result.data));
                    editFormRef.value.setErrors(modelErrors);
                  } else {
                    ElMessageBox.alert(result.message, '提示', { type: 'error' });
                  }
                }
              },
              null,
              true,
            );
          }
        } catch (error) {
          console.log(error);
        } finally {
          editFormloading.value = false;
        }
      } else if (editFormMode.value === 'details') {
        dialogVisible.value = false;
        editFormMode.value = null;
      } else if (editFormMode.value === 'export') {
        try {
          loading.value = true;
          const url =
            config.buttons.find((o) => o.path === editFormMode.value)?.meta?.action +
            (exportModel.value.format ? 'csv' : 'xlsx');
          const method = 'POST';
          const queryData = buildQuery();
          const fields = Object.entries(props.schema.properties)
            .filter((o) => !o[1].meta.hideForList)
            .map((o) => {
              return { name: o[0], label: o[1].title };
            });
          if (exportModel.value.import_compat) {
            fields.unshift({ name: 'id', label: 'id' });
          }
          const data = export_data(
            config.model,
            queryData.params.kwargs.domain,
            fields,
            exportModel.value.import_compat,
            exportModel.value.currentPage ? tableData.value.map((o) => o.id) : false,
          );
          const formData = new FormData();
          formData.append('data', JSON.stringify(data));
          formData.append('token', 'dummy-because-api-expects-one');
          formData.append('csrf_token', tokenStore.csrfToken);
          const result = await request(method, url, formData);
          if (!result.error) {
            downloadFile(result.data, result.name);
          } else {
            ElMessageBox.alert(result.message, '提示', { type: 'error' });
          }
        } catch (error) {
          if (error === 'cancel') {
            ElMessage({
              type: 'info',
              message: '操作取消',
            });
          }
        } finally {
          loading.value = false;
        }
      } else if (editFormMode.value === 'import') {
        try {
          const valid = await editFormRef.value.validate();
          if (valid) {
            editFormloading.value = true;
            //
            const createIdResult = await request(
              'POST',
              'web/dataset/call_kw/base_import.import/create',
              import_id(config.model),
            );
            let importId = null;
            if (!createIdResult.error) {
              importId = createIdResult.data.result;
            } else {
              ElMessageBox.alert(createIdResult.message, '提示', { type: 'error' });
            }
            //
            const url = config.buttons.find((o) => o.path === editFormMode.value)?.meta?.action;
            const formData = new FormData();
            formData.append('id', importId);
            formData.append('model', config.model);
            formData.append('csrf_token', tokenStore.csrfToken);
            formData.append('ufile', importModel.value.fiels[0].raw);
            const result = await request('POST', url, formData);
            if (!result.error) {
              const fields = Object.entries(props.schema.properties)
                .filter((o) => !o[1].hideForTable)
                .map((o) => {
                  return o[0];
                });
              const result2 = await request(
                'POST',
                'web/dataset/call_kw/base_import.import/execute_import',
                execute_import(importId, fields, fields, {
                  has_headers: importModel.value.has_headers,
                }),
              );
              if (!result2.error) {
                editFormloading.value = false;
                dialogVisible.value = false;
                await load();
              } else {
                ElMessageBox.alert(result.message, '提示', { type: 'error' });
              }
            } else {
              ElMessageBox.alert(result.message, '提示', { type: 'error' });
            }
          }
        } catch (error) {
          console.log(error);
        } finally {
          editFormloading.value = false;
        }
      } else if (editFormMode.value === 'filter') {
        await load();
        dialogVisible.value = false;
      } else {
        try {
          console.log(editFormMode.value);
          const valid = await editFormRef.value.validate();
          if (valid) {
            editFormloading.value = true;
            const url = config.buttons.find((o) => o.path === editFormMode.value)?.meta?.action;
            let data = JSON.parse(JSON.stringify(editFormModel.value));
            if (editFormButton.value.meta.prepare?.constructor) {
              data = editFormButton.value.meta.prepare(data);
            }
            const result = await request('POST', url, data);
            if (!result.error) {
              ElMessage.success(result.message);
              editFormloading.value = false;
              dialogVisible.value = false;
              await load();
            } else {
              ElMessageBox.alert(result.message, '提示', { type: 'error' });
            }
          }
        } catch (error) {
          console.log(error);
        } finally {
          editFormloading.value = false;
        }
      }
    };

    const showList = async (value, nav, config) => {
      if (!subDrawer.value) {
        try {
          const targetRoute = findTargetRoute(nav);
          if (!targetRoute) {
            console.error('未找到目标路由');
            return;
          }

          // 对config进行处理
          const processedConfig = processConfig(config, targetRoute);

          subListQuery.value = {
            query: value,
            buttons: targetRoute.meta.children,
            config: processedConfig,
          };
          subDrawer.value = true;
        } catch (error) {
          console.error('处理列表显示时发生错误:', error);
          // 可以添加错误处理逻辑，例如用户提示等
        }
      }
    };

    // 从路由列表中查找目标路由
    const findTargetRoute = (nav) => {
      return router.getRoutes().find((o) => o.path === nav) || {};
    };

    // 处理config参数
    const processConfig = async (config, route) => {
      if (typeof config === 'string') {
        const module = await import(config);
        const configValue = module.default;

        return configValue.constructor === Function ? configValue(route.meta.businessType) : configValue;
      }
      return config;
    };
    const download = (url, filename) => {
      const downloadUrl = window.URL.createObjectURL(url);
      const link = document.createElement('a');
      link.href = downloadUrl;
      link.download = filename;
      link.click();
      window.URL.revokeObjectURL(downloadUrl);
    };
    const getButtonDisabled = (item) => {
      if (item.meta?.disabled) {
        if (item.meta.disabled.constructor === Function) {
          return item.meta.disabled(selectedRows.value, queryModel.value);
        }
        return item.meta.disabed;
      }
      if (item.meta.command === 'delete' || item.meta.command === 'archive' || item.meta.command === 'unarchive') {
        if (selectedRows.value.length === 0) {
          return true;
        }
      }
      if (item.meta.command === 'delete' || item.meta.command === 'unarchive') {
        if (!queryModel.value.isDeleted) {
          return true;
        }
      }
      if (item.meta.command === 'archive') {
        if (queryModel.value.isDeleted) {
          return true;
        }
      }
      return false;
    };
    function buildQuery() {
      const data = {
        includeAll: !!props.schema.meta.isTree,
        filters: [],
      };
      const queryValue = [];
      for (const [key, value] of Object.entries(JSON.parse(JSON.stringify(queryModel.value)))) {
        const propSchema = querySchema.value.properties[key];
        if (Array.isArray(value)) {
          if (value.length) {
            if (propSchema.input === 'daterange' || propSchema.input === 'datetimerange') {
              data.filters.push({
                logic: 'and',
                property: key,
                operator: '>=',
                value: value[0],
              });
              data.filters.push({
                logic: 'and',
                property: key,
                operator: '<',
                value: value[1],
              });
            } else {
              data.filters.push({
                logic: 'and',
                property: key,
                operator: 'in',
                value: value,
              });
            }
          }
        } else if (value?.constructor === Object) {
          // if(Object.keys(value).length)
          // {
          //   queryValue.push([key, value]);
          // }
        } else if (value || value?.constructor === Boolean) {
          data.filters.push({
            logic: 'and',
            property: key,
            operator: '=',
            value: value,
          });
        }
      }
      if (queryValue.length) {
        data.query = Object.fromEntries(queryValue);
      }
      const orderBy = Array.from(sortColumns.value)
        .map(([key, order]) => `${key} ${order.match(/(.*)ending/)[1]}`)
        .join(',');
      if (orderBy) {
        data.orderBy = orderBy;
      }
      return data;
    }

    let originalColumns = [];
    onMounted(async () => {
      if (route.meta.children?.length) {
        for (const item of route.meta.children) {
          if (item.meta.disabled?.constructor === String) {
            item.meta.disabled = await importFunction(item.meta.disabled);
          }
        }
      }
      //
      if (!props.schema.meta.isTree) {
        getSortModel(queryModel.value);
      }
      originalColumns = getColumns(props.schema);
      columns.value = JSON.parse(JSON.stringify(originalColumns));
      // if (props.query) {
      //   Object.assign(queryModel.value.query, props.query);
      // }
      if (!props.schema.meta?.disableQueryOnLoad) {
        await load();
      } else {
        loading.value = false;
      }
    });
    const queryFormFold = ref(true);
    const queryStyle = computed(() => {
      const height =
        useCssVar(`--el-component-size-${appStore.settings.size}`).value || useCssVar('--el-component-size').value;
      return {
        overflow: 'hidden',
        height: queryFormFold.value ? `calc(${height} + 18px)` : 'auto',
      };
    });

    const resetColumns = () => {
      if (originalColumns) {
        columns.value = JSON.parse(JSON.stringify(originalColumns));
      }
    };
    const queryFormRef = ref(null);
    const reset = async () => {
      queryFormRef.value.reset();
      await reload();
    };
    return {
      appStore,
      buttons,
      getButtonDisabled,
      //查询表单
      loading,
      reset,
      load,
      reload,
      querySchema,
      queryModel,
      queryFormRef,
      queryStyle,
      queryFormFold,
      //列表
      tableData,
      tableSchema,
      treeProps,
      tableKey,
      tableRef,
      sortChange,
      buttonClick,
      showColumn,
      selectedRows,
      handleSelectionChange,
      getClass,
      //分页
      pageModel,
      onPageSizeChange,
      onPageIndexChange,
      //对话框
      dialogVisible,
      editFormRef,
      editFormloading,
      editFormMode,
      editFormTitle,
      editFormSchema,
      editFormModel,
      editFormSubmit,
      //抽屉
      filterDrawer,
      columns,
      resetColumns,
      subDrawer,
      getProp,
      showList,
      subListQuery,
    };
  },
};
