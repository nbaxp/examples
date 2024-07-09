import { defineStore } from 'pinia';

import settings from '@/config/settings.js';

export default defineStore('tabs', {
  state: () => ({
    ...settings,
  }),
});
