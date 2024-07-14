const rules = [
  {
    required: true,
  },
];

export default function () {
  return {
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
        title: '菜单栏折叠',
        rules,
        type: 'boolean',
      },
      size: {
        title: '控件大小',
        input: 'select',
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
      mode: {
        title: '主题模式',
        input: 'select',
        options: [
          { value: 'auto', label: '系统', icon: 'platform' },
          { value: 'light', label: '浅色', icon: 'sunny' },
          { value: 'dark', label: '深色', icon: 'moon' },
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
    },
  };
}
