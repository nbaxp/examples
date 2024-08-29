import i18n from '@/locales/index.js';
import { useAppStore, useTabsStore, useTokenStore, useUserStore } from '@/store/index.js';
import { useTitle } from '@vueuse/core';
import NProgress from '~/lib/nprogress/nprogress.vite-esm.js';

NProgress.configure({ showSpinner: false });

const beforeEach = async (to, from, next) => {
  console.debug(`before route: ${from.fullPath}-->${to.fullPath}`);
  NProgress.start();
  const tokenStore = useTokenStore();
  const userStore = useUserStore();
  const appStore = useAppStore();
  const isLogin = await tokenStore.isLogin();
  if (isLogin) {
    if (!userStore.userName) {
      await userStore.getUserInfo();
      next({ path: to.fullPath });
    } else if (!appStore.menus) {
      await appStore.refreshMenu();
      next({ path: to.fullPath });
    } else {
      next();
    }
  } else {
    if (!to.meta.requiresAuth) {
      next();
    } else {
      next({ path: '/login', query: { redirect: to.fullPath } });
    }
  }
};

const afterEach = (to, from) => {
  console.debug(`after route: ${to.name}: ${from.fullPath}-->${to.fullPath}`);
  try {
    const appStore = useAppStore();
    if (appStore.settings.useTabs && to.matched[0].name === 'root' && !to.meta?.hideInMenu) {
      const tabsStore = useTabsStore();
      tabsStore.addRoute(to);
    }
    if (to.meta?.title) {
      const { t } = i18n.global;
      useTitle().value = t(to.meta.title);
    }
  } finally {
    NProgress.done();
  }
};

export default function createRouteGuard(router) {
  router.beforeEach(beforeEach);
  router.afterEach(afterEach);
}
