export default [
  {
    name: 'default',
    path: '/',
    redirect: '/home',
    component: 'index',
    meta: { title: 'Home Page', icon: 'home' },
    children: [
      {
        path: 'home',
        component: 'home',
        meta: {
          title: 'Home Page',
          icon: 'home',
        },
      },
      {
        path: 'user',
        meta: {
          title: 'User Center',
        },
        children: [
          {
            path: 'info',
            component: 'list',
            meta: {
              title: 'User Info',
            },
          },
          {
            path: 'settings',
            component: 'list',
            meta: {
              title: 'User Settings',
            },
          },
        ],
      },
      {
        path: 'system',
        meta: {
          title: 'System Management',
        },
        children: [
          {
            path: 'menu',
            component: 'list',
            meta: {
              title: 'Menu Management',
            },
          },
          {
            path: 'user',
            component: 'list',
            meta: {
              title: 'User Management',
            },
          },
          {
            type: 'page',
            path: 'role',
            component: 'list',
            meta: {
              title: 'Role Management',
              model: 'system/role',
            },
            children: [
              {
                type: 'button',
                meta: {
                  title: 'Search',
                  action: 'query',
                  method: 'POST',
                  api: 'role',
                },
              },
              {
                type: 'button',
                meta: {
                  title: 'Create',
                  action: 'create',
                  method: 'POST',
                  api: 'role',
                },
              },
              {
                type: 'button',
                meta: {
                  title: 'Edit',
                  action: 'edit',
                  method: 'POST',
                  api: 'role',
                },
              },
              {
                type: 'button',
                meta: {
                  title: 'Delete',
                  action: 'delete',
                  method: 'POST',
                  api: 'role',
                },
              },
            ],
          },
          {
            path: 'department',
            component: 'list',
            meta: {
              title: 'Department Management',
            },
          },
          {
            path: 'dictionary',
            component: 'list',
            meta: {
              title: 'Dictionary Management',
            },
          },
          {
            path: 'monitor',
            component: 'list',
            meta: {
              title: 'System Monitor',
            },
          },
        ],
      },
      {
        path: 'folder',
        component: 'folder',
        isMarkdown: true,
        meta: {
          title: '目录结构',
          icon: 'files',
          permission: '*',
        },
      },
      {
        path: 'i18n',
        component: 'i18n',
        isMarkdown: true,
        meta: {
          title: '国际化',
        },
      },
      {
        path: 'router',
        component: 'router',
        isMarkdown: true,
        meta: {
          title: '路由',
        },
      },
      {
        path: 'pinia',
        component: 'pinia',
        isMarkdown: true,
        meta: {
          title: '状态管理',
        },
      },
      {
        path: 'mock',
        component: 'mock',
        isMarkdown: true,
        meta: {
          title: '模拟数据',
        },
      },
      {
        path: 'basic',
        meta: {
          title: '开发基础',
        },
        children: [
          {
            path: 'git',
            component: 'git',
            isMarkdown: true,
            meta: {
              title: 'Git',
            },
          },
          {
            path: 'markdown',
            component: 'markdown',
            isMarkdown: true,
            meta: {
              title: 'Markdown',
            },
          },
        ],
      },
      {
        path: 'web',
        meta: {
          title: 'Web 基础',
        },
        children: [
          {
            path: 'html',
            component: 'web/html',
            isMarkdown: true,
            meta: {
              title: 'HTML',
            },
          },
          {
            path: 'css',
            meta: {
              title: 'CSS',
            },
            children: [
              {
                path: 'flex',
                component: 'web/css/flex',
                isMarkdown: true,
                meta: {
                  title: 'Flex 布局',
                },
              },
            ],
          },
          {
            path: 'js',
            meta: {
              title: 'JavaScript',
            },
            children: [
              {
                path: 'esm',
                component: 'web/javascript/esm',
                isMarkdown: true,
                meta: {
                  title: 'ESM 模块化',
                },
              },
              {
                path: 'promise',
                component: 'web/javascript/promise',
                isMarkdown: true,
                meta: {
                  title: 'Async 异步',
                },
              },
              {
                path: 'fetch',
                component: 'web/javascript/fetch',
                isMarkdown: true,
                meta: {
                  title: 'Fetch 网络请求',
                },
              },
            ],
          },
        ],
      },
      {
        path: 'vue',
        meta: {
          title: 'Vue 技术栈',
        },
        children: [
          {
            path: 'basic',
            component: 'vue/basic',
            isMarkdown: true,
            meta: {
              title: 'Vue 基础',
            },
          },
          {
            path: 'echarts',
            component: 'vue/echarts',
            meta: {
              title: 'Vue ECharts',
            },
          },
          {
            path: 'ep',
            meta: {
              title: 'Element Plus',
            },
            children: [
              {
                path: 'menu',
                component: 'ep/menu',
                isMarkdown: true,
                meta: {
                  title: '菜单 Menu',
                },
              },
            ],
          },
        ],
      },
    ],
  },
  {
    name: 'module2',
    path: '/m2',
    redirect: '/m2/home',
    component: 'index',
    meta: { title: 'Module2', icon: 'home' },
    children: [
      {
        path: 'home',
        component: 'home',
        meta: { title: 'Module2 Home Page' },
      },
    ],
  },
];
