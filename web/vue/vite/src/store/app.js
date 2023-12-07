import { defineStore } from 'pinia';

import settings from '@/config/settings.js';
import i18n from '@/locale/index.js';
import { log } from '@/utils/index.js';
import { getUrl } from '@/utils/request.js';

export default defineStore('app', {
  state: () => ({
    settings: { ...settings },
  }),
  actions: {
    async getLocale() {
      log('fetch locale');
      const response = await fetch(getUrl('locale'), { method: 'POST' });
      const result = await response.json();
      this.locale = result;
      i18n.global.locale.value = this.locale.locale;
      i18n.global.fallbackLocale.value = this.locale.fallbackLocale;
      Object.keys(this.locale.messages).forEach((key) => i18n.global.setLocaleMessage(key, this.locale.messages[key]));
    },
    async getMenus() {
      log('fetch menus');
      const response = await fetch(getUrl('menu'), { method: 'POST' });
      const result = await response.json();
      this.menus = result;
      return result;
    },
  },
});
