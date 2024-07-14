import html from 'utils';
import { onActivated, onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';

export default {
  template: html`
<el-input v-model="model" />
<div>{{created}}</div>
    `,
  setup() {
    const router = useRouter();
    onMounted(() => {
      created.value = new Date();
      console.log(`onMounted:${router.currentRoute.value.fullPath}`);
    });
    onActivated(() => {
      console.log(`onActivated:${router.currentRoute.value.fullPath}`);
    });
    const created = ref(null);
    return {
      created,
      model: ref(''),
    };
  },
};
