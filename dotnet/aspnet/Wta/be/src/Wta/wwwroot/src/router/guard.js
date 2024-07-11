import { useAppStore, useTabsStore, useTokenStore, useUserStore } from '@/store/index.js';
import { useTitle } from '@vueuse/core';
import NProgress from 'nprogress/nprogress.vite-esm.js';

NProgress.configure({ showSpinner: false });

const beforeEach = async (to, from, next) => {
  console.debug(`before route: ${from.fullPath}-->${to.fullPath}`);
  NProgress.start();
  const tokenStore = useTokenStore();
  const appStore = useAppStore();
  const isLogin = await tokenStore.isLogin();
  if (isLogin) {
    if (!appStore.menus) {
      await appStore.refreshMenu();
      next({ path: to.fullPath });
    } else {
      next();
    }
  } else {
    if (to.path !== '/login') {
      next({ path: '/login', query: { redirect: to.fullPath } });
    } else {
      next();
    }
  }
};

const afterEach = (to, from) => {
  console.debug(`after route: ${to.name}: ${from.fullPath}-->${to.fullPath}`);
  try {
    const appStore = useAppStore();
    if (appStore.settings.useTabs && !to.meta?.hideInMenu) {
      const tabsStore = useTabsStore();
      tabsStore.addRoute(to);
    }
    if (to.meta?.title) {
      useTitle().value = to.meta.title;
    }
  } finally {
    NProgress.done();
  }
};

export default function createRouteGuard(router) {
  router.beforeEach(beforeEach);
  router.afterEach(afterEach);
}
