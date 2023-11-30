import { jwtDecode } from 'jwt-decode';
import { defineStore } from 'pinia';

import useRouter from '@/router/index.js';
import { log } from '@/utils/index.js';
import { getUrl } from '@/utils/request.js';

import useUserStore from './user.js';

export const REFRESH_TOKEN_KEY = 'refresh_token';

export default defineStore('token', {
  state: () => ({
    accessToken: null,
    refreshToken: localStorage.getItem(REFRESH_TOKEN_KEY),
  }),
  actions: {
    async refresh() {
      log('refresh token');
      if (this.refreshToken) {
        let valid = false;
        const exp = new Date(jwtDecode(this.refreshToken).exp * 1000);
        if (exp > new Date()) {
          const response = await fetch(getUrl('token/refresh'), {
            method: 'POST',
            body: JSON.stringify(this.refreshToken),
          });
          if (response.status === 200) {
            const result = await response.json();
            this.setToken(result.access_token, result.refresh_token);
            valid = true;
          }
        }
        if (!valid) {
          this.removeToken();
        }
      }
    },
    async isLogin() {
      if (this.accessToken) {
        const exp = new Date(jwtDecode(this.accessToken).exp * 1000);
        if (exp > new Date()) {
          return true;
        }
        await this.refresh();
        if (this.accessToken) {
          return true;
        }
      }
      return false;
    },
    setToken(accessToken, refreshToken) {
      if (accessToken && refreshToken) {
        this.accessToken = accessToken;
        this.refreshToken = refreshToken;
        localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);
      }
    },
    removeToken() {
      this.accessToken = null;
      this.refreshToken = null;
      localStorage.removeItem(REFRESH_TOKEN_KEY);
    },
    async logout() {
      this.removeToken();
      const userStore = useUserStore();
      userStore.$reset();
      const router = await useRouter();
      router.push({ path: '/redirect', query: { redirect: router.currentRoute.value.fullPath } });
    },
  },
});
