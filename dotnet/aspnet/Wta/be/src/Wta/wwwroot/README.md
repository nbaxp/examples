# 说明

## 项目结构

```txt
.
├── README.md                         # 说明文档
├── index.html                        # 主 html 文件
├── lib                               # 项目依赖
├── mock                              # mock 目录
│     └── index.ts
├── package.json
├── package-lock.json
├── node_modules                      # 项目依赖
├── src                               # 项目代码
├── .prettierrc.json
├── biome.json
├── .gitignore
└── .gitattributres
```

## 项目代码结构

```txt
src
├── app.js
├── assets                            # 资源层
├── components                        # 公共组件层
├── config                            # 配置
├── constants                         # 常量
├── hooks                             # 钩子
├── layouts                           # 布局
├── views                             # 页面
├── router                            # 路由
├── store                             # Pinia 数据
├── style                             # 样式
└── utils                             # 工具层
│     ├── index.js                      # 通用
│     ├── request.js                    # 请求
│     ├── validation.ts                 # 模型验证
│     └── schema                        # json schema 相关
├── main.css                          # css 入口
└── main.js                           # js 入口
```

## 路由与菜单
