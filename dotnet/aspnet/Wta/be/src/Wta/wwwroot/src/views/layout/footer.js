import { useAppStore } from '@/store/index.js';
import html, { format } from 'utils';

export default {
  template: html`<div class="footer flex items-center justify-center h-full" style="height:60px">{{format($t('copyright'),new Date().getFullYear())}}</div>`,
  setup() {
    const appStore = useAppStore();
    return {
      appStore,
      format,
    };
  },
};