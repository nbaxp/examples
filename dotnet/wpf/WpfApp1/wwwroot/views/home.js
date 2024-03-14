import html from "html";

export default {
  template: html`<a href="javascript:;" @click="download">导入模板下载</a>
    <br />
    <el-tree v-if="0" default-expand-all :data="list" :props="props" @node-click="handleNodeClick" /> `,
  setup() {
    const download = () => {
      window.open(`../assets/导入模版.zip?time=${Date.now()}`);
    };
    const page = "page";
    const createRoute = (path, title, type = "group") => {
      return {
        path,
        title,
        meta: {
          title,
          type,
        },
      };
    };
    const createGroup = (input = 1, compare = 1, settle = 1) => {
      const result = [];
      if (input) {
        result.push({
          ...createRoute("input", "数据输入"),
          children: [{ ...createRoute("jie-suan", "结算数据", page) }, { ...createRoute("fa-yun", "发运数据", page) }, { ...createRoute("edi", "EDI数据", page) }],
        });
      }
      if (compare) {
        result.push({
          ...createRoute("compare", "数据比对"),
          children: [{ ...createRoute("fa-yun", "EDI与发运数据", page) }, { ...createRoute("jie-suan", "EDI、发运与计算数据比对", page) }],
        });
      }
      if (settle) {
        result.push({
          ...createRoute("settle", "结算开票"),
          children: [
            { ...createRoute("ke-jie-suan", "可结算单", page) },
            { ...createRoute("bu-ke-jie-suan", "不可结算单", page) },
            { ...createRoute("shang-wu", "商务审批", page) },
            { ...createRoute("ku-cun", "寄售库库存扣减审批", page) },
          ],
        });
      }
      return result;
    };
    const list = [
      {
        ...createRoute("bbac", "BBAC"),
        children: [
          {
            ...createRoute("jis", "JIS"),
            children: createGroup(),
          },
          {
            ...createRoute("zhi-gong", "直供件"),
            children: createGroup(),
          },
          {
            ...createRoute("yin-du", "印度件"),
            children: createGroup(),
          },
          {
            ...createRoute("bei-jian", "备件"),
            children: createGroup(),
          },
          {
            ...createRoute("mai-dan", "买单件"),
            children: createGroup(1, 1, 0),
          },
        ],
      },
      {
        ...createRoute("hbpo", "HBPO"),
        children: [
          {
            ...createRoute("jis", "JIS"),
            children: createGroup(),
          },
          {
            ...createRoute("zhi-gong", "直供件"),
            children: createGroup(),
          },
          // {
          //   ...createRoute("yin-du", "印度件"),
          //   children: createGroup(0, 0, 0),
          // },
          // {
          //   ...createRoute("bei-jian", "备件"),
          //   children: createGroup(0, 0, 0),
          // },
          {
            ...createRoute("mai-dan", "买单件"),
            children: createGroup(),
          },
        ],
      },
    ];
    const props = {
      label: (data) => {
        return data.meta?.title;
      },
    };
    const handleNodeClick = (a, b, c, d) => {
      console.log(a);
      console.log(b);
      console.log(c);
      console.log(d);
      if (a.meta?.type === "page") {
        console.log(a.path);
      }
    };
    return {
      download,
      list,
      props,
      handleNodeClick,
    };
  },
};
