import request from '@/utils/request.js';
import { normalize, schemaToModel } from '@/utils/schema.js';
import AppForm from '@/components/form/index.js';
import { ElMessageBox } from 'element-plus';
import Chart from '@/components/chart/index.js';
import html from 'utils';
import { ref, onMounted, nextTick } from 'vue';
import { useI18n } from 'vue-i18n';
import options from '@/utils/chart.js';
import { merge } from 'lodash';

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
          {{$t('通知')}}
        </el-button>
      </app-form>
    </el-row>
    <el-tabs type="border-card" v-model="currentTab" @tab-change="tabChange">
      <el-tab-pane name="OEE" label="OEE">
        <el-row
          style="flex-wrap: wrap; align-items: center; justify-content: space-between;"
        >
          <div style="width:50%;max flex:1;padding:1rem;">
            <el-card>
              <chart
                ref="chart11Ref"
                v-if="chart11"
                :option="chart11"
                height="250px"
              />
            </el-card>
          </div>
          <div style="width:50%;max flex:1;padding:1rem;">
            <el-card>
              <chart
                ref="chart12Ref"
                v-if="chart12"
                :option="chart12"
                height="250px"
                @click="o=>tabChange(o.name)"
              />
            </el-card>
          </div>
          <div style="width:100%;max flex:1;padding:1rem;">
            <el-card>
              <chart
                ref="chart13Ref"
                v-if="chart13"
                :option="chart13"
                height="250px"
              />
            </el-card>
          </div>
        </el-row>
      </el-tab-pane>
      <el-tab-pane name="可用性" label="可用性">
        <el-row
          style="flex-wrap: wrap; align-items: center; justify-content: space-between;"
        >
          <div style="width:50%;max flex:1;padding:1rem;">
            <el-card>
              <chart v-if="chart21" :option="chart21" height="250px" />
            </el-card>
          </div>
          <div style="width:50%;max flex:1;padding:1rem;">
            <el-card>
              <chart v-if="chart22" :option="chart22" height="250px" />
            </el-card>
          </div>
          <div style="width:100%;max flex:1;padding:1rem;">
            <el-card>
              <chart v-if="chart23" :option="chart23" height="250px" />
            </el-card>
          </div>
        </el-row>
      </el-tab-pane>
      <el-tab-pane name="性能" label="性能">性能</el-tab-pane>
      <el-tab-pane name="质量" label="质量">质量</el-tab-pane>
    </el-tabs>
  `,
  setup() {
    const { t } = useI18n();
    const schema = ref(null);
    const model = ref(null);
    const formRef = ref(null);
    const loading = ref(false);
    const currentTab = ref(null);
    const chart11 = ref(options());
    const chart12 = ref(options());
    const chart13 = ref(options());
    const chart11Ref = ref(null);
    const chart12Ref = ref(null);
    const chart13Ref = ref(null);
    const chart21 = ref(null);
    const chart22 = ref(null);
    const chart23 = ref(null);
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
      console.log(data);
      if (currentTab.value === 'OEE') {
        merge(chart11.value, data.chart11);
        merge(chart12.value, data.chart12);
        merge(chart13.value, data.chart13);
      } else if (currentTab.value === '可用性') {
        chart21.value = data.chart1;
        chart22.value = data.chart2;
        chart23.value = data.chart3;
      }
    };
    const tabChange = async (name) => {
      currentTab.value = name;
      if (name === 'OEE') {
        schema.value.meta.url = 'oee-dashboard/index';
        await submit();
      } else if (name === '可用性') {
        schema.value.meta.url = 'oee-dashboard/availability';
        await submit();
      }
    };
    const tooltipFormatter = (data) => {
      return `${data[0].axisValue}:${data[0].value}`;
    };
    onMounted(async () => {
      const result = await request('GET', 'oee-dashboard/schema');
      schema.value = normalize(result.data.data);
      model.value = result.data.data?.model ?? schemaToModel(schema.value);
      tabChange('OEE');
      chart11.value.series[0].itemStyle = {
        color({ data }) {
          return data.value <= 0.6 ? '#00ff00' : '#ff6600';
        },
      };
      chart11.value.tooltip.formatter = tooltipFormatter;
      //chart12.value.tooltip.formatter = tooltipFormatter;
      nextTick(async () => {
        await submit();
      });
      // const charts = [chart11Ref];
      // charts.forEach((o) => {
      //   o.value.formatter = (data) => {
      //     charts.forEach((p) => {
      //       if (p !== o) {
      //         const { dataIndex } = data[0];
      //         p.value.dispatchAction({
      //           type: 'showTip',
      //           dataIndex,
      //           seriesIndex: 0,
      //         });
      //       }
      //       return data[0].value;
      //     });
      //   };
      // });
    });
    return {
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
      chart11,
      chart12,
      chart13,
      chart11Ref,
      chart12Ref,
      chart13Ref,
      chart21,
      chart22,
      chart23,
    };
  },
};
