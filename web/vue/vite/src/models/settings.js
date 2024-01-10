const rules = [
  {
    required: true,
  },
];

export default function () {
  return {
    title: '配置',
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
        title: 'API路径',
        rules,
      },
      serverLocale: {
        title: '服务端资源',
        type: 'boolean',
      },
      serverRoute: {
        title: '服务端路由',
        type: 'boolean',
      },
      maxTabs: {
        title: 'Tabs数量',
        type: 'integer',
        rules,
      },
      isMenuCollapse: {
        title: '折叠菜单',
        type: 'boolean',
      },
      size: {
        title: '组件大小',
        input: 'select',
        options: [
          { value: 'large', label: '大' },
          { value: 'default', label: '默认' },
          { value: 'small', label: '小' },
        ],
      },
      color: {
        title: '主题颜色',
        input: 'color',
      },
      mode: {
        title: '模式',
        input: 'select',
        options: [
          { value: 'auto', label: '跟随系统', icon: 'platform' },
          { value: 'light', label: '亮色模式', icon: 'sunny' },
          { value: 'dark', label: '暗色模式', icon: 'moon' },
        ],
      },
      useDarkNav: {
        title: '暗色导航',
        type: 'boolean',
      },
      useTabs: {
        title: '多标签页',
        type: 'boolean',
      },
      showBreadcrumb: {
        title: '显示 Breadcrumb',
        type: 'boolean',
      },
      showCopyright: {
        title: '显示 Footer',
        type: 'boolean',
      },
    },
  };
}
