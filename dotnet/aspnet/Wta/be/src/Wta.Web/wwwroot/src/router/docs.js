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

export default {
  path: '/docs',
  redirect: '/docs/home',
  meta: {
    title: '帮助中心',
  },
  children: [
    {
      path: 'home',
      component: () => view('home'),
      meta: {
        title: '帮助中心',
      },
    },
    {
      path: 'product',
      meta: {
        title: '产品中心',
      },
      children: [
        {
          path: 'home',
          component: () => view('product/home'),
          meta: {
            title: '首页',
          },
        },
        {
          path: 'oee',
          component: () => view('product/oee'),
          meta: {
            title: 'OEE',
          },
        },
        {
          path: 'wms',
          component: () => view('product/wms'),
          meta: {
            title: 'WMS',
          },
        },
        {
          path: 'mes',
          component: () => view('product/mes'),
          meta: {
            title: 'MES',
          },
        },
      ],
    },
    {
      path: 'developer',
      meta: {
        title: '研发中心',
      },
      children: [
        {
          path: 'home',
          component: () => view('developer/home'),
          meta: {
            title: '首页',
          },
        },
        {
          path: 'oauth2',
          component: () => view('developer/oauth2'),
          meta: {
            title: '应用集成',
          },
        },
        {
          path: 'links',
          meta: {
            title: '文档链接',
          },
          children: [
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
  ],
};
