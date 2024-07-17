import { defineStore } from 'pinia';

import request from '@/utils/request.js';
import { log } from 'utils';

import { useTokenStore, useUserStore } from '@/store/index.js';

export default defineStore('user', {
  state: () => ({
    userName: null,
    name: null,
    avatar: null,
    roles: [],
    permissions: [],
  }),
  actions: {
    async getUserInfo() {
      log('fetch user info');
      const tokenStore = useTokenStore();
      if (tokenStore.accessToken) {
        const url = 'user/info';
        const result = await request('POST', url);
        if (result.ok) {
          this.$patch(result.data);
        }
      }
    },
    hasPermission(permission) {
      const userStore = useUserStore();
      const tokenStore = useTokenStore();
      if (/\[\]/.test(permission)) {
        const roles = JSON.parse(permission);
        return userStore.roles.some((o) => roles.includes(o));
      }
      if (permission === 'Anonymous') {
        return true;
      }
      if (permission === 'Authenticated') {
        return tokenStore.isLogin();
      }
      return !permission || this.permissions?.some((o) => o === permission);
    },
  },
});
