import { useTitle } from "@vueuse/core";
import { useTabsStore, useUserStore, useTokenStore } from "../store/index.js";
import NProgress from "../lib/nprogress/nprogress.vite-esm.js";
import { log } from "utils";

NProgress.configure({ showSpinner: false });

const beforeEach = async (to, from, next) => {
  log(`before route: ${from.fullPath}-->${to.fullPath}`);
  NProgress.start();

  if (to.path !== "/login" && to.meta?.permission) {
    //需要权限验证
    const tokenStore = useTokenStore();
    const isLogin = await tokenStore.isLogin();
    if (!isLogin) {
      //未登录
      next({ path: "/login", query: { redirect: to.fullPath } });
    } else {
      const userStore = useUserStore();
      if (userStore.hasPermission(to.meta.permission)) {
        //有权限
        next();
      } else {
        //权限不足
        next({ path: "/403", query: { redirect: to.fullPath } });
      }
    }
  } else {
    //无需权限验证
    next();
  }
};

const afterEach = (to, from) => {
  log(`after route: ${from.fullPath}-->${to.fullPath}`);
  try {
    if (to.path === "/login") {
      // logout
    }
    if (!to.meta?.hideInMenu) {
      const tabsStore = useTabsStore();
      tabsStore.addRoute(to);
    }
    if (to.meta?.title) {
      useTitle().value = `${to.meta.title}`;
    }
  } finally {
    NProgress.done();
  }
};

export { beforeEach, afterEach };
