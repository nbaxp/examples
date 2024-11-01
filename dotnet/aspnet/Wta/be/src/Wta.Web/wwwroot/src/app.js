import { useAppStore } from '@/store/index.js';
import { ElConfigProvider } from 'element-plus';
import html from 'utils';
import { ref } from 'vue';
import en from '~/lib/element-plus/locale/en.min.js';
import zh from '~/lib/element-plus/locale/zh-cn.min.js';
import $ from '~/lib/jquery/jquery.esm.js';

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
    $('body').on(
      'click',
      '.el-radio:not(.is-checked) + .el-cascader-node__label',
      (e) => {
        $(e.target).prev('.el-radio:not(.is-checked)')[0].click();
      },
    );
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
