import { useTabsStore } from '@/store/index.js';
import html from 'utils';

export default {
  template: html`<router-view v-if="!tabsStore.isRefreshing" />`,
  setup() {
    const tabsStore = useTabsStore();
    return {
      tabsStore,
    };
  },
};
