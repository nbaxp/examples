import { defineStore } from 'pinia';

import settings from '@/config/settings.js';

export default defineStore('user', {
  state: () => ({
    ...settings,
  }),
});
