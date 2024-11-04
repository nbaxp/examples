import { createRouter, createWebHashHistory, createWebHistory } from 'vue-router';
import createRouteGuard from './guard.js';
import routes from './routes.js';
import { trimStart, trimEnd } from 'lodash';

const populateFullPath = async (list, parent = null) => {
  if (!list) return;
  for (const item of list) {
    item.meta ??= {};
    if (item.path.startsWith('http')) {
      item.meta.fullPath = item.path;
    } else {
      if (!parent) {
        item.meta.fullPath = '/' + trimStart(item.path, '/');
      } else {
        item.meta.fullPath = trimEnd(parent?.meta?.fullPath, '/') + '/' + trimStart(item.path, '/');
      }
    }
    console.log(item.meta.fullPath);
    if (item.children?.length) {
      await populateFullPath(item.children, item);
    }
  }
};

populateFullPath(routes);

const router = createRouter({
  history: createWebHashHistory(),
  routes,
});

createRouteGuard(router);

export default router;
