import { createRouter, createWebHashHistory } from 'vue-router';

import { useAppStore } from '@/store/index.js';
import { listToTree } from '@/utils/index.js';

import { afterEach, beforeEach } from './guard.js';

const layouts = import.meta.glob('../layout/**/*.vue');
const views = import.meta.glob('../views/**/*.vue');
const layout = (name) => {
  return layouts[`../layout/${name}.vue`];
};
const view = (name) => {
  return views[`../views/${name}.vue`];
};

const routes = [
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

const router = createRouter({
  history: createWebHashHistory(),
  routes,
});

const convert = (list) => {
  list.forEach((o) => {
    o.meta ??= {};
    o.meta.type = o.type;
    delete o.type;
    if (o.meta.type === 'page') {
      o.meta.buttons = o.children;
      delete o.children;
    }
    if (o.component) {
      const file = o.component;
      if (o.redirect) {
        o.component = layout(file);
      } else if (o.isMarkdown) {
        o.meta.file = file;
        o.component = view('md');
      } else {
        o.component = view(file);
      }
    }
    if (o.children?.length) {
      convert(o.children);
    }
  });
};

async function refreshRouter() {
  const appStore = useAppStore();
  await appStore.getMenus();
  const tree = listToTree(appStore.menus);
  convert(tree);
  tree.forEach((o) => router.addRoute(o.path, o));
}

router.beforeEach(beforeEach);
router.afterEach(afterEach);

export default router;

export { refreshRouter };
