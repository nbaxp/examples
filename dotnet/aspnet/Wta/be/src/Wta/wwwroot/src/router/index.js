import { createRouter, createWebHashHistory } from 'vue-router';
import createRouteGuard from './guard.js';
import routes from './routes.js';

const router = createRouter({
  history: createWebHashHistory(),
  routes,
});

createRouteGuard(router);

export default router;
