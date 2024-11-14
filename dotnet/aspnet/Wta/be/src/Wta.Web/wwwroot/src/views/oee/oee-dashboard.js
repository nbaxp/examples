import request from '@/utils/request.js';
import { normalize, schemaToModel } from '@/utils/schema.js';
import AppForm from '@/components/form/index.js';
import { ElMessageBox } from 'element-plus';
import Chart from '@/components/chart/index.js';
import html from 'utils';
import { ref, onMounted, nextTick } from 'vue';
import { useI18n } from 'vue-i18n';

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
        <el-button @click="reset(formRef)" class="mb-5 ml-3">
          {{$t('发起事件')}}
        </el-button>
      </app-form>
    </el-row>
    <el-tabs type="border-card">
      <el-tab-pane label="OEE">
        <el-row
          style="flex-wrap: wrap; align-items: center; justify-content: space-between;"
        >
          <div style="width:50%;max flex:1;padding:1rem;">
            <el-card>
              <chart
                v-if="oeeAssetOption"
                :option="oeeAssetOption"
                height="250px"
              />
            </el-card>
          </div>
          <div style="width:50%;max flex:1;padding:1rem;">
            <el-card>
              <chart
                v-if="oeeComponentsOption"
                :option="oeeComponentsOption"
                height="250px"
              />
            </el-card>
          </div>
          <div style="width:100%;max flex:1;padding:1rem;">
            <el-card>
              <chart
                v-if="oeeTrendOption"
                :option="oeeTrendOption"
                height="250px"
              />
            </el-card>
          </div>
        </el-row>
      </el-tab-pane>
      <el-tab-pane label="可用性">可用性</el-tab-pane>
      <el-tab-pane label="性能">性能</el-tab-pane>
      <el-tab-pane label="质量">质量</el-tab-pane>
    </el-tabs>
  `,
  setup() {
    const { t } = useI18n();
    const schema = ref(null);
    const model = ref(null);
    const formRef = ref(null);
    const loading = ref(false);
    const oeeAssetOption = ref(null);
    const oeeComponentsOption = ref(null);
    const oeeTrendOption = ref(null);
    const validate = async () => {
      return formRef.value.validate();
    };
    const load = async () => {
      loading.value = true;
      try {
        const valid = await validate();
        if (valid) {
          const url = 'oee-dashboard/index';
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
      oeeAssetOption.value = data.asset;
      oeeAssetOption.value.series[0].itemStyle = {
        color({ data }) {
          return data.value > 0.6 ? '#00ff00' : '#ff6600';
        },
      };
      oeeComponentsOption.value = data.components;
      oeeTrendOption.value = data.trend;
      console.log(data);
    };
    onMounted(async () => {
      const result = await request('GET', 'oee-dashboard/schema');
      schema.value = normalize(result.data.data);
      model.value = result.data.data?.model ?? schemaToModel(schema.value);
      nextTick(async () => {
        await submit();
      });
    });
    return {
      formRef,
      load,
      reset,
      schema,
      model,
      loading,
      submit,
      success,
      oeeAssetOption,
      oeeComponentsOption,
      oeeTrendOption,
    };
  },
};
