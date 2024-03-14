import createRoute, { createPage, createButton } from "./utils.js";

// #region 数据输入
function createInputJieSuanShuju(path, business, client, title = "结算数据") {
  const routes = {
    ...createPage(path, `title=${title}&businessType=${business}&client=${client}`),
    component: "/input/jie-suan",
    children: [
      createButton("query", "title=查询&isTop=true"),
      //createButton("import", "title=导入&isTop=true"),
      createButton("export", "title=导出&pattern=row&key=billNum"),
      //createButton("delete", "title=删除&isTop=true"),
    ],
  };
  if (business !== "MaiDanJianBBAC") {
    routes.children.push(createButton("import", "title=导入&isTop=true"));
    routes.children.push(createButton("delete", "title=删除&isTop=true"));
  }
  return routes;
}

function createInputFaYunShuJu(path, business, client, title = "发运数据") {
  return {
    ...createPage(path, `title=${title}&businessType=${business}&client=${client}`),
    component: "/input/fa-yun",
    children: [createButton("query", "title=查询&isTop=true"), createButton("sync", "title=手动同步&isTop=true"), createButton("export", "title=导出&isTop=true&pattern=paged")],
  };
}

function createInputEdiShuJu(path, business, client, title = "EDI数据") {
  return {
    ...createPage(path, `title=${title}&businessType=${business}&client=${client}`),
    component: "/input/edi",
    children: [createButton("query", "title=查询&isTop=true"), createButton("export", "title=导出&isTop=true&pattern=paged")],
  };
}
// #endregion

// #region 数据比对
function createCompareFaYun(path, business, client, title = "EDI与发运对比") {
  return {
    ...createPage(path, `title=${title}&businessType=${business}&client=${client}`),
    component: "/compare/fa-yun",
    children: [
      createButton("query", "title=查询&isTop=true"),
      createButton("compare", "title=生成比对&isTop=true"),
      createButton("delete", "title=删除&isTop=true"),
      createButton("export", "title=下载&pattern=file&disabled=o=>o.stateName!='Succeeded'"),
    ],
  };
}

function createCompareJieSuan(path, business, client, title = "EDI、发运与结算比对") {
  return {
    ...createPage(path, `title=${title}&businessType=${business}&client=${client}`),
    component: "/compare/jie-suan",
    children: [
      createButton("query", "title=查询&isTop=true"),
      createButton("compare", "title=生成比对&isTop=true"),
      createButton("delete", "title=删除&isTop=true"),
      createButton("export", "title=下载&pattern=file&disabled=o=>o.stateName!='Succeeded'"),
      //createButton("export", "title=更新结算数据状态&isTop=true"),
    ],
  };
}
// #endregion

// #region 结算开票
function createUsableSettleList(path, business, client, title = "可结算单") {
  return {
    ...createPage(path, `title=${title}&businessType=${business}&client=${client}`),
    component: "/settle/usable",
    children: [
      createButton("query", "title=查询&isTop=true"),
      createButton("add", "title=创建发票"), //行级按钮
      createButton("export", "title=导出&isTop=true&pattern=paged"),
    ],
  };
}
function createUnableSettleList(path, business, client, title = "不可结算明细") {
  return {
    ...createPage(path, `title=${title}&businessType=${business}&client=${client}`),
    component: "/settle/unable",
    children: [createButton("query", "title=查询&isTop=true"), createButton("add", "title=生成可结算单&isTop=true"), createButton("export", "title=导出&isTop=true&pattern=paged")],
  };
}
function createCommerceCheckList(path, business, client, title = "商务审批") {
  return {
    ...createPage(path, `title=${title}&businessType=${business}&client=${client}`),
    component: "/settle/commerce",
    children: [
      createButton("query", "title=查询&isTop=true"),
      createButton("export", "title=导出&isTop=true&pattern=paged"),
      createButton(
        "approval",
        "title=商务审核通过&isTop=true",
        (_, q) => q.filters.some((o) => o.column === "state" && o.value === 1) && q.filters.some((o) => o.column === "invoiceState" && o.value === 1)
      ),
      createButton(
        "reject",
        "title=退回",
        (_, q) => q.filters.some((o) => o.column === "state" && o.value === 1) && q.filters.some((o) => o.column === "invoiceState" && o.value === 1)
      ),
      createButton("export-group", "title=导出发票分组&pattern=paged", (r, _) => r.invoiceState !== 2),
      createButton(
        "receive",
        "title=客户已收票&isTop=true",
        (_, q) => q.filters.some((o) => o.column === "state" && o.value === 3) && q.filters.some((o) => o.column === "invoiceState" && o.value === 1)
      ),
      // createButton(
      //   "bao-fei",
      //   "title=报废&isTop=true",
      //   (_, q) => q.filters.some((o) => o.column === "state" && (o.value === 3 || o.value === 4)) && q.filters.some((o) => o.column === "invoiceState" && o.value === 1)
      // ),
    ],
  };
}
function createVmiOutCheckList(path, business, client, title = "寄售库库存扣减审批") {
  return {
    ...createPage(path, `title=${title}&businessType=${business}&client=${client}`),
    component: "/settle/inventory",
    children: [
      createButton("query", "title=查询&isTop=true"),
      createButton("approval", "title=扣减审核通过&isTop=true"),
      createButton("reject", "title=退回&isTop=true"),
      createButton("export", "title=导出&isTop=true&pattern=paged"),
    ],
  };
}

