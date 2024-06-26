import html from "html";
import { useAppStore } from "../store/index.js";
import { reload } from "../utils/index.js";

export default {
  template: html`<a href="javascript:;" @click="reload" class="logo">
    <div class="flex h-full items-center">
      <img src="/assets/logo.svg" />
      <h1 v-if="!appStore.isMenuCollapse">{{$t('application')}}</h1>
    </div>
  </a>`,
  setup() {
    const appStore = useAppStore();
    return {
      appStore,
      reload,
    };
  },
};
