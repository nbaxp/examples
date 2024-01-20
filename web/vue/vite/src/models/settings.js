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
        type: 'boolean',
      },
      useMock: {
        type: 'boolean',
      },
      baseURL: {
        rules,
      },
      serverLocale: {
        type: 'boolean',
      },
      serverRoute: {
        type: 'boolean',
      },
      maxTabs: {
        type: 'integer',
        rules,
      },
      isMenuCollapse: {
        type: 'boolean',
      },
      size: {
        input: 'select',
        options: [
          { value: 'large', label: 'large' },
          { value: 'default', label: 'default' },
          { value: 'small', label: 'small' },
        ],
      },
      color: {
        input: 'color',
      },
      mode: {
        input: 'select',
        options: [
          { value: 'auto', label: 'system', icon: 'platform' },
          { value: 'light', label: 'light', icon: 'sunny' },
          { value: 'dark', label: 'dark', icon: 'moon' },
        ],
      },
      useDarkNav: {
        type: 'boolean',
      },
      useTabs: {
        type: 'boolean',
      },
      showBreadcrumb: {
        type: 'boolean',
      },
      showCopyright: {
        type: 'boolean',
      },
    },
  };
}
