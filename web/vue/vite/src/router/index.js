import { createRouter, createWebHashHistory } from 'vue-router';

import { useTokenStore, useUserStore } from '@/store/index.js';
import { log } from '@/utils/index.js';
import { getUrl } from '@/utils/request.js';

import { afterEach, beforeEach } from './guard.js';

const layouts = import.meta.glob('../layout/**/*.vue');
const views = import.meta.glob('../views/**/*.vue');
const layout = (name) => {
  return layouts[`../layout/${name}.vue`];
};
const view = (name) => {
  return views[`../views/${name}.vue`];
};

let router = null;

const routes = [
  {
    name: 'layout',
    path: '/',
    redirect: '/home',
    component: layout('index'),
  },
  {
    path: '/login',
    component: view('login'),
    meta: {
      title: '登录',
      hideInMenu: true,
    },
  },
  {
    path: '/403',
    component: view('403'),
    meta: {
      title: '权限不足',
      hideInMenu: true,
    },
  },
  {
    path: '/redirect',
    component: view('redirect'),
    meta: {
      title: '跳转',
      hideInMenu: true,
    },
  },
  {
    path: '/:pathMatch(.*)*',
    component: view('404'),
    meta: {
      title: '无法找到',
      hideInMenu: true,
    },
  },
];

const convert = (list) => {
  list.forEach((o) => {
    if (o.component) {
      if (o.isMarkdown) {
        o.meta.file = o.component;
        o.component = view('md');
      } else {
        const file = o.component;
        o.component = view(file);
      }
    }
    if (o.children?.length) {
      convert(o.children);
    }
  });
};

async function getMenuInfo() {
  log('fetch menus');
  const response = await fetch(getUrl('menu'), { method: 'POST' });
  const result = await response.json();
  // 转换格式开始
  convert(result);
  // 转换格式结束
  return result;
}

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
