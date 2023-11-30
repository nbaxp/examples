import { defineStore } from 'pinia';

import settings from '@/config/settings.js';
import { log } from '@/utils/index.js';
import { getUrl } from '@/utils/request.js';

export default defineStore('app', {
  state: () => ({
    ...settings,
  }),
  actions: {
    async getSiteInfo() {
      log('fetch site info');
      const response = await fetch(getUrl('locale'), { method: 'POST' });
      const result = response.json();
      return result;
    },
  },
});
