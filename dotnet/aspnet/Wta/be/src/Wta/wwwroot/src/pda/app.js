import { ConfigProvider } from 'vant';
import { Locale } from 'vant';
import html from 'utils';
import { ref, watchEffect } from 'vue';
import en from '~/lib/vant/es/locale/lang/en-US.mjs';
import zh from '~/lib/vant/es/locale/lang/zh-CN.mjs';
import { useMediaQuery } from '@vueuse/core';
import { useI18n } from 'vue-i18n';

export default {
  components: { ConfigProvider },
  template: html`
    <config-provider :theme="isDarkNow?'dark':'light'" theme-vars-scope="global">
      <router-view></router-view>
    </config-provider>
  `,
  setup() {
    const isDarkNow = useMediaQuery('(prefers-color-scheme: dark)');
    const options = ref({
      'zh-CN': zh,
      'en-US': en,
    });
    const i18n = useI18n();
    watchEffect(() => {
      Locale.use(i18n.locale.value, options.value[i18n.locale.value]);
    });
    return {
      isDarkNow,
    };
  },
};
