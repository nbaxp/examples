import createRoute, { createPage, createButton } from "./utils.js";

export default [
  {
    ...createRoute("base-data", "title=基础数据"),
    children: [
      {
        ...createPage("user", "title=用户管理"),
        children: [
          createButton("query", "title=查询&isTop=true"),
          createButton("create", "title=新建&isTop=true"),
          createButton("update", "title=编辑"),
          createButton("delete", "title=删除&disabled=o=>o.userName==='admin'"),
          createButton("reset-password", "title=重置密码&method=PUT"),
        ],
      },
      {
        ...createPage("role", "title=角色管理"),
        children: [createButton("query", "title=查询&isTop=true"), createButton("create", "title=新建&isTop=true"), createButton("delete", "title=删除&disabled=o => o.isStatic")],
      },
      // {
      //   ...createPage("material", "title=物料主数据"),
      //   children: [createButton("query", "title=查询&isTop=true"), createButton("export", "title=导出&isTop=true&pattern=paged")],
      // },
      {
        ...createPage("version", "title=期间设置"),
        children: [
          createButton("query", "title=查询&isTop=true"),
          createButton("create", "title=新建&isTop=true"),
          createButton("open-version", "title=启用&isTop=true"),
          createButton("closed-version", "title=停用&isTop=true"),
          createButton("delete", "title=删除&isTop=true"),
        ],
      },
      {
        ...createPage("material-relationship", "title=客户零件关系"),
        children: [createButton("query", "title=查询&isTop=true"), createButton("import", "title=导入&isTop=true"), createButton("export", "title=导出&isTop=true&pattern=paged")],
      },
      {
        ...createPage("parts-relationship", "title=变更新旧LU关系"),
        children: [createButton("query", "title=查询&isTop=true"), createButton("import", "title=导入&isTop=true"), createButton("export", "title=导出&isTop=true&pattern=paged")],
      },
      {
        ...createPage("code-setting", "title=通用代码"),
        children: [
          createButton("query", "title=查询&isTop=true"),
          createButton("create", "title=新建&isTop=true"),
          createButton("delete", "title=删除&isTop=true"),
          createButton("import", "title=导入&isTop=true"),
          createButton("export", "title=导出&isTop=true&pattern=paged"),
        ],
      },
      // {
      //   ...createPage("bom", "title=BOM结构"),
      //   children: [createButton("query", "title=查询&isTop=true"), createButton("export", "title=导出&isTop=true&pattern=paged")],
      // },
      {
        ...createPage("bei-jian", "title=备件价格单"),
        children: [
          createButton("query", "title=查询&isTop=true"),
          createButton("import", "title=导入&isTop=true"),
          createButton("export", "title=导出&isTop=true&pattern=paged"),
          createButton("enable", "title=启用&disabled=o=>o.isCancel===false"),
          createButton("disable", "title=停用&disabled=o=>o.isCancel===true"),
        ],
      },
      {
        ...createPage("yin-du-jian", "title=印度件价格单"),
        children: [
          createButton("query", "title=查询&isTop=true"),
          createButton("import", "title=导入&isTop=true"),
          createButton("export", "title=导出&isTop=true&pattern=paged"),
          createButton("enable", "title=启用&disabled=o=>o.isCancel===false"),
          createButton("disable", "title=停用&disabled=o=>o.isCancel===true"),
        ],
      },
      // {
      //   ...createPage("cai-gou", "title=采购价格单"),
      //   children: [createButton("query", "title=查询&isTop=true"), createButton("import", "title=导入&isTop=true"), createButton("export", "title=导出&isTop=true&pattern=paged")],
      // },
      {
        ...createPage("xiao-shou", "title=销售价格单"),
        children: [
          createButton("query", "title=查询&isTop=true"),
          createButton("import", "title=导入&isTop=true"),
          createButton("export", "title=导出&isTop=true&pattern=paged"),
          createButton("enable", "title=启用&disabled=o=>o.isCancel===false"),
          createButton("disable", "title=停用&disabled=o=>o.isCancel===true"),
        ],
      },
      // {
      //   ...createPage("ke-hu", "title=客户库位关系表"),
      //   children: [createButton("query", "title=查询&isTop=true"), createButton("import", "title=导入&isTop=true"), createButton("export", "title=导出&isTop=true&pattern=paged")],
      // },
      {
        ...createPage("job-item", "title=定时任务"),
        children: [
          createButton("query", "title=查询&isTop=true"),
          createButton("create", "title=新建&isTop=true"),
          createButton("delete", "title=删除&isTop=true"),
          createButton("update", "title=编辑"),
          createButton("log", "title=日志"),
        ],
      },
      {
        ...createPage("job-log", "title=任务日志&isHidden=true"),
        children: [createButton("query", "title=查询&isTop=true"), createButton("delete", "title=删除&isTop=true")],
      },
    ],
  },
];
