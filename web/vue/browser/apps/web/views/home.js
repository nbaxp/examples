import html from 'utils';
import Md from '@/components/markdown/index.js';
import { ref, onMounted, watch } from 'vue';
import { useUserStore } from '@/store/index.js';
import Editor from '@/components/editor/index.js';
import Chart from '@/components/chart/index.js';

export default {
  components: { Md, Editor, Chart },
  template: html`
    <el-row><md name="home" /></el-row>
    <el-row><editor v-model="model" upload-url="file/upload" /> </el-row>
    <el-row><chart :options="options" width="400px" height="200px" /></el-row>
    <h2>CKEditor</h2>
  `,
  setup() {
    const userStore = useUserStore();
    onMounted(async () => {
      await userStore.getUserInfo();
    });

    const model = ref('');
    watch(model, () => {
      console.log(model.value);
    });

    const options = {
      title: {
        text: '基础折线图',
      },
      xAxis: {
        type: 'category',
        data: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
      },
      yAxis: {
        type: 'value',
      },
      series: [
        {
          data: [150, 230, 224, 218, 135, 147, 260],
          type: 'line',
        },
      ],
    };

    return {
      model,
      options,
    };
  },
};
