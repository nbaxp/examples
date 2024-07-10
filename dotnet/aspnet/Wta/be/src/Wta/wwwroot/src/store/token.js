import jwtDecode from 'jwt-decode';
import { defineStore } from 'pinia';
import {getUrl} from '@/utils/request.js';

const REFRESH_TOKEN_KEY = 'refresh_token';

export default defineStore('token', {
  state: () => ({
    accessToken: null,
    refreshToken: localStorage.getItem(REFRESH_TOKEN_KEY),
  }),
  actions: {
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
    setToken(accessToken,refreshToken){
      this.accessToken = accessToken;
      this.refreshToken = refreshToken;
      localStorage.setItem(REFRESH_TOKEN_KEY,refreshToken);
    },
    async refresh() {
      if (this.refreshToken) {
        const exp = new Date(jwtDecode(this.refreshToken).exp * 1000);
        const now = new Date();
        if (exp<now) {
          const response = await fetch(getUrl('token/refresh'), {
            method: 'POST',
            body: JSON.stringify(this.refreshToken),
            headers: {
              'Content-Type': 'application/json',
            },
          });
          if (response.status === 200) {
            const result = await response.json();
            this.accessToken = result.data.access_token;
            this.refreshToken = result.data.refresh_token;
            return;
          }
        }
        this.refreshToken = null;
        localStorage.removeItem(REFRESH_TOKEN_KEY);
      }
    },
  },
});
