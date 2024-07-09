export default [
  {
    path: '/login',
    component: () => import('@/views/login.js'),
    meta: {
      title: '登录',
      hideInMenu: true,
    },
  },
  {
    path: '/403',
    component: () => import('@/views/403.js'),
    meta: {
      title: '权限不足',
      hideInMenu: true,
    },
  },
  {
    path: '/redirect',
    component: () => import('@/views/redirect.js'),
    meta: {
      title: '跳转',
      hideInMenu: true,
    },
  },
  {
    path: '/:pathMatch(.*)*',
    component: () => import('@/views/404.js'),
    meta: {
      title: '无法找到',
      hideInMenu: true,
    },
  },
];
