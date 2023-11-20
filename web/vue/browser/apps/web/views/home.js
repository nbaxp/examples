import html from "utils";
import Md from "~/components/markdown/index.js";
import { onMounted } from "vue";
import { useUserStore } from "~/store/index.js";

export default {
  components: { Md },
  template: html`<md name="home" />`,
  setup() {
    const userStore = useUserStore();
    onMounted(async ()=>{
      await userStore.getUserInfo();
    });
  },
};
