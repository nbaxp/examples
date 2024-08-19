import { createRouter, createWebHashHistory } from 'vue-router';
import createRouteGuard from './guard.js';
import routes from './routes.js';
import docs from './docs.js';

const router = createRouter({
  history: createWebHashHistory(),
  routes: [...routes, ...docs],
});

createRouteGuard(router);

export default router;
