import html from 'utils';
import { ElConfigProvider } from 'element-plus';
import zh from '@/lib/element-plus/locale/zh-cn.min.js';
import en from '@/lib/element-plus/locale/en.min.js';
import { ref } from 'vue';
import { useAppStore } from '@/store/index.js';

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
