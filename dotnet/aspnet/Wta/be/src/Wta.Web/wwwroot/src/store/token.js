import { getUrl } from '@/utils/request.js';
import jwtDecode from 'jwt-decode';
import { defineStore } from 'pinia';

const REFRESH_TOKEN_KEY = 'refresh_token';

export default defineStore('token', {
  state: () => ({
    accessToken: null,
    refreshToken: localStorage.getItem(REFRESH_TOKEN_KEY),
    exp: null,
    name: null,
  }),
  actions: {
    update(accessToken, refreshToken) {
      this.accessToken = accessToken;
      this.refreshToken = refreshToken;
      localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);
      const data = jwtDecode(this.accessToken);
      this.exp = new Date(data.exp * 1000);
      this.name = data.name;
    },
    clear() {
      this.accessToken = null;
      this.refreshToken = null;
      localStorage.removeItem(REFRESH_TOKEN_KEY);
      this.exp = null;
      this.name = null;
    },
    async isLogin() {
      if (this.accessToken) {
        const exp = new Date(jwtDecode(this.accessToken).exp * 1000);
        if (exp > new Date()) {
          return true;
        }
        this.accessToken = null;
      }
      await this.refresh();
      return !!this.accessToken;
    },
    async refresh() {
      if (this.refreshToken) {
        const exp = new Date(jwtDecode(this.refreshToken).exp * 1000);
        if (exp > new Date()) {
          const response = await fetch(getUrl('token/refresh'), {
            method: 'POST',
            body: JSON.stringify(this.refreshToken),
            headers: {
              'Content-Type': 'application/json',
            },
          });
          if (response.status === 200) {
            const { data } = await response.json();
            this.update(data.access_token, data.refresh_token);
            return;
          }
        }
        this.clear();
      }
    },
  },
});
