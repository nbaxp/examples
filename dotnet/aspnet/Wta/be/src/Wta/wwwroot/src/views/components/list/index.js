import { useAppStore, useTokenStore } from '@/store/index.js';
import request, { getUrl } from '@/utils/request.js';
import { schemaToModel } from '@/utils/schema.js';
import AppFormInput from '@/views/components/form/form-input.js';
import AppForm from '@/views/components/form/index.js';
import SvgIcon from '@/views/components/icon/index.js';
import { useCssVar } from '@vueuse/core';
import { ElMessage, ElMessageBox } from 'element-plus';
import * as jsondiffpatch from 'jsondiffpatch';
import { camelCase, capitalize } from 'lodash';
import { downloadFile, format, importFunction } from 'utils';
import html, { getProp, listToTree } from 'utils';
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
  template: html`<div class="pb-5" v-loading="loading">
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
    <app-form
      inline
      mode="query"
      :schema="schema"
      v-model="queryModel"
      @submit="load"
      :hideButton="true"
      :isQueryForm="true"
      class="query"
      label-position="right"
      :style="queryStyle"
    >
      <template v-for="item in filterList.filter(o=>!o.hidden&&o.readOnly)">
        <template v-if="schema.properties[item.column]?.title">
          <el-form-item :label="item.title??schema.properties[item.column].title">
            <app-form-input v-model="item" :schema="schema.properties[item.column]" prop="value" mode="query" />
          </el-form-item>
        </template>
        <div v-else>{{item.column}}</div>
      </template>
    </app-form>
    <el-row>
      <el-col>
        <template v-for="item in buttons">
          <el-button
            v-if="!item.meta.hidden&&item.meta.buttonType==='table'"
            @click="click(item,selectedRows)"
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
        <el-button v-if="false" @click="click('filter',selectedRows)">
          <el-icon><ep-filter /></el-icon>
          <span>{{$t('筛选')}}</span>
        </el-button>
        <slot name="tableButtons" :rows="selectedRows"></slot>
      </el-col>
    </el-row>
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
              <el-link type="primary" @click="click({path:key},[scope.row])">
                {{scope.row[key]}}
              </el-link>
            </template>
          </el-table-column>
        </template>
        <template v-else-if="item.type!=='object'&&!item.meta.hidden">
          <template v-if="!item.hideForList&&showColumn(item,key)">
            <el-table-column :prop="key" sortable="custom" :sort-orders="['descending', 'ascending', null]">
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
                @click="click(item,[scope.row])"
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
<!--filter drawer-->
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
<el-drawer :close-on-click-modal="false" v-model="subDrawer" destroy-on-close size="50%">
  <el-scrollbar>
    <app-list v-if="subDrawer" :query="subListQuery" :buttons="subListQuery.buttons" :config="subListQuery.config" />
  </el-scrollbar>
  <template #footer>
    <span class="dialog-footer">
      <el-button type="primary" @click="subDrawer=false">{{$t('confirm')}}</el-button>
    </span>
  </template>
</el-drawer>
<el-dialog v-model="dialogVisible" align-center destroy-on-close :close-on-click-modal="false" style="max-height:100%;">
  <template #header><span class="el-dialog__title">{{editFormTitle}}</span></template>
  <template #footer>
    <span class="dialog-footer">
      <el-button type="primary" @click="submit">{{$t('confirm')}}</el-button>
    </span>
  </template>
  <el-row v-loading="editFormloading">
    <el-col>
      <el-scrollbar>
        <template v-if="editFormMode==='create'||editFormMode==='update'||editFormMode==='details'">
          <app-form
            :disabled="editFormMode==='details'"
            :mode="editFormMode"
            ref="editFormRef"
            inline
            label-position="left"
            :hideButton="true"
            :schema="editFormSchema"
            v-model="editFormModel"
            style="height:100%;"
          />
        </template>
        <template v-else-if="editFormMode==='export'">
          <app-form
            ref="exportFormRef"
            mode="update"
            label-position="left"
            :schema="config.export?.schema"
            v-model="exportModel"
            :hideButton="true"
            :isQueryForm="true"
            style="height:100%;"
          ></app-form>
        </template>
        <template v-else-if="editFormMode==='import'">
          <app-form
            ref="importFormRef"
            mode="update"
            label-position="left"
            :schema="config.import?.schema"
            v-model="importModel"
            :hideButton="true"
            :isQueryForm="true"
            style="height:100%;"
          ></app-form>
        </template>
        <template v-else-if="editFormMode==='filter'">
          <el-form :model="filterList" inline class="filter">
            <el-row v-for="(item,index) in filterList.filter(o=>!o.hidden)">
              <el-col :span="6">
                <el-select clearable :disabled="item.readOnly" v-model="item.column" :placeholder="$t('字段')">
                  <template v-for="(value, prop) in config.query.schema.properties">
                    <el-option :value="prop" :label="value.title" />
                  </template>
                </el-select>
              </el-col>
              <el-col :span="6" v-if="item.column">
                <el-select clearable :disabled="item.readOnly" v-model="item.action" :placeholder="$t('操作符')">
                  <el-option
                    v-for="item in getOperators(config.edit.schema.properties[item.column])"
                    :value="item.value"
                    :label="item.label"
                  />
                </el-select>
              </el-col>
              <el-col :span="10" v-if="item.column">
                <app-form-input v-model="item" :schema="config.edit.schema.properties[item.column]" prop="value" />
              </el-col>
              <el-col :span="2" v-if="!item.readOnly&&item.action">
                <el-button circle @click="filterList.splice(index, 1)">
                  <template #icon>
                    <ep-close />
                  </template>
                </el-button>
              </el-col>
            </el-row>
            <el-row>
              <el-col>
                <el-button circle @click="pushfilterList">
                  <template #icon>
                    <ep-plus />
                  </template>
                </el-button>
              </el-col>
            </el-row>
          </el-form>
        </template>
        <template v-else>
          <app-form
            ref="editFormRef"
            mode="editFormMode"
            label-position="left"
            :schema="editFormSchema"
            v-model="editFormModel"
            :hideButton="true"
            inline
            style="height:100%;"
          ></app-form>
        </template>
      </el-scrollbar>
    </el-col>
  </el-row>
</el-dialog>`,
  styles: html`<style>
    /* .el-form.filter .el-col {
      padding: 5px;
    }
    dl.upload {
      min-height: 100%;
    }
    dl.upload dt {
      font-weight: bold;
      line-height: 3em;
    }
    dl.upload dd {
      line-height: 2em;
    }
    dl.upload .el-form-item {
      width: 300px;
    }
    div.upload {
      width: 100%;
    } */
  </style>`,
  props: ['schema'],
  emits: ['command'],
  setup(props, context) {
    // 初始化
    const appStore = useAppStore();
    const tokenStore = useTokenStore();
    const loading = ref(true);
    const listScrollbarRef = ref(null);
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
    const tableKey = ref(false);
    const tableRef = ref(null);
    const uploadRef = ref(null);
    const columns = ref([]);
    const filterDrawer = ref(false);
    const subDrawer = ref(false);
    const subListQuery = ref(props.query ?? {});
    const tableLoading = ref(false);
    const selectedRows = ref([]);
    const dialogVisible = ref(false);
    const route = useRoute();
    const router = useRouter();
    const { t } = useI18n();
    // 注释一下代码暂停权限验证
    // const buttons = ref(props.buttons ?? route.meta.children.filter((o) => o.meta.hasPermission));
    // 添加下行代码暂停权限验证
    const buttons = ref(props.schema.meta?.buttons ?? route.meta.children);
    const queryModel = ref(schemaToModel(props.schema));
    watch(queryModel.value, async (value, oldValue, a) => {
      if (props.schema.autoSubmit) {
        await load();
      }
    });
    const sortColumns = ref(new Map());
    const querySchema = ref(props.querySchema);
    const filterList = ref([]);
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
    const tempModel = ref(null);
    const versions = ref([]);
    const onClick = async (method, confirMmessage = '确认操作吗？', reload = true) => {
      try {
        if (confirMmessage) {
          await ElMessageBox.confirm(confirMmessage, '提示', {
            type: 'warning',
          });
        }
        tableLoading.value = true;
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
        tableLoading.value = false;
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
        if (!property.meta?.hidden) {
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
    const getFilters = (item, prop) => {
      if (item.input === 'select' && item.options) {
        return item.options.map((o) => ({ text: o.label, value: o.value }));
      }
      return null;
    };
    const filterHandler = (value, row, column) => {
      return row[column.property] === value;
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
          items = listToTree(items, props.schema.meta.tree);
        }
        tableData.value = items;
        pageModel.total = data.data.items.length;
        //data.value = listData;
        tableKey.value = !tableKey.value;
        // nextTick(() => {
        //   tableRef.value.doLayout();
        //   nextTick(() => listScrollbarRef.value.update());
        // });
      } catch (error) {
        console.log(error);
      } finally {
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
    const click = async (item, rows) => {
      editFormButton.value = item;
      editFormloading.value = true;
      editFormMode.value = item.meta.command;
      if (editFormMode.value === 'search') {
        //list
        await load();
      } else if (editFormMode.value === 'create' || editFormMode.value === 'update') {
        editFormSchema.value = props.schema;
        //create
        if (editFormMode.value === 'create') {
          editFormModel.value = schemaToModel(editFormSchema.value);
        } else {
          //const url = format(config.edit.detailsUrl, rows[0].id);
          tempModel.value = JSON.parse(JSON.stringify(rows[0]), (k, v) => {
            if (v === false && editFormSchema.value[k]?.type !== 'boolean') {
              return null;
            }
            return v;
          });
          editFormModel.value = { ...tempModel.value };
          //(await request(config.edit.detailsMethod ?? 'POST', url)).data;
          editFormModel.value.id = rows[0].id;
        }
        editFormTitle.value = `${t(item.path)}${editFormSchema.value.title}`;
        dialogVisible.value = true;
      } else if (editFormMode.value === 'delete') {
        try {
          await ElMessageBox.confirm(format('确认删除选中的%s行数据吗？', rows.length), '提示', {
            type: 'warning',
          });
          tableLoading.value = true;
          const url = item?.meta?.action;
          const method = config.edit.deleteMethod ?? 'POST';
          const data = unlink(
            config.model,
            rows.map((o) => o.id),
          );
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
          tableLoading.value = false;
        }
      } else if (editFormMode.value === 'archive' || editFormMode.value === 'unarchive') {
        try {
          await ElMessageBox.confirm(
            `确认${editFormMode.value === 'archive' ? '归档' : '激活'}选中的${rows.length}行数据吗？`,
            '提示',
            {
              type: 'warning',
            },
          );
          tableLoading.value = true;
          const url = item?.meta?.action;
          const method = config.edit.deleteMethod ?? 'POST';
          const data = (item.path === 'archive' ? action_archive : action_unarchive)(
            config.model,
            rows.map((o) => o.id),
          );
          const result = await request(method, url, data);
          if (!result.error) {
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
          tableLoading.value = false;
        }
      } else if (editFormMode.value === 'export') {
        // if (item.meta.pattern === 'paged') {
        //   const url = config.edit.exportUrl;
        //   const method = config.edit.exportMethod;
        //   const postData = buildQuery();
        //   await onClick(async () => {
        //     const response = await request(method, url, postData);
        //     if (!response.errors) {
        //       window.open(getUrl(`settleaccount/getblobfile/download/${response.data}`));
        //     }
        //   }, '确认导出?');
        // } else if (item.meta.pattern === 'file') {
        //   window.open(getUrl(`settleaccount/getblobfile/download/${rows[0].downFileName}`));
        // } else if (item.meta.pattern === 'row') {
        //   const url = config.edit.exportUrl;
        //   const method = config.edit.exportMethod ?? 'POST';
        //   const postData = {
        //     [item.meta.key]: rows[0][item.meta.key],
        //   };
        //   const response = await request(method, url, postData);
        //   if (!response.errors) {
        //     window.open(getUrl(`settleaccount/getblobfile/download/${response.data}`));
        //   }
        // } else {
        //   console.log(item);
        // }
        //exportModel
        try {
          editFormSchema.value = config.export.schema;
          exportModel.value = schemaToModel(editFormSchema.value);
          editFormloading.value = true;
          editFormTitle.value = `${t(item.path)}${editFormSchema.value.title}`;
          dialogVisible.value = true;
        } catch (e) {
          console.log(e);
        } finally {
          editFormloading.value = false;
        }
      } else if (editFormMode.value === 'import') {
        //import
        try {
          editFormSchema.value = config.import.schema;
          importModel.value = schemaToModel(config.import.schema);
          editFormloading.value = true;
          editFormTitle.value = `${t(item.path)}${editFormSchema.value.title}`;
          dialogVisible.value = true;
        } catch (e) {
          console.log(e);
        } finally {
          editFormloading.value = false;
        }
      } else if (editFormMode.value === 'details') {
        editFormSchema.value = props.schema;
        editFormTitle.value = t('详情');
        const url = `/${item.meta.url}`;
        const method = item.meta.method;
        const result = await request(method, url, rows[0].id);
        if (!result.error) {
          editFormModel.value = result.data.data;
        }
        dialogVisible.value = true;
      } else if (item === 'filter') {
        editFormTitle.value = t('自定义查询');
        dialogVisible.value = true;
      } else if (props.schema[item.path]) {
        try {
          editFormloading.value = true;
          editFormSchema.value = config[item.path].schema;
          editFormMode.value = item.path;
          editFormTitle.value = editFormSchema.value.title;
          tempModel.value = JSON.parse(JSON.stringify(rows[0]), (k, v) => {
            if (v === false && editFormSchema.value[k]?.type !== 'boolean') {
              return null;
            }
            return v;
          });
          editFormModel.value = schemaToModel(editFormSchema.value);
          for (const prop in editFormModel.value) {
            const from = editFormSchema.value.properties[prop].from ?? prop;
            if (Object.hasOwn(tempModel.value, from)) {
              editFormModel.value[prop] = tempModel.value[from];
            }
          }
          dialogVisible.value = true;
        } catch (e) {
          console.log(e);
        } finally {
          editFormloading.value = false;
        }
      } else {
        context.emit('command', item, rows, load, showList);
      }
      editFormloading.value = false;
    };
    const submit = async () => {
      if (editFormMode.value === 'create' || editFormMode.value === 'update') {
        try {
          const valid = await editFormRef.value.validate();
          if (valid) {
            await onClick(
              async () => {
                const url = schema.meta.buttons.find((o) => o.path === editFormMode.value)?.meta?.action;
                // if (editFormMode.value === "update") {
                // 	url = format(url, editFormModel.value.id);
                // }
                const method =
                  (editFormMode.value === 'create' ? config.edit.createMethod : config.edit.updateMethod) ?? 'post';
                let modelData = JSON.parse(JSON.stringify(editFormModel.value));
                let changed = true;
                let id = null;
                if (editFormMode.value === 'update') {
                  id = modelData.id;
                  const patchData = jsondiffpatch.diff(tempModel.value, modelData);
                  if (!patchData) {
                    changed = false;
                  } else {
                    const patchModel = {};
                    for (const key in patchData) {
                      if (Object.hasOwnProperty.call(patchData, key)) {
                        patchModel[key] = patchData[key][1];
                      }
                    }
                    modelData = patchModel;
                  }
                }
                if (changed) {
                  const data = web_save(config.model, [[id], modelData], config.edit.schema.properties);
                  const result = await request(method, url, data);
                  if (!result.error) {
                    dialogVisible.value = false;
                    editFormMode.value = null;
                    await reload();
                  } else {
                    ElMessageBox.alert(result.message, '提示', { type: 'error' });
                  }
                } else {
                  ElMessageBox.alert('没有编辑数据，请编辑后保存', '提示', { type: 'error' });
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
          tableLoading.value = true;
          const url =
            config.buttons.find((o) => o.path === editFormMode.value)?.meta?.action +
            (exportModel.value.format ? 'csv' : 'xlsx');
          const method = 'POST';
          const queryData = buildQuery();
          const fields = Object.entries(props.schema.properties)
            .filter((o) => !o[1].hideForTable)
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
          tableLoading.value = false;
        }
      } else if (editFormMode.value === 'import') {
        try {
          const valid = await importFormRef.value.validate();
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
      if ('disabled' in item.meta) {
        if (item.meta.disabled.constructor === Function) {
          return item.meta.disabled(selectedRows.value, queryModel.value);
        }
        return item.meta.disabed;
      }
      return false;
      //   if (src) {
      //     //item.meta.disabled && item.meta.disabled.constructor === Function && item.meta.disabled(selectedRows, queryModel);
      //     const method = await importFunction(src);
      //     return src.startsWith('async') ? await method(row) : method(row);
      //   }
      // return false;
    };
    const pushfilterList = () => {
      filterList.value.push({
        logic: 'and',
        column: '',
        action: '=',
        value: null,
      });
    };
    const operators = [
      {
        value: '=',
        label: '等于',
      },
      {
        value: '!=',
        label: '不等于',
      },
      {
        value: '>',
        label: '大于',
      },
      {
        value: '<',
        label: '小于',
      },
      {
        value: '>=',
        label: '大于等于',
      },
      {
        value: '<=',
        label: '小于等于',
      },
      {
        value: 'ilike',
        label: '包含',
      },
      {
        value: 'not ilike',
        label: '不包含',
      },
      {
        value: 'in',
        label: '在',
      },
      {
        value: 'not in',
        label: '不在',
      },
    ];
    const getOperators = (schema) => {
      const values = ['=', '!='];
      if (schema.type === 'string') {
        values.push('ilike', 'not ilike');
        if (schema.input && ['year', 'month', 'date', 'datetime'].includes(schema.input)) {
          values.push('>', '<', '>=', '<=');
        }
      } else if (schema.type === 'array') {
        values.push('in', 'not in');
      } else {
        values.push('>', '<', '>=', '<=');
      }
      return operators;
    };
    function buildQuery() {
      // const specification = props.schema.properties;
      // const limit = pageModel.pageSize;
      // const offset = (pageModel.pageIndex - 1) * pageModel.pageSize;
      // const order = Array.from(sortColumns.value)
      //   .map((o) => `${o[0]} ${o[1] === 'ascending' ? 'ASC' : 'DESC'}`)
      //   .join(',');
      // const domain = filterList.value
      //   .filter((o) => {
      //     return o.column && o.action && (o.value || o.value === false);
      //   })
      //   .map((o) => {
      //     return [o.column, o.action, o.value];
      //   });
      // for (const key of Object.keys(queryModel.value)) {
      //   const schema = props.schema.properties[key];
      //   if (schema) {
      //     const value = queryModel.value[key];
      //     const type = schema.type ?? 'string';
      //     const input = schema.input ?? 'text';
      //     if (value !== null && (schema.type !== 'array' || value.length > 0)) {
      //       if (type !== 'boolean' || value !== false || schema.sendFalse) {
      //         let action = schema.action ?? null;
      //         if (action === null) {
      //           if (input === 'select') {
      //             action = '=';
      //           } else {
      //             action = 'ilike';
      //           }
      //         }
      //         domain.push([key, action, value]);
      //       }
      //     }
      //   }
      // }
      // return web_search_read(config.model, specification, offset, limit, domain, order);
      const data = {
        includeAll: !!props.schema.meta.isTree,
        query: {},
      };
      for (const [key, value] of Object.entries(unref(queryModel))) {
        if (key !== 'totalCount' && key !== 'items' && key !== 'pageSizeOptions') {
          if (value !== null) {
            data.query[key] = value;
          }
        }
      }
      data.orderBy = Object.entries(sortColumns.value)
        .map(([key, order]) => `${key} ${order}`)
        .join(',');
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
      getSortModel(queryModel.value);
      filterList.value = props.schema.meta?.filters ?? [];
      for (const o of filterList.value) {
        if (o.default) {
          o.value = o.default.constructor === Function ? o.default() : o.default;
        }
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
    return {
      appStore,
      loading,
      listScrollbarRef,
      reload,
      onClick,
      queryModel,
      buildQuery,
      pageModel,
      treeProps,
      tableKey,
      tableRef,
      uploadRef,
      tableLoading,
      columns,
      showColumn,
      filterDrawer,
      subDrawer,
      dialogVisible,
      selectedRows,
      querySchema,
      filterList,
      tableSchema,
      buttons,
      tableData,
      getClass,
      sortChange,
      getProp,
      importFormRef,
      editFormRef,
      editFormloading,
      editFormMode,
      editFormTitle,
      editFormSchema,
      editFormModel,
      exportModel,
      importModel,
      onPageSizeChange,
      onPageIndexChange,
      handleSelectionChange,
      load,
      click,
      submit,
      showList,
      subListQuery,
      getButtonDisabled,
      versions,
      pushfilterList,
      getOperators,
      getFilters,
      filterHandler,
      tempModel,
      queryFormFold,
      queryStyle,
      resetColumns,
    };
  },
};
