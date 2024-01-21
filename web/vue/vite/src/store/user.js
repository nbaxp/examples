import { defineStore } from 'pinia';

import { log } from '@/utils/index.js';
import request from '@/utils/request.js';

import useTokenStore from './token.js';

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
    hasPermission(role) {
      return !role || this.roles?.some((o) => o === role);
    },
    hasPermission(permission) {
      return !permission || this.permissions?.some((o) => o === permission);
    },
  },
});
