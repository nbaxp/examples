import html from 'utils';
import {onMounted} from 'vue';

export default {
  template: `<div>{{new Date()}}</div>
        <pre>{{JSON.stringify($router.getRoutes().find(o=>o.name==='root'),null,4)}}</pre>`,
  setup() {
    console.log('test:setup');
    onMounted(() => {
      console.log('test:onMounted');
    });
  },
};
