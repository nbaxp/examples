import html from 'utils';
import { onMounted } from 'vue';
import { useRouter } from 'vue-router';

export default {
  template: html`
<div>{{new Date()}}</div>
    `,
  setup() {
    const router = useRouter();
    onMounted(() => {
      console.log(router.currentRoute.value.fullPath);
    });
  },
};
