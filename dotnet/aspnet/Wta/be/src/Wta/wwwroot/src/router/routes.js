import docs from './docs.js';

export default [
  {
    name: 'blank',
    path: '/',
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
        path: '/oauth/login',
        component: () => import('@/views/oauth-login.js'),
        meta: {
          title: '三方登录',
        },
      },
      {
        path: '/logout',
        component: () => import('@/views/login.js'),
        meta: {
          title: '登录',
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
        path: '/404',
        component: () => import('@/views/404.js'),
        meta: {
          title: '未找到',
        },
      },
      {
        path: '/register',
        component: () => import('@/views/register.js'),
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
  {
    name: 'portal',
    path: '',
    redirect: '/home',
    component: () => import('@/layouts/portal.js'),
    children: [
      {
        path: '/home',
        component: () => import('@/views/home.js'),
        meta: {
          title: '首页',
        },
      },
      docs,
    ],
  },
  {
    path: '/pda',
    component: () => import('@/views/pda.js'),
    meta: {
      title: 'pda',
    },
  },
];
