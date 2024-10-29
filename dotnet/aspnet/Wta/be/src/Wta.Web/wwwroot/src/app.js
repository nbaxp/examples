import { useAppStore } from '@/store/index.js';
import { ElConfigProvider } from 'element-plus';
import html from 'utils';
import { ref } from 'vue';
import en from '~/lib/element-plus/locale/en.min.js';
import zh from '~/lib/element-plus/locale/zh-cn.min.js';

export default {
  components: { ElConfigProvider },
  template: html`
    <el-config-provider
      :locale="options[$i18n.locale]"
      :size="appStore.settings.size??'default'"
      :button="{autoInsertSpace:true}"
    >
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
