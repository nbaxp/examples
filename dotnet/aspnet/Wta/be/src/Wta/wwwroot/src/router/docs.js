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
    path: '/docs',
    redirect: '/docs/home',
    component: () => import('@/layouts/index.js'),
    meta: {
      title: '帮助',
    },
    children: [
      {
        path: 'home',
        component: () => view('home'),
        meta: {
          title: '首页',
        },
      },
    ],
  },
];

//export default [];