// #endregion

export default [
  {
    ...createPage("input/jie-suan/detail", "title=数据输入结算数据明细&isHidden=true"),
    children: [createButton("query", "title=查询&isTop=true")],
  },
  {
    ...createPage("settle/detail", "title=结算单结算数据明细&isHidden=true"),
    children: [createButton("query", "title=查询&isTop=true")],
  },
  {
    ...createPage("settle/inventory-detail", "title=寄售库库存扣减审批明细&isHidden=true"),
    children: [createButton("query", "title=查询&isTop=true")],
  },
  {
    ...createRoute("jis-bbac", "title=JIS-BBAC"),
    children: [
      {
        ...createRoute("input", "title=数据输入"),
        children: [
          createInputJieSuanShuju("bbac_sa_service", "JisBBAC", "bbac-1040", "JIS-BBAC 结算数据"),
          createInputJieSuanShuju("bbac_sa_service2", "JisBBAC", "bbac-1046", "JIS-BBAC顺义 结算数据"),
          createInputFaYunShuJu("bbac_se_detail_service", "JisBBAC", "bbac", "JIS-BBAC 发运数据"),
          createInputEdiShuJu("bbac_se_edi_service", "JisBBAC", "bbac", "JIS-BBAC EDI数据"),
        ],
      },
      {
        ...createRoute("contrast", "title=数据比对"),
        children: [
          createCompareFaYun("bbac_sa_detail_jobservice", "JisBBAC", "bbac", "JIS-BBAC EDI与发运对比"),
          createCompareJieSuan("first_bbac_sa_detail_jobservice", "JisBBAC", "bbac", "JIS-BBAC EDI、发运与结算对比"),
        ],
      },
      {
        ...createRoute("settlement", "title=结算开票"),
        children: [
          createUsableSettleList("bbac_can_sa_service", "JisBBAC", "bbac", "JIS-BBAC 可结算单"),
          createUnableSettleList("bbac_not_sa_service", "JisBBAC", "bbac", "JIS-BBAC 不可结算明细"),
          createCommerceCheckList("bbac_ba_service", "JisBBAC", "bbac", "JIS-BBAC 商务审批"),
          createVmiOutCheckList("bbac_pd_service", "JisBBAC", "bbac", "JIS-BBAC 寄售库库存扣减审批"),
        ],
      },
    ],
  },
  {
    ...createRoute("jis-hbpo", "title=JIS-HBPO"),
    children: [
      {
        ...createRoute("input", "title=数据输入"),
        children: [
          createInputJieSuanShuju("hbpo_sa_service", "JisHBPO", "hbpo", "JIS-HBPO 结算数据"),
          createInputFaYunShuJu("hbpo_se_detail_service", "JisHBPO", "hbpo", "JIS-HBPO 发运数据"),
          createInputEdiShuJu("hbpo_se_edi_service", "JisHBPO", "hbpo", "JIS-HBPO EDI数据"),
        ],
      },
      {
        ...createRoute("contrast", "title=数据比对"),
        children: [
          createCompareFaYun("hbpo_sa_detail_jobservice", "JisHBPO", "hbpo", "JIS-HBPO EDI与发运对比"),
          createCompareJieSuan("first_hbpo_sa_detail_jobservice", "JisHBPO", "hbpo", "JIS-HBPO EDI、发运与结算对比"),
        ],
      },
      {
        ...createRoute("settlement", "title=结算开票"),
        children: [
          createUsableSettleList("hbpo_can_sa_service", "JisHBPO", "hbpo", "JIS-HBPO 可结算单"),
          createUnableSettleList("hbpo_not_sa_service", "JisHBPO", "hbpo", "JIS-HBPO 不可结算明细"),
          createCommerceCheckList("hbpo_ba_service", "JisHBPO", "hbpo", "JIS-HBPO 商务审批"),
          createVmiOutCheckList("hbpo_pd_service", "JisHBPO", "hbpo", "JIS-HBPO 寄售库库存扣减审批"),
        ],
      },
    ],
  },
  {
    ...createRoute("jit", "title=直供件"),
    children: [
      {
        ...createRoute("jit-bbac", "title=BBAC"),
        children: [
          {
            ...createRoute("input", "title=数据输入"),
            children: [
              createInputJieSuanShuju("bbac_jit_pub_sa_service", "ZhiGongJianBBAC", "bbac-1040", "直供件-BBAC 结算数据"),
              createInputJieSuanShuju("bbac_jit_pub_sa_service2", "ZhiGongJianBBAC", "bbac-1046", "直供件-BBAC顺义 结算数据"),
              createInputFaYunShuJu("bbac_jit_pub_se_detail_service", "ZhiGongJianBBAC", "bbac", "直供件-BBAC 发运数据"),
            ],
          },
          {
            ...createRoute("contrast", "title=数据比对"),
            children: [createCompareJieSuan("bbac_jit_pub_sa_detail_jobservice", "ZhiGongJianBBAC", "bbac", "直供件-BBAC 发运与结算对比")],
          },
          {
            ...createRoute("settlement", "title=结算开票"),
            children: [
              createUsableSettleList("bbac_jit_pub_can_sa_service", "ZhiGongJianBBAC", "bbac", "直供件-BBAC 可结算单"),
              createUnableSettleList("bbac_jit_pub_not_sa_service", "ZhiGongJianBBAC", "bbac", "直供件-BBAC 不可结算明细"),
              createCommerceCheckList("bbac_jit_pub_ba_service", "ZhiGongJianBBAC", "bbac", "直供件-BBAC 商务审批"),
              createVmiOutCheckList("bbac_jit_pub_pd_service", "ZhiGongJianBBAC", "bbac", "直供件-BBAC 寄售库库存扣减审批"),
            ],
          },
        ],
      },
      {
        ...createRoute("jit-hbpo", "title=HBPO"),
        children: [
          {
            ...createRoute("input", "title=数据输入"),
            children: [
              createInputJieSuanShuju("hbpo_jit_pub_sa_service", "ZhiGongJianHBPO", "hbpo", "直供件-HBPO 结算数据"),
              createInputFaYunShuJu("hbpo_jit_pub_se_detail_service", "ZhiGongJianHBPO", "hbpo", "直供件-HBPO 发运数据"),
            ],
          },
          {
            ...createRoute("contrast", "title=数据比对"),
            children: [createCompareJieSuan("bbac_jit_pub_sa_detail_jobservice", "ZhiGongJianHBPO", "hbpo", "直供件-HBPO 发运与结算对比")],
          },
          {
            ...createRoute("settlement", "title=结算开票"),
            children: [
              createUsableSettleList("hbpo_jit_pub_can_sa_service", "ZhiGongJianHBPO", "hbpo", "直供件-HBPO 可结算单"),
              createUnableSettleList("hbpo_jit_pub_not_sa_service", "ZhiGongJianHBPO", "hbpo", "直供件-HBPO 不可结算明细"),
              createCommerceCheckList("hbpo_jit_pub_ba_service", "ZhiGongJianHBPO", "hbpo", "直供件-HBPO 商务审批"),
              createVmiOutCheckList("hbpo_jit_pub_pd_service", "ZhiGongJianHBPO", "hbpo", "直供件-HBPO 寄售库库存扣减审批"),
            ],
          },
        ],
      },
    ],
  },
  {
    ...createRoute("md", "title=买单件"),
    children: [
      {
        ...createRoute("md-bbac", "title=BBAC"),
        children: [
          {
            ...createRoute("input", "title=数据输入"),
            children: [
              createInputJieSuanShuju("bbac_md_pub_sa_service", "MaiDanJianBBAC", "bbac", "买单件-BBAC 结算数据"),
              createInputFaYunShuJu("bbac_md_pub_se_detail_service", "MaiDanJianBBAC", "bbac", "买单件-BBAC 发运数据"),
            ],
          },
          {
            ...createRoute("contrast", "title=数据比对"),
            children: [createCompareJieSuan("bbac_md_pub_sa_detail_jobservice", "MaiDanJianBBAC", "bbac", "买单件-BBAC 发运与结算对比")],
          },
        ],
      },
      {
        ...createRoute("md-hbpo", "title=HBPO"),
        children: [
          {
            ...createRoute("input", "title=数据输入"),
            children: [
              createInputJieSuanShuju("hbpo_md_pub_sa_service", "MaiDanJianHBPO", "hbpo", "买单件-HBPO 结算数据"),
              createInputFaYunShuJu("hbpo_md_pub_se_detail_service", "MaiDanJianHBPO", "hbpo", "买单件-HBPO 发运数据"),
            ],
          },
          {
            ...createRoute("contrast", "title=数据比对"),
            children: [createCompareJieSuan("hbpo_md_pub_sa_detail_jobservice", "MaiDanJianHBPO", "hbpo", "买单件-HBPO 发运与结算对比")],
          },
          {
            ...createRoute("settlement", "title=结算开票"),
            children: [
              createUsableSettleList("hbpo_md_pub_can_sa_service", "MaiDanJianHBPO", "hbpo", "买单件-HBPO 可结算单"),
              createUnableSettleList("hbpo_md_pub_not_sa_service", "MaiDanJianHBPO", "hbpo", "买单件-HBPO 不可结算明细"),
              createCommerceCheckList("hbpo_md_pub_ba_service", "MaiDanJianHBPO", "hbpo", "买单件-HBPO 商务审批"),
              createVmiOutCheckList("hbpo_md_pub_pd_service", "MaiDanJianHBPO", "hbpo", "买单件-HBPO 寄售库库存扣减审批"),
            ],
          },
        ],
      },
    ],
  },
  {
    ...createRoute("bj", "title=备件"),
    children: [
      {
        ...createRoute("input", "title=数据输入"),
        children: [
          createInputJieSuanShuju("bj_pub_sa_service", "BeiJian", "bbac", "备件-BBAC 结算数据"),
          createInputFaYunShuJu("bj_pub_se_detail_service", "BeiJian", "bbac", "备件-BBAC 发运数据"),
        ],
      },
      {
        ...createRoute("contrast", "title=数据比对"),
        children: [createCompareJieSuan("bj_pub_sa_detail_jobservice", "BeiJian", "bbac", "备件-BBAC 发运与结算对比")],
      },
      {
        ...createRoute("settlement", "title=结算开票"),
        children: [
          createUsableSettleList("bj_pub_can_sa_service", "BeiJian", "bbac", "备件-BBAC 可结算单"),
          createUnableSettleList("bj_pub_not_sa_service", "BeiJian", "bbac", "备件-BBAC 不可结算明细"),
          createCommerceCheckList("bj_pub_ba_service", "BeiJian", "bbac", "备件-BBAC 商务审批"),
          createVmiOutCheckList("bj_pub_pd_service", "BeiJian", "bbac", "备件-BBAC 寄售库库存扣减审批"),
        ],
      },
    ],
  },
  {
    ...createRoute("in", "title=印度件"),
    children: [
      {
        ...createRoute("input", "title=数据输入"),
        children: [
          createInputJieSuanShuju("in_pub_sa_service", "YinDuJian", "bbac", "印度件-BBAC 结算数据"),
          createInputFaYunShuJu("in_pub_se_detail_service", "YinDuJian", "bbac", "印度件-BBAC 发运数据"),
        ],
      },
      {
        ...createRoute("contrast", "title=数据比对"),
        children: [createCompareJieSuan("in_pub_sa_detail_jobservice", "YinDuJian", "bbac", "印度件-BBAC 发运与结算对比")],
      },
      {
        ...createRoute("settlement", "title=结算开票"),
        children: [
          createUsableSettleList("in_pub_can_sa_service", "YinDuJian", "bbac", "印度件-BBAC 可结算单"),
          createUnableSettleList("in_pub_not_sa_service", "YinDuJian", "bbac", "印度件-BBAC 不可结算明细"),
          createCommerceCheckList("in_pub_ba_service", "YinDuJian", "bbac", "印度件-BBAC 商务审批"),
          createVmiOutCheckList("in_pub_pd_service", "YinDuJian", "bbac", "印度件-BBAC 寄售库库存扣减审批"),
        ],
      },
    ],
  },
  {
    ...createRoute("finance", "title=财务审核"),
    children: [
      {
        ...createPage("check", `title=财务审核`),
        component: "/finance/check",
        children: [
          createButton("query", "title=查询&isTop=true"),
          createButton("export", "title=导出&isTop=true&pattern=paged"),
          createButton(
            "import",
            "title=导入开票文件&isTop=true",
            (_, q) => q.filters.some((o) => o.column === "state" && o.value === 2) && q.filters.some((o) => o.column === "invoiceState" && o.value === 1)
          ),
          createButton(
            "approval",
            "title=财务审核通过&isTop=true",
            (_, q) => q.filters.some((o) => o.column === "state" && o.value === 2) && q.filters.some((o) => o.column === "invoiceState" && o.value === 1)
          ),
          createButton("export-group", "title=导出发票分组&pattern=paged", (r, _) => r.invoiceState !== 2),
          //createButton("approval", "title=发票重开"),
          createButton(
            "reject",
            "title=退回&isTop=true",
            (_, q) => q.filters.some((o) => o.column === "state" && o.value === 2) && q.filters.some((o) => o.column === "invoiceState" && o.value === 1)
          ),
          createButton(
            "sync",
            "title=同步到QAD&isTop=true",
            (_, q) => q.filters.some((o) => o.column === "state" && o.value === 5) && q.filters.some((o) => o.column === "invoiceState" && o.value === 1)
          ),
        ],
      },
      {
        ...createPage("sync", `title=Qad发票同步状态查询`),
        component: "/finance/sync",
        children: [createButton("query", "title=查询&isTop=true"), createButton("export", "title=导出&isTop=true&pattern=paged")],
      },
    ],
  },
  {
    ...createRoute("cost", "title=实际采购成本&isHidden=true"),
    children: [
      {
        ...createPage("cost", "title=实际采购成本", "/settle/cost"),
        children: [createButton("query", "title=查询&isTop=true"), createButton("export", "title=导出&pattern=paged"), createButton("add", "title=生成&pattern=paged")],
      },
    ],
  },
  {
    ...createRoute("vmi", "title=寄售库"),
    children: [
      {
        ...createPage("balance", "title=库存余额查询"),
        children: [createButton("query", "title=查询&isTop=true"), createButton("export", "title=导出&isTop=true&pattern=paged")],
      },
      {
        ...createPage("sum", "title=库存余额汇总"),
        children: [createButton("query", "title=查询&isTop=true"), createButton("export", "title=导出&isTop=true&pattern=paged")],
      },
      {
        ...createPage("backup", "title=时点库存余额查询"),
        children: [
          createButton("query", "title=查询&isTop=true"),
          createButton("export", "title=导出&isTop=true&pattern=paged"),
          createButton("invoke", "title=手动备份&isTop=true"),
        ],
      },
      {
        ...createPage("log", "title=库存事务查询"),
        path: "log",
        children: [
          createButton("query", "title=查询&isTop=true"),
          createButton("export", "title=导出&isTop=true&pattern=paged"),
          createButton("export-replenishment", "title=补货数据导出&isTop=true&pattern=paged"),
        ],
      },
      {
        ...createPage("adjust", "title=寄售库存调整"),
        children: [createButton("query", "title=查询&isTop=true"), createButton("create", "title=新建&isTop=true"), createButton("import", "title=导入&isTop=true")],
      },
    ],
  },
];
