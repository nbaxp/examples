import Layout from '@/views/components/layout/portal-layout.js';
import html from 'utils';
import { onMounted } from 'vue';

export default {
  components: { Layout },
  template: `
<layout>
About
</layout>
    `,
  setup() {
    console.log('test:setup');
    onMounted(() => {
      console.log('test:onMounted');
    });
  },
};
