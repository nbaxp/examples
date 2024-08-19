import { useAppStore } from '@/store/index.js';
import { reload } from '@/utils/index.js';
import html from 'utils';

export default {
  template: html`
    <a href="javascript:;" @click="reload" class="logo">
      <div class="flex h-full items-center">
        <img src="./src/assets/logo.svg" />
        <h1 v-if="!appStore.settings.isMenuCollapse">{{$t('应用名称')}}</h1>
      </div>
    </a>
  `,
  setup() {
    const appStore = useAppStore();
    return {
      appStore,
      reload,
    };
  },
};
