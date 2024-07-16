import router from '@/router/index.js';
import { getUrl } from '@/utils/request.js';
import { defineStore } from 'pinia';

import { listToTree, traverseTree } from 'utils';
import settings from '../config/settings.js';

const routes = [
  {
    path: '/',
    redirect: 'home',
    component: () => import('../views/layout/portal-layout.js'),
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
      {
        path: 'page1',
        component: () => import('../views/test.js'),
        meta: {
          title: '仓库管理',
          icon: 'folder',
        },
      },
      {
        path: 'page2',
        component: () => import('../views/test.js'),
        meta: {
          title: '生产工单',
          icon: 'folder',
        },
      },
      {
        path: 'page3',
        component: () => import('../views/test.js'),
        meta: {
          title: '设备管理于巡检',
          icon: 'folder',
        },
      },
      {
        path: 'page4',
        component: () => import('../views/test.js'),
        meta: {
          title: '质量管理',
          icon: 'folder',
        },
      },
      {
        path: 'about',
        component: () => import('../views/about.js'),
        meta: {
          title: '关于',
          icon: 'folder',
        },
      },
    ],
  },
  {
    path: '/wms',
    component: () => import('../views/layout/admin-layout.js'),
    meta: {
      title: 'WMS',
      icon: 'folder',
    },
    children: [
      {
        path: '',
        component: () => import('../views/wms/home.js'),
        meta: {
          title: 'WMS home',
          icon: 'file',
        },
      },
      {
        path: 'page1',
        component: () => import('../views/wms/page.js'),
        meta: {
          title: 'WMS page',
          icon: 'file',
          noCache: true,
        },
      },
    ],
  },
  {
    path: '/mes',
    component: () => import('../views/layout/admin-layout.js'),
    meta: {
      title: 'MES',
      icon: 'folder',
    },
    children: [
      {
        path: '',
        component: () => import('../views/mes/home.js'),
        meta: {
          title: 'MES home',
          icon: 'file',
        },
      },
      {
        path: 'page1',
        component: () => import('../views/mes/page.js'),
        meta: {
          title: 'MES page',
          icon: 'file',
          noCache: true,
        },
      },
    ],
  },
];

const getRoutes = async () => {
  // let routes = null;
  try {
    const response = await fetch(getUrl('menu'), { method: 'POST' });
    if (response.ok) {
      const result = await response.json();
      const list = result.data.map((o) => {
        const route = {
          id: o.id,
          parentId: o.parentId,
          path: o.routePath,
          meta: {
            type: o.type,
            buttonType: o.buttonType,
            title: o.name,
            icon: o.icon,
            classList: o.classList,
            order: o.order,
            noCache: o.noCache,
            permission: o.number,
            authorize: o.authorize,
            method: o.method,
            url: o.url,
            command: o.command,
            hidden: o.hidden,
            schema: o.schema,
            component: o.component,
          },
        };
        if (o.redirect) {
          route.redirect = o.redirect;
        }
        if (o.component) {
          const path = `../views/${o.component}.js`;
          route.component = () => import(path);
        }
        return route;
      });
      const tree = listToTree(list);
      traverseTree(tree, (o) => {
        if (o.meta.type === 'menu' && o.children?.length) {
          o.meta.buttons = o.children;
          o.children = undefined;
        }
      });
      console.log(tree);
      return tree;
    }
  } catch (e) {
    console.log(e);
  }
  // return routes;
  return [];
};

export default defineStore('app', {
  state: () => ({
    settings: Object.assign({ ...settings }, JSON.parse(localStorage.getItem('settings'))),
    menus: null,
  }),
  actions: {
    async refreshMenu() {
      this.menus = await getRoutes();
      //
      const populateFullPath = (list, parent = null) => {
        if (!list) return;
        for (const item of list) {
          item.meta ??= {};
          if (!parent) {
            if (!item.path.startsWith('/')) {
              item.path = `/${item.path}`;
              if (!item.component) {
                item.component = () => import('../views/layout/admin-layout.js');
              }
            }
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
          item.name = `route${item.meta.fullPath.replaceAll('/', '_')}`;
          console.log(item.meta.fullPath);
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
