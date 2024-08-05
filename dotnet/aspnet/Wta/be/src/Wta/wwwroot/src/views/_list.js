import html from 'utils';
import { onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';

import AppList from '@/components/list/index.js';
import request from '@/utils/request.js';
import { normalize } from '@/utils/schema.js';

export default {
  components: { AppList },
  template: html`<app-list v-if="schema" :schema="schema" @command="onCommand" />`,
  setup() {
    const route = useRoute();
    const schema = ref(null);
    const onCommand = async (item, rows) => {
      console.log(item.path, item, rows);
    };
    onMounted(async () => {
      const response = await import(`@/models${route.meta.fullPath}.js`).catch((e) => {
        console.log(e);
      });
      if (response) {
        const useSchema = response.default;
        schema.value = normalize(typeof useSchema === 'function' ? useSchema() : useSchema);
      } else {
        const url = route.meta.buttons.find((o) => o.meta.command === 'search').meta.url.replace(/search$/, 'schema');
        const result = await request('GET', `/${url}`);
        schema.value = normalize(result.data.data);
      }
      schema.value.meta.buttons = route.meta.buttons ?? [];
    });
    return { schema, onCommand };
  },
};
