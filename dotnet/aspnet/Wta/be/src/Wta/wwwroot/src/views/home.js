import Md from '@/views/components/markdown/index.js';
import html from 'utils';
import { ref } from 'vue';

export default {
  components: { Md },
  template: html`
<el-row>
  <el-col class="py-8">
    <el-carousel :interval="4000" type="card" height="240px">
      <el-carousel-item>
        <img src="./src/assets/images/1.png" />
      </el-carousel-item>
      <el-carousel-item>
        <img src="./src/assets/images/2.png" />
      </el-carousel-item>
      <el-carousel-item>
        <img src="./src/assets/images/3.png" />
      </el-carousel-item>
    </el-carousel>
  </el-col>
</el-row>
<el-row>
  <el-col class="py-8">
    <img src="./src/assets/images/4.png" style="margin:0 auto" />
  </el-col>
</el-row>
<el-row>
    <el-col class="py-8">
      <el-tabs type="border-card" tab-position="top">
        <el-tab-pane v-for="(item1,index1) in list" :label="item1.label">
        <el-tabs tab-position="left">
          <el-tab-pane v-for="(item2,index2) in item1.children" :label="item2.label">
            <md :name="'flow/'+((index1+1)*10+index2+1)" />
          </el-tab-pane>
        </el-tabs>
        </el-tab-pane>
      </el-tabs>
    </el-col>
</el-row>
  `,
  styles: html`
  <style>
  .el-carousel__item h3 {
    color: #475669;
    opacity: 0.75;
    line-height: 200px;
    margin: 0;
    text-align: center;
  }

  .el-carousel__item:nth-child(2n) {
    background-color: #99a9bf;
  }

  .el-carousel__item:nth-child(2n + 1) {
    background-color: #d3dce6;
  }
  .el-tabs__nav-scroll{
    display:flex;
    justify-content: center;
  }
</style>
  `,
  setup() {
    const counter = ref(0);
    const list = [
      {
        label: '技术管理',
        children: [
          {
            label: '产品信息',
          },
          {
            label: '生产工序',
          },
          {
            label: '产品BOM',
          },
          {
            label: '基础设置',
          },
        ],
      },
      {
        label: '计划管理',
        children: [
          {
            label: '生产计划',
          },
          {
            label: '计划看板',
          },
          {
            label: '执行跟踪',
          },
          {
            label: '费用统计',
          },
          {
            label: '用料统计',
          },
        ],
      },
      {
        label: '生产管理',
        children: [
          {
            label: '生产工单',
          },
          {
            label: '生产领料',
          },
          {
            label: '生产退料',
          },
          {
            label: '生产报工',
          },
          {
            label: '生产入库',
          },
        ],
      },
      {
        label: '委外管理',
        children: [
          {
            label: '供应商管理',
          },
          {
            label: '委外工单',
          },
          {
            label: '委外领料',
          },
          {
            label: '委外入库',
          },
        ],
      },
      {
        label: '库存管理',
        children: [
          {
            label: '仓库管理',
          },
          {
            label: '入库单',
          },
          {
            label: '出库单',
          },
          {
            label: '库存调拨',
          },
          {
            label: '库存盘点',
          },
          {
            label: '出入库统计',
          },
        ],
      },
    ];
    return {
      counter,
      list,
    };
  },
};
