import html from 'utils';
import { onMounted } from 'vue';
import VueQrcode from 'vue-qrcode';
import { useRouter } from 'vue-router';

export default {
  compoents: { VueQrcode },
  template: html`<vue-qrcode
    value="https://www.1stg.me"
    @change="onDataUrlChange"
  />`,
  setup() {
    const router = useRouter();
    onMounted(() => {
      console.log(router.currentRoute.value.fullPath);
    });
  },
};
