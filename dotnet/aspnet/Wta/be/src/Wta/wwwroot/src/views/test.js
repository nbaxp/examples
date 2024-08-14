import QrCode from '@/components/form/qr-code.js';
import html from 'utils';
import { onMounted } from 'vue';
import { useRouter } from 'vue-router';

export default {
  components: { QrCode },
  template: html`<qr-code
    v-model="https://www.1stg.me"
  />`,
  setup() {
    const router = useRouter();
    onMounted(() => {
      console.log(router.currentRoute.value.fullPath);
    });
  },
};
