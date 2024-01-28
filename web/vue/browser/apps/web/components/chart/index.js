import { nextTick, ref } from 'vue';
import VChart from 'vue-echarts';

const template = `<v-chart
  v-if="renderChart"
  :option="options"
  :autoresize="autoresize"
  :style="{ width, height }"
/>`;

export default {
  template,
  components: { VChart },
  props: {
    options: {
      default: {},
    },
    autoresize: {
      default: true,
    },
    width: {
      default: '100%',
    },
    height: {
      default: '100%',
    },
  },
  setup() {
    const renderChart = ref(false);
    nextTick(() => {
      renderChart.value = true;
    });
    return {
      renderChart,
    };
  },
};
