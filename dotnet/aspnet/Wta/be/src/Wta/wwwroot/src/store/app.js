import settings from '@/config/settings.js';
import router from '@/router/index.js';
import { getUrl } from '@/utils/request.js';
import { defineStore } from 'pinia';
import { listToTree, traverseTree } from 'utils';

const getRoutes = async () => {
  try {
    const response = await fetch(getUrl('menu'), { method: 'POST' });
    if (response.ok) {
      const result = await response.json();
      const list = result.data.map((o) => {
        const { id, parentId, routePath: path, redirect, component, ...meta } = o;
        meta.title ??= o.name;
        const route = {
          id,
          parentId,
          path: path === null ? '' : path,
          redirect,
          component,
          meta,
        };
        // if (route.component) {
        //   const componentPath = `../views/${route.component}.js`;
        //   route.component = () => import(componentPath);
        // }
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
      const populateFullPath = async (list, parent = null) => {
        if (!list) return;
        for (const item of list) {
          item.meta ??= {};
          if (!parent) {
            if (!item.path.startsWith('/')) {
              item.path = `/${item.path}`;
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
          let viewPath = item.component;
          if (!viewPath && item.meta.type === 'menu') {
            const response = await fetch(`./src/views${item.meta.fullPath}.js`, { method: 'HEAD' }).catch((e) => {
              console.log(e);
            });
            if (response.ok) {
              viewPath = item.meta.fullPath.substring(1);
            } else {
              viewPath = '_list';
            }
          }
          if (viewPath) {
            item.component = () => import(`@/views/${viewPath}.js`);
          }
          if (item.children?.length) {
            await populateFullPath(item.children, item);
          }
        }
      };
      await populateFullPath(this.menus);
      //
      const key = 'root';
      if (router.getRoutes().some((o) => o.name === key)) {
        router.removeRoute(key);
      }
      router.addRoute('/', {
        name: key,
        path: '/',
        redirect: '/home',
        component: () => import('@/layouts/index.js'),
        children: this.menus,
      });
    },
  },
});
