import redirect from '@/views/redirect.js';

export default [
  {
    name: 'default',
    path: '/',
    redirect: '/home',
    component: () => import('@/layouts/blank.js'),
    children: [
      {
        path: '/login',
        component: () => import('@/views/login.js'),
        meta: {
          title: '登录',
        },
      },
      {
        path: '/forgot-password',
        component: () => import('@/views/forgot-password.js'),
        meta: {
          title: '忘记密码',
        },
      },
      {
        path: '/403',
        component: () => import('@/views/403.js'),
        meta: {
          title: '权限不足',
        },
      },
      {
        path: '/redirect',
        component: () => import('@/views/redirect.js'),
        meta: {
          title: '跳转',
        },
      },
      {
        path: '/:pathMatch(.*)*',
        component: () => import('@/views/404.js'),
        meta: {
          title: '无法找到',
        },
      },
    ],
  },
];
