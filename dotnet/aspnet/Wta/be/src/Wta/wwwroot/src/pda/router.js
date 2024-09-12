import { createRouter, createWebHashHistory } from 'vue-router';

const routes = [
  {
    path: '/',
    redirect: '/home',
    component: () => import('./layouts/index.js'),
    children: [
      {
        path: 'home',
        component: () => import('./views/home.js'),
      },
      {
        path: 'scan',
        component: () => import('./views/scan.js'),
      },
    ],
  },
];

const router = createRouter({
  history: createWebHashHistory(),
  routes,
});

export default router;
