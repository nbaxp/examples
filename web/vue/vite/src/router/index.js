import { createRouter, createWebHashHistory } from 'vue-router';
import { useAppStore } from '@/store/index.js';
import { listToTree } from '@/utils/index.js';
import { defineAsyncComponent, provide, markRaw } from 'vue';

import docs from './docs.js';
import { afterEach, beforeEach } from './guard.js';

const layouts = import.meta.glob('../layout/**/*.vue');
const views = import.meta.glob('../views/**/*.vue');

const layout = (name) => {
  return layouts[`../layout/${name}.vue`];
};

const view = (file, name = null) => {
  return markRaw({
    name: name ?? `/${file}`,
    components: {
      AppPage: defineAsyncComponent(views[`../views/${file}.vue`]),
    },
    template: `<app-page />`,
  });
};

const routes = [
  {
    path: '/register',
    component: view('register'),
    meta: {
      title: 'register',
      hideInMenu: true,
    },
  },
  {
    path: '/forgot-password',
    component: view('forgot-password'),
    meta: {
      title: 'register',
      hideInMenu: true,
    },
  },
  {
    path: '/login',
    component: view('login'),
    meta: {
      title: 'login',
      hideInMenu: true,
    },
  },
  {
    path: '/403',
    component: view('403'),
    meta: {
      title: '403',
      hideInMenu: true,
    },
  },
  {
    path: '/redirect',
    component: view('redirect'),
    meta: {
      title: 'redirect',
      hideInMenu: true,
    },
  },
  {
    path: '/:pathMatch(.*)*',
    component: view('404'),
    meta: {
      title: '404',
      hideInMenu: true,
    },
  },
  docs,
];

const router = createRouter({
  history: createWebHashHistory(),
  routes,
});

const convert = (list, parent = null) => {
  list.forEach((o) => {
    if (o.meta.type === 'menu') {
      o.meta.buttons = o.children;
      delete o.children;
    }
    o.name = (parent ? parent.name + '/' : '/') + o.path;
    const file = o.component;
    if (o.redirect) {
      o.component = layout(file ?? 'index');
    } else if (file) {
      o.component = view(file, o.name);
    }
    if (o.children?.length) {
      convert(o.children, o);
    }
  });
};

async function refreshRouter() {
  const appStore = useAppStore();
  await appStore.getMenus();
  const tree = listToTree(appStore.menus, (o) => {
    o.meta ??= {};
    o.meta.type = o.type;
    o.meta.noCache = o.noCache;
    o.meta.title = o.title;
    o.meta.icon = o.icon;
    o.meta.order = o.order;
    o.meta.buttonType = o.buttonType;
    o.meta.buttonClass = o.buttonClass;
    o.meta.apiMethod = o.apiMethod;
    o.meta.apiUrl = o.apiUrl;
    o.meta.command = o.command;
    o.meta.hidden = o.hidden;
    o.meta.schema = o.schema;
    delete o.type;
    delete o.title;
    delete o.icon;
    delete o.order;
    delete o.buttonType;
    delete o.buttonClass;
    delete o.apiMethod;
    delete o.apiUrl;
    delete o.command;
    delete o.hidden;
    delete o.schema;
  });
  convert(tree);
  const route = router.getRoutes().find((o) => o.name === 'layout');
  if (route) {
    router.removeRoute(route.name);
  }
  router.addRoute('/', {
    name: 'layout',
    path: '/',
    redirect: '/home',
    component: layout('index'),
    meta: { title: 'home', icon: 'home' },
    children: tree,
  });
}

router.beforeEach(beforeEach);
router.afterEach(afterEach);

export default router;

export { refreshRouter };
