import Layout from '@/views/components/layout/portal-layout.js';
import html from 'utils';
import { onMounted } from 'vue';

export default {
  components: { Layout },
  template: `
<el-row>
  <el-col class="py-8">
    <img src="./src/assets/images/4.png" style="margin:0 auto" />
  </el-col>
</el-row>
    `,
  setup() {
    console.log('test:setup');
    onMounted(() => {
      console.log('test:onMounted');
    });
  },
};
