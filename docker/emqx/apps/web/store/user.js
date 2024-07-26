import { defineStore } from 'pinia';
import { useTokenStore } from '@/store/index.js';
import request from '@/utils/request.js';
import { log } from 'utils';

export const REFRESH_TOKEN_KEY = 'refresh_token';

export default defineStore('user', {
  state: () => ({
    userName: null,
    avatar: null,
    permissions: [],
  }),
  actions: {
    async getUserInfo() {
      log('fetch user info');
      const tokenStore = useTokenStore();
      if (tokenStore.accessToken) {
        const url = 'user/info';
        const result = await request('POST', url);
        if (!result.errors) {
          this.$patch(result.data);
        }
      }
    },
    hasPermission(permission) {
      if (permission === '*') {
        return true;
      }
      return this.permissions?.filter((o) => o === permission).length > 0;
    },
  },
});
