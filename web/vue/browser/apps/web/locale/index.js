import { createI18n } from 'vue-i18n';
import useAppStore from '@/store/app.js';
import { log } from 'utils';

let options = null;
let i18n = null;

export default async function () {
  if (i18n === null) {
    log('init i18n');
    const appStore = useAppStore();
    const config = await appStore.getSiteInfo();
    options = config.options;
    config.locale = localStorage.getItem('locale') || config.locale || 'zh-CN';
    i18n = createI18n(config);
  }

  return i18n;
}

export { options };
