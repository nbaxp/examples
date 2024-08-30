import html from 'utils';
import VueMd from '@/components/markdown/index.js';

const view = (name) => {
  return {
    components: { VueMd },
    template: html`
      <vue-md :name="'${name}'" />
    `,
    setup() {
      return {
        name,
      };
    },
  };
};

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
      {
        path: '/docs',
        redirect: '/docs/home',
        meta: {
          title: '文档',
        },
        children: [
          {
            path: 'home',
            component: () => view('home'),
            meta: {
              title: '智能工厂',
            },
          },
          {
            path: 'wms',
            component: () => view('wms'),
            meta: {
              title: 'WMS',
            },
          },
          {
            path: 'js',
            meta: {
              title: 'JavaScript',
            },
            children: [
              {
                path: 'https://lodash.com/',
                meta: {
                  title: 'Lodash',
                },
              },
              {
                path: 'https://echarts.apache.org/zh/index.html',
                meta: {
                  title: 'ECharts',
                },
              },
              {
                path: 'https://cn.vuejs.org/',
                meta: {
                  title: 'Vue',
                },
              },
              {
                path: 'https://router.vuejs.org/zh/',
                meta: {
                  title: 'Vue Router',
                },
              },
              {
                path: 'https://pinia.vuejs.org/zh/',
                meta: {
                  title: 'Pinia',
                },
              },
              {
                path: 'https://vueuse.org/',
                meta: {
                  title: 'VueUse',
                },
              },
              {
                path: 'https://vueuse.org/',
                meta: {
                  title: 'VueUse',
                },
              },
              {
                path: 'https://vue-i18n.intlify.dev/',
                meta: {
                  title: 'Vue I18n',
                },
              },
              {
                path: 'https://vue-echarts.dev/',
                meta: {
                  title: 'Vue ECharts',
                },
              },
              {
                path: 'https://element-plus.org/zh-CN/',
                meta: {
                  title: 'Element Plus',
                },
              },
            ],
          },
          {
            path: 'css',
            meta: {
              title: 'CSS',
            },
            children: [
              {
                path: 'https://tailwindcss.com/',
                meta: {
                  title: 'Tailwind CSS',
                },
              },
            ],
          },
        ],
      },
    ],
  },
];
