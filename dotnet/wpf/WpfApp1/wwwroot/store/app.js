import { defineStore } from "pinia";
import settings from "../config/settings.js";
import { getAccessToken, getUser, isLogin } from "../api/user.js";
import { refreshRouter } from "../router/index.js";
import { getLocalizationAsync } from "../api/site.js";

const useAppStore = defineStore("app", {
  state: () => {
    const state = {
      settings: { ...settings },
      isMenuCollapse: false,
      isRefreshing: false,
      routes: [],
      cache: new Map(),
    };
    const localSettings = JSON.parse(localStorage.getItem("settings") ?? "{}");
    Object.assign(state.settings, localSettings);
    return state;
  },
  actions: {
    async init() {
      // 获取站点信息
      // const result = await get("localization", null, null, true, true);
      // this.localization = result.data;
      //
      this.token = getAccessToken();
      this.localization = await getLocalizationAsync();
      // 获取用户信息
      if (await isLogin()) {
        this.user = await getUser();
        await refreshRouter();
      }
    },
    add(route) {
      if (!this.routes.find((o) => o.fullPath === route.fullPath)) {
        this.routes.push(route);
      } else {
        const index = this.routes.findIndex((o) => o.fullPath === route.fullPath);
        this.routes[index] = route;
      }
    },
  },
});

export default useAppStore;
