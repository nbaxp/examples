import request from '@/utils/request.js';
import { normalize, schemaToModel } from '@/utils/schema.js';
import AppForm from '@/components/form/index.js';
import { ElMessageBox } from 'element-plus';
import Chart from '@/components/chart/index.js';
import html from 'utils';
import { ref, onMounted, watch } from 'vue';
import { useI18n } from 'vue-i18n';
import { dayjs, ElMessage, useFormItem } from 'element-plus';
import { DATETIME_VALUE_FORMAT } from '@/constants/index.js';

export default {
  components: { AppForm, Chart },
  template: html`
    <el-row v-loading="loading">
      <app-form
        ref="formRef"
        inline
        v-if="model"
        v-model="model"
        :schema="schema"
        @success="success"
        :hideButton="true"
      >
        <el-button @click="submit" class="el-button--primary mb-5">
          {{$t('查询')}}
        </el-button>
      </app-form>
    </el-row>
    <el-tabs type="border-card" v-model="currentTab" @tab-change="tabChange">
      <el-tab-pane name="DataEntry" label="数据编辑">
        <div
          style="flex-wrap: wrap; align-items: center; justify-content: space-between;"
        >
          <template v-for="item in oeeData">
            <div style="min-width: 25%; flex: 1 1 auto; padding: 1rem;">
              <el-card>
                <el-form>
                  <el-form-item label="状态">
                    <el-select v-model="item.statusId">
                      <el-option
                        v-for="item2 in statusList"
                        :key="item2.id"
                        :label="item2.name"
                        :value="item2.id"
                      />
                    </el-select>
                  </el-form-item>
                  <el-form-item label="班次">
                    <el-select v-model="item.shiftId">
                      <el-option
                        v-for="item2 in shiftList"
                        :key="item2.id"
                        :label="item2.name"
                        :value="item2.id"
                      />
                    </el-select>
                  </el-form-item>
                  <el-form-item label="时间段">
                    <el-date-picker
                      v-model="item.range"
                      type="datetimerange"
                      range-separator="-"
                      start-placeholder="开始"
                      end-placeholder="截至"
                      :value-format="DATETIME_VALUE_FORMAT"
                    />
                  </el-form-item>
                  <el-form-item label="时长(分钟)">
                    {{dayjs(item.range[1]).diff(dayjs(item.range[0]),'minute')}}
                  </el-form-item>
                  <template
                    v-if="statusList.find(o=>o.name==='正常生产'&&o.id===item.statusId)"
                  >
                    <el-form-item label="总产出">
                      <el-input v-model="item.total" />
                    </el-form-item>
                    <el-form-item label="设备相关废品">
                      <el-input v-model="item.eequipmentScrap" />
                    </el-form-item>
                    <el-form-item label="设备无关废品">
                      <el-input v-model="item.nonEequipmentScrap" />
                    </el-form-item>
                  </template>
                  <template v-else>
                    <el-form-item label="原因">
                      <el-select v-model="item.reasonId">
                        <el-option
                          v-for="item2 in reasonList"
                          :key="item2.id"
                          :label="item2.name"
                          :value="item2.id"
                        />
                      </el-select>
                    </el-form-item>
                  </template>
                  <el-form-item label="备注">
                    <el-input v-model="item.remark" />
                  </el-form-item>
                </el-form>
              </el-card>
            </div>
          </template>
        </div>
      </el-tab-pane>
      <el-tab-pane name="OeeAnalysis" label="OEE分析">OEE分析</el-tab-pane>
      <el-tab-pane name="ShiftEffectiveness" label="班次效能">性能</el-tab-pane>
    </el-tabs>
  `,
  setup() {
    const { t } = useI18n();
    const schema = ref(null);
    const model = ref(null);
    const formRef = ref(null);
    const loading = ref(false);
    const currentTab = ref('DataEntry');
    const oeeData = ref([]);
    const shiftList = ref([]);
    const statusList = ref([]);
    const reasonList = ref([]);

    const validate = async () => {
      return formRef.value.validate();
    };
    const load = async () => {
      loading.value = true;
      try {
        const valid = await validate();
        if (valid) {
          const url = 'oee-analysis/index';
          const result = (await request('POST', url, model.value)).data;
          if (!result.error) {
            const data = result.data;
            console.log(data);
          } else {
            ElMessageBox.alert(result.message, t('提示'), { type: 'error' });
          }
        }
      } finally {
        loading.value = false;
      }
    };
    const reset = async (formRef) => {
      formRef.reset();
      await load();
    };
    const submit = async () => {
      await formRef.value.submit();
    };
    const success = (result) => {
      const data = result.data;
      console.log(data);
      if (currentTab.value === 'DataEntry') {
        oeeData.value = data.map((o) => {
          return { range: [o.start, o.end], ...o };
        });
      }
    };
    const tabChange = async (name) => {
      currentTab.value = name;
      if (name === 'OEE') {
        schema.value.meta.url = 'oee-analysis/index';
        await submit();
      } else if (name === '可用性') {
        schema.value.meta.url = 'oee-dashboard/availability';
        await submit();
      }
    };

    onMounted(async () => {
      const result = await request('GET', 'oee-analysis/schema');
      schema.value = normalize(result.data.data);
      model.value = result.data.data?.model ?? schemaToModel(schema.value);
      schema.value.meta.url = 'oee-analysis/index';
      shiftList.value = (
        await request('POST', 'oee-shift/search', { includeAll: true })
      ).data.data.items;
      statusList.value = (
        await request('POST', 'oee-status/search', { includeAll: true })
      ).data.data.items;
      reasonList.value = (
        await request('POST', 'oee-reason/search', { includeAll: true })
      ).data.data.items;
    });
    return {
      DATETIME_VALUE_FORMAT,
      formRef,
      currentTab,
      tabChange,
      load,
      reset,
      schema,
      model,
      loading,
      submit,
      success,
      oeeData,
      shiftList,
      statusList,
      reasonList,
      dayjs,
    };
  },
};
