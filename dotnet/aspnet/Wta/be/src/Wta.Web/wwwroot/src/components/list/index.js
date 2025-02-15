import AppFormInput from '@/components/form/form-input.js';
import AppForm from '@/components/form/index.js';
import SvgIcon from '@/components/icon/index.js';
import { DATETIME_FILENAME_FORMAT } from '@/constants/index.js';
import useExport from '@/models/export.js';
import useImport from '@/models/import.js';
import { useAppStore, useUserStore } from '@/store/index.js';
import request from '@/utils/request.js';
import { schemaToModel, toQuerySchema } from '@/utils/schema.js';
import { useCssVar } from '@vueuse/core';
import { ElMessage, ElMessageBox } from 'element-plus';
import { dayjs } from 'element-plus';
import { camelCase } from 'lodash';
import { downloadFile, importFunction } from 'utils';
import html, { getProp, delay, listToTree } from 'utils';
import { computed, onMounted, reactive, ref, shallowRef } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRoute, useRouter } from 'vue-router';

export default {
  name: 'AppList',
  components: {
    AppForm,
    AppFormInput,
    SvgIcon,
  },
  template: html`
    <div class="pb-5" v-loading="loading" element-loading-text="Loading...">
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
                <el-icon>
                  <svg-icon
                    :name="item.meta.icon??item.meta.command??item.path"
                  />
                </el-icon>
                <span>{{$t(item.meta.title)}}</span>
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
              {{$t('查询')}}
            </el-button>
            <el-button @click="reset(queryFormRef)" class="mb-5 ml-3">
              {{$t('重置')}}
            </el-button>
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
          <el-table-column
            v-if="!schema.disableSelection"
            fixed="left"
            type="selection"
            :selectable="schema.selectable"
          />
          <el-table-column type="index" :label="$t('行号')">
            <template #default="scope">
              {{ (pageModel.pageIndex - 1) * pageModel.pageSize + scope.$index +
              1 }}
            </template>
          </el-table-column>
          <template v-for="(item,key) in schema.properties" :key="key">
            <template v-if="item.navigation">
              <el-table-column :prop="key" :label="item.title">
                <template #default="scope">
                  {{getProp(scope.row,item.navigation)}}
                </template>
              </el-table-column>
            </template>
            <template v-else-if="item.oneToMany">
              <el-table-column :prop="key" :label="item.title">
                <template #default="scope">
                  <el-link
                    type="primary"
                    @click="showList({[key]:scope.row[key]},item.oneToMany,item.config)"
                  >
                    <app-form-input
                      mode="details"
                      :schema="item"
                      :prop="key"
                      v-model="scope.row"
                    />
                  </el-link>
                </template>
              </el-table-column>
            </template>
            <template v-else-if="item.link">
              <el-table-column :prop="key" :label="item.title">
                <template #default="scope">
                  <el-link
                    type="primary"
                    @click="buttonClick({path:key},[scope.row])"
                  >
                    {{scope.row[key]}}
                  </el-link>
                </template>
              </el-table-column>
            </template>
            <template v-else-if="item.type!=='object'&&!item.meta.hidden">
              <template v-if="item.type==='array'"></template>
              <template
                v-else-if="!item.meta.hideForList&&showColumn(item,key)"
              >
                <el-table-column
                  :prop="key"
                  :sortable="isSortable(item)"
                  :sort-orders="['descending', 'ascending', null]"
                >
                  <template #header="scope">{{item.title}}</template>
                  <template #default="scope">
                    <app-form-input
                      mode="details"
                      :schema="item"
                      :prop="key"
                      v-model="scope.row"
                    />
                  </template>
                </el-table-column>
              </template>
            </template>
            <template v-if="item.type==='object'&&false">
              <template v-for="(item2,key2) in item['properties']">
                <el-table-column :prop="key+'.'+key2">
                  <template #header="scope">{{item2.title}}</template>
                  <template #default="scope">
                    <template v-if="scope.row[key]">
                      <app-form-input
                        mode="details"
                        :schema="item2"
                        :prop="key2"
                        v-model="scope.row[key]"
                      />
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
                {{$t('操作')}}
                <el-icon class="el-icon--right"><ep-filter /></el-icon>
              </el-button>
            </template>
            <template #default="scope">
              <div class="flex">
                <template v-for="item in buttons">
                  <el-button
                    :class="item.meta.htmlClass??'is-plan'"
                    v-if="!item.meta.hidden&&item.meta.buttonType==='row'"
                    @click="buttonClick(item,[scope.row])"
                    :disabled="item.meta.disabled && item.meta.disabled(scope.row)"
                  >
                    <el-icon>
                      <svg-icon
                        :name="item.meta.icon??item.meta.command??item.path"
                      />
                    </el-icon>
                    <span>{{$t(item.meta.title)}}</span>
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
    <el-drawer
      v-model="filterDrawer"
      destroy-on-close
      @close="tableRef.doLayout()"
    >
      <template #header>
        <span class="el-dialog__title">{{$t('过滤')}}</span>
      </template>
      <el-scrollbar>
        <el-row>
          <el-col>
            <el-form inline>
              <div>
                <el-button
                  type="primary"
                  @click="columns.forEach(o=>o.checked=true)"
                >
                  {{$t('全选')}}
                </el-button>
                <el-button
                  type="primary"
                  @click="columns.forEach(o=>o.checked=!o.checked)"
                >
                  {{$t('反选')}}
                </el-button>
                <el-button type="primary" @click="resetColumns">
                  {{ $t('重置') }}
                </el-button>
              </div>
              <div
                v-for="item in columns"
                style="display:inline-block;padding:10px;width:50%;"
              >
                <el-checkbox
                  v-model="item.checked"
                  :label="item.title"
                  size="large"
                />
              </div>
            </el-form>
          </el-col>
        </el-row>
      </el-scrollbar>
      <template #footer>
        <span class="dialog-footer">
          <el-button type="primary" @click="filterDrawer=false">
            {{$t('确定')}}
          </el-button>
        </span>
      </template>
    </el-drawer>
    <!--通用对话框-->
    <el-dialog
      v-model="dialogVisible"
      :fullscreen="dialogFullscreen"
      align-center
      append-to-body
      destroy-on-close
      :close-on-click-modal="false"
    >
      <template #header="{ close, titleId, titleClass }">
        <span class="el-dialog__title">{{editFormTitle}}</span>
        <el-icon
          v-if="false"
          @click="dialogFullscreen=!dialogFullscreen"
          style="cursor:pointer;float:right;"
        >
          <svg-icon v-if="!dialogFullscreen" name="fullscreen" />
          <svg-icon v-else name="fullscreen-exit" />
        </el-icon>
      </template>
      <template #footer>
        <span class="dialog-footer">
          <el-button type="primary" @click="editFormSubmit">
            {{$t('确定')}}
          </el-button>
          <el-button
            v-if="editFormCommand!=='details'"
            @click="reset(editFormRef)"
            class="ml-3"
          >
            {{$t('重置')}}
          </el-button>
        </span>
      </template>
      <el-scrollbar>
        <el-row v-loading="editFormloading">
          <el-col style="max-height:calc(100vh - 180px);">
            <app-form
              :disabled="editFormCommand==='details'"
              :mode="editFormCommand"
              ref="editFormRef"
              inline
              label-position="right"
              :hideButton="true"
              :schema="editFormSchema"
              v-model="editFormModel"
              style="height:100%;"
            >
              <!--下载导入母版-->
              <template v-if="editFormCommand==='import'">
                <template
                  v-for="item in buttons.filter(o=>o.meta.hidden&&o.meta.command==='import-template')"
                >
                  <el-form-item label=" ">
                    <el-button @click="buttonClick(item)">
                      <el-icon>
                        <svg-icon
                          :name="item.meta.icon??item.meta.command??item.path"
                        />
                      </el-icon>
                      <span>{{$t(item.meta.title)}}</span>
                    </el-button>
                  </el-form-item>
                </template>
              </template>
            </app-form>
          </el-col>
        </el-row>
      </el-scrollbar>
    </el-dialog>
  `,
  props: ['schema'],
  emits: ['command'],
  setup(props, context) {
    // 初始化
    const appStore = useAppStore();
    const userStore = useUserStore();
    // const tokenStore = useTokenStore();
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
    const dialogFullscreen = ref(false);
    const route = useRoute();
    const router = useRouter();
    const { t } = useI18n();
    const sortColumns = ref(new Map());
    const tableSchema = ref({});
    const tableData = shallowRef([]);
    const editFormRef = ref(null);
    const editFormloading = ref(false);
    const editFormCommand = ref(null);
    const editFormTitle = ref('');
    const editFormSchema = ref(null);
    const editFormModel = ref(null);
    const editFormButton = ref(null);
    const buttons = ref(
      props.schema.meta?.buttons.filter((o) => userStore.hasPermission(o.meta)),
    );
    const getSortModel = () => {
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
    const getClass = ({ column }) => {
      if (column.property) {
        column.order =
          sortColumns.value.get(column.property)?.toLowerCase() ?? '';
      }
    };
    const sortChange = async ({ prop, order }) => {
      if (order === null) {
        sortColumns.value.delete(prop);
      } else {
        sortColumns.value.set(prop, order);
      }
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
        const button = props.schema.meta?.buttons.find(
          (o) => o.meta.command === 'search',
        );
        const url = `/${button.meta.url}`;
        const method = button.meta.method;
        const postData = buildQuery();
        const data = (await request(method, url, postData)).data;
        if (data.error) {
          await ElMessageBox.confirm(
            data.error.data?.message ?? data.error.message ?? data.error.code,
            '提示',
            {
              type: 'error',
            },
          );
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
      if (item.meta.command === 'import-template') {
        const result = await request(
          item.meta.method ?? 'POST',
          `/${item.meta.url}`,
        );
        if (!result.error) {
          downloadFile(result.data, result.name);
        }
        return;
      }
      editFormCommand.value = item.meta.command;
      const showForm = ['create', 'update', 'details', 'import', 'export'].some(
        (o) => o === item.meta.command,
      );
      try {
        if (showForm) {
          editFormButton.value = item;
          editFormloading.value = true;
          editFormSchema.value = props.schema;
        }
        if (editFormCommand.value === 'search') {
          await load();
        } else if (editFormCommand.value === 'create') {
          editFormModel.value = schemaToModel(editFormSchema.value);
        } else if (
          editFormCommand.value === 'details' ||
          editFormCommand.value === 'update'
        ) {
          const detailsButton = props.schema.meta.buttons.find(
            (o) => o.meta.command === 'details',
          );
          const url = `/${detailsButton?.meta?.url}`;
          const method = detailsButton.meta.method ?? 'POST';
          const result = await request(method, url, rows[0].id);
          editFormModel.value = result.data.data;
        } else if (editFormCommand.value === 'import') {
          editFormSchema.value = useImport();
          editFormModel.value = schemaToModel(editFormSchema.value);
        } else if (editFormCommand.value === 'export') {
          editFormSchema.value = useExport();
          editFormModel.value = schemaToModel(editFormSchema.value);
          editFormModel.value.includeAll = !!props.schema.meta.isTree;
          editFormModel.value.name = `${props.schema?.title}_${dayjs().format(DATETIME_FILENAME_FORMAT)}`;
        } else if (
          editFormCommand.value === 'delete' ||
          editFormCommand.value === 'archive' ||
          editFormCommand.value === 'unarchive'
        ) {
          try {
            await ElMessageBox.confirm(
              t('确认{0}选中的{1}行数据吗？', [
                t(item.meta.title),
                rows.length,
              ]),
              '提示',
              {
                type: 'warning',
              },
            );
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
                message: t('操作取消'),
              });
            }
          } finally {
            loading.value = false;
          }
        } else {
          context.emit('command', item, rows, load, showList);
        }
        if (showForm) {
          editFormTitle.value = `${t(item.meta.title)}${props.schema?.title}`;
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
      if (editFormCommand.value === 'details') {
        dialogVisible.value = false;
        return;
      }
      try {
        await editFormRef.value.validate();
        const button = props.schema.meta.buttons.find(
          (o) => o.meta.command === editFormCommand.value,
        );
        const url = `/${button?.meta?.url}`;
        const method = button.meta.method ?? 'POST';
        let data = null;
        if (
          editFormCommand.value === 'search' ||
          editFormCommand.value === 'export'
        ) {
          data = buildQuery();
          if (editFormCommand.value === 'export') {
            Object.assign(data, editFormModel.value);
          }
        } else if (editFormCommand.value === 'import') {
          data = new FormData();
          for (const [key, value] of Object.entries(editFormModel.value)) {
            data.append(key, value);
          }
        } else {
          data = editFormModel.value;
        }
        const result = await request(method, url, data);
        if (!result.error) {
          dialogVisible.value = false;
          await reload();
          if (editFormCommand.value === 'export') {
            downloadFile(result.data, result.name);
          }
        } else {
          if (result.code === 400) {
            const modelErrors = JSON.parse(JSON.stringify(result.data));
            editFormRef.value.setErrors(modelErrors);
          } else {
            ElMessageBox.alert(result.message, t('提示'), { type: 'error' });
          }
        }
        // dialogVisible.value = false;
      } catch (error) {
        if (error === 'cancel') {
          ElMessage({
            type: 'info',
            message: t('操作取消'),
          });
        }
      } finally {
        editFormloading.value = false;
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

        return configValue.constructor === Function
          ? configValue(route.meta.businessType)
          : configValue;
      }
      return config;
    };

    const getButtonDisabled = (item) => {
      if (item.meta?.disabled) {
        if (item.meta.disabled.constructor === Function) {
          return item.meta.disabled(selectedRows.value, queryModel.value);
        }
        return item.meta.disabed;
      }
      if (
        item.meta.command === 'delete' ||
        item.meta.command === 'archive' ||
        item.meta.command === 'unarchive'
      ) {
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
      for (const [key, value] of Object.entries(
        JSON.parse(JSON.stringify(queryModel.value)),
      )) {
        const propSchema = querySchema.value.properties[key];
        if (Array.isArray(value)) {
          if (value.length) {
            if (
              propSchema.input === 'daterange' ||
              propSchema.input === 'datetimerange'
            ) {
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
        useCssVar(`--el-component-size-${appStore.settings.size}`).value ||
        useCssVar('--el-component-size').value;
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
    const reset = async (formRef) => {
      formRef.reset();
      if (!dialogVisible.value) {
        await reload();
      }
    };
    const isSortable = (item) => {
      if (props.schema.meta?.isTree) {
        return false;
      }
      if (item.meta.skipSorting) {
        return false;
      }
      return 'custom';
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
      dialogFullscreen,
      editFormRef,
      editFormloading,
      editFormCommand,
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
      isSortable,
    };
  },
};
