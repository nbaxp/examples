import { useAppStore } from '@/store/index.js';
import { ElConfigProvider } from 'element-plus';
import en from 'element-plus/locale/en.min.js';
import zh from 'element-plus/locale/zh-cn.min.js';
import html from 'utils';
import { ref } from 'vue';

export default {
  components: { ElConfigProvider },
  template: html`
<el-config-provider :locale="options[$i18n.locale]" :size="appStore.size??'default'">
  <router-view></router-view>
</el-config-provider>
`,
  setup() {
    const options = ref({
      'zh-CN': zh,
      'en-US': en,
    });
    const appStore = useAppStore();
    return {
      options,
      appStore,
    };
  },
};
