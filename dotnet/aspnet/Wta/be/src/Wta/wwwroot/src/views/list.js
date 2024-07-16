import html from 'utils';
import { onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';

import AppList from '@/views/components/list/index.js';

export default {
  components: { AppList },
  template: html`<app-list v-if="config" :config="config" @command="onCommand" />`,
  setup() {
    const route = useRoute();
    const config = ref(null);
    const onCommand = async (item, rows) => {
      console.log(item.path, item, rows);
    };
    onMounted(async () => {
      const useConfig = (await import(`../models${route.meta.fullPath}.js`)).default;
      config.value = typeof useConfig === 'function' ? useConfig() : useConfig;
      config.value.buttons = route.meta.buttons ?? [];
    });
    return { config, onCommand };
  },
};
