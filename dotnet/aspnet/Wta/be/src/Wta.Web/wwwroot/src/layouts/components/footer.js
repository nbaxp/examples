import { useAppStore } from '@/store/index.js';
import html, { format } from 'utils';

export default {
  template: html`
    <div class="footer flex items-center justify-center w-full h-full" style="height:60px">
      {{$t('版权信息',[new Date().getFullYear()])}}
    </div>
  `,
  setup() {
    const appStore = useAppStore();
    return {
      appStore,
      format,
    };
  },
};
