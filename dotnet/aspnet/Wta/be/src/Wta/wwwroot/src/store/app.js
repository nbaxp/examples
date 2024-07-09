import { defineStore } from 'pinia';

import settings from '../config/settings.js';

export default defineStore('app', {
  state: () => ({
    settings: { ...settings },
    menu: null,
  }),
  actions: {
    async getMenus() {},
  },
});
