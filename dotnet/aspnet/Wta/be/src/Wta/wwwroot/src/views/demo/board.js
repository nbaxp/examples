import { DATETIME_DISPLAY_FORMAT } from '@/constants/index.js';
import { dayjs } from 'element-plus';
import html from 'utils';
import { onMounted, ref } from 'vue';

export default {
  template: html`
    {{dayjs(vm.now).format(DATETIME_DISPLAY_FORMAT)}}
  `,
  styles: html`
    <style>
      .el-header,
      .el-footer {
        display: none !important;
      }
    </style>
  `,
  setup() {
    const hasEventSource = !!window.EventSource;
    const vm = ref({});
    const update = (data) => {
      vm.value = data;
    };
    const connect = () => {
      const es = new EventSource('/api/board/index');
      es.onmessage = (event) => {
        update(JSON.parse(event.data));
      };
      es.onerror = (e) => {
        es.close();
        console.log(e);
        setTimeout(connect, 5 * 1000);
      };
    };

    onMounted(() => {
      if (hasEventSource) {
        try {
          connect();
        } catch (e) {
          console.log(e);
          setTimeout(connect, 5 * 1000);
        }
      }
    });

    return {
      vm,
      dayjs,
      DATETIME_DISPLAY_FORMAT,
    };
  },
};
