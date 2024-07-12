import router from '@/router/index.js';
import { defineStore } from 'pinia';

import settings from '../config/settings.js';
import redirect from '../views/redirect.js';

const routes = [
  {
    path: '/',
    redirect: '/home',
    component: () => import('../views/components/layout/portal-layout.js'),
    meta: {
      title: '门户',
      icon: 'folder',
    },
    children: [
      {
        path: 'home',
        component: () => import('../views/home.js'),
        meta: {
          title: '首页',
          icon: 'file',
        },
      },
    ],
  },
  {
    path: '/wms',
    component: () => import('../views/test.js'),
    meta: {
      title: '仓库管理',
      icon: 'folder',
    },
  },
  {
    path: '/mes',
    component: () => import('../views/test.js'),
    meta: {
      title: '生产工单',
      icon: 'folder',
    },
  },
  {
    path: '/device',
    component: () => import('../views/test.js'),
    meta: {
      title: '设备管理于巡检',
      icon: 'folder',
    },
  },
  {
    path: '/test',
    component: () => import('../views/components/layout/admin-layout.js'),
    meta: {
      title: 'Test',
      icon: 'folder',
    },
    children: [
      {
        path: '',
        component: () => import('../views/test.js'),
        meta: {
          title: 'module1 home',
          icon: 'file',
        },
      },
      {
        path: 'page1',
        component: () => import('../views/test.js'),
        meta: {
          title: 'module1 page',
          icon: 'file',
        },
      },
    ],
  },
  {
    path: '/about',
    component: () => import('../views/about.js'),
    meta: {
      title: '关于',
      icon: 'folder',
    },
  },
];

const getRoutes = async () => {
  return routes;
};

export default defineStore('app', {
  state: () => ({
    settings: { ...settings },
    menus: null,
  }),
  actions: {
    async refreshMenu() {
      this.menus = await getRoutes();
      //
      const populateFullPath = (list, parent = null) => {
        if (!list) return;
        for (const item of list) {
          Object.assign(item, {
            meta: item.meta ?? {},
          });
          if (!parent) {
            item.meta.fullPath = item.path;
          } else {
            if (!item.path) {
              item.meta.fullPath = parent?.meta?.fullPath;
            } else {
              if (parent.meta.fullPath.endsWith('/')) {
                item.meta.fullPath = `${parent?.meta?.fullPath}${item.path}`;
              } else {
                item.meta.fullPath = `${parent?.meta?.fullPath}/${item.path}`;
              }
            }
          }
          item.name = item.meta.fullPath.replaceAll('/', '_');
          if (item.children?.length) {
            populateFullPath(item.children, item); // 递归处理子节点
          }
        }
      };
      populateFullPath(this.menus);
      //
      const key = 'root';
      const root = router.getRoutes().find((o) => o.name === key);
      if (root) {
        router.removeRoute(root);
      }
      router.addRoute('/', { name: key, path: '/', children: this.menus });
    },
  },
});
