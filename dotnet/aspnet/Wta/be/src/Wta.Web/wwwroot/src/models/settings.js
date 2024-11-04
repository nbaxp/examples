import { normalize } from '@/utils/schema.js';

const rules = [
  {
    required: true,
  },
];

export default function () {
  const schema = {
    title: 'settings',
    properties: {
      isDebug: {
        title: '调试模式',
        type: 'boolean',
      },
      useMock: {
        title: '模拟请求',
        type: 'boolean',
      },
      baseURL: {
        title: '后端地址',
        rules,
      },
      serverLocale: {
        title: '服务端区域设置',
        type: 'boolean',
      },
      serverRoute: {
        title: '服务端路由模式',
        type: 'boolean',
      },
      isMenuCollapse: {
        title: '折叠菜单',
        rules,
        type: 'boolean',
      },
      size: {
        title: '控件大小',
        input: 'radio',
        default: 'default',
        options: [
          { value: 'large', label: '大' },
          { value: 'default', label: '默认' },
          { value: 'small', label: '小' },
        ],
      },
      color: {
        title: '主题色',
        input: 'color',
      },
      theme: {
        title: '主题',
        input: 'radio',
        default: 'auto',
        options: [
          { value: 'light', label: '浅色' },
          { value: 'dark', label: '深色' },
        ],
      },
      useDarkNav: {
        title: '暗色导航',
        type: 'boolean',
      },
      useTabs: {
        title: '启用 Tab 页签',
        type: 'boolean',
      },
      showBreadcrumb: {
        title: '显示站内导航',
        type: 'boolean',
      },
      showCopyright: {
        title: '显示页脚',
        type: 'boolean',
      },
      showTopMenu: {
        title: '启用主导航',
        type: 'boolean',
      },
    },
  };
  return normalize(schema);
}
