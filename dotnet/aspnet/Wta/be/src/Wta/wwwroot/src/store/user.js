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
    logout() {
      this.$reset();
    },
    async getUserInfo() {
      log('fetch user info');
      const tokenStore = useTokenStore();
      if (tokenStore.accessToken) {
        const url = 'user-info/index';
        const result = await request('GET', url);
        if (!result.error) {
          const state = result.data.data;
          this.$patch(state);
        }
      }
    },
    hasPermission(meta) {
      if (meta?.hidden) {
        return false;
      }
      if (!meta?.authType) {
        return true;
      } else {
        if (meta?.authType === 'anonymous') {
          return true;
        } else if (meta?.authType === 'roles') {
          const userStore = useUserStore();
          const roles = meta?.roles.split(',');
          if (!userStore.roles.some((o) => roles.includes(o))) {
            return false;
          }
        } else if (meta?.authType === 'permission') {
          const userStore = useUserStore();
          const permission = meta?.number;
          if (!userStore.permissions.some((o) => o === permission)) {
            return false;
          }
        }
      }
      return true;
    },
  },
});
