import { createRouter, createWebHashHistory } from 'vue-router';
import { getMenuInfo } from '@/api/index.js';
import { beforeEach, afterEach } from './guard.js';
import { useTokenStore, useUserStore } from '@/store/index.js';
import { log } from 'utils';

let router = null;

const routes = [
  {
    name: 'layout',
    path: '/',
    redirect: '/home',
    component: () => import('../layout/index.js'),
  },
  {
    path: '/login',
    component: () => import('../views/login.js'),
    meta: {
      title: '登录',
      hideInMenu: true,
    },
  },
  {
    path: '/403',
    component: () => import('../views/403.js'),
    meta: {
      title: '权限不足',
      hideInMenu: true,
    },
  },
  {
    path: '/redirect',
    component: () => import('../views/redirect.js'),
    meta: {
      title: '跳转',
      hideInMenu: true,
    },
  },
  {
    path: '/:pathMatch(.*)*',
    component: () => import('../views/404.js'),
    meta: {
      title: '无法找到',
      hideInMenu: true,
    },
  },
];

export default async function () {
  if (router === null) {
    log('init router');
    const serverRoutes = await getMenuInfo();
    routes.find((o) => o.path === '/').children = serverRoutes;
    router = createRouter({
      history: createWebHashHistory(),
      routes,
    });
    router.beforeEach(beforeEach);
    router.afterEach(afterEach);
    const tokenStore = useTokenStore();
    await tokenStore.refresh();
    const userStore = useUserStore();
    await userStore.getUserInfo();
  }
  return router;
}
