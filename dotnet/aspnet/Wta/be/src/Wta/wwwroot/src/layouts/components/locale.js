import SvgIcon from '@/components/icon/index.js';
import { useTabsStore } from '@/store/index.js';
import html from 'utils';
import { nextTick } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRouter } from 'vue-router';
import config from '~/src/locales/config.js';

export default {
  components: { SvgIcon },
  template: html`<el-dropdown class="cursor-pointer">
    <span class="el-dropdown-link flex">
      <el-icon :size="18">
        <svg-icon name="lang" />
      </el-icon>
    </span>
    <template #dropdown>
      <el-dropdown-menu>
        <el-dropdown-item v-for="locale in $i18n.availableLocales" @click="change(locale)">
          {{config.options.find(o=>o.key===locale)?.value}}
          <el-icon class="el-icon--right" v-if="locale===$i18n.locale">
            <ep-select />
          </el-icon>
        </el-dropdown-item>
      </el-dropdown-menu>
    </template>
  </el-dropdown>`,
  setup() {
    const i18n = useI18n();
    const tabsStore = useTabsStore();
    const router = useRouter();
    const change = (locale) => {
      i18n.locale.value = locale;
      tabsStore.isRefreshing = true;
      nextTick(() => {
        tabsStore.isRefreshing = false;
      });
    };
    return {
      config,
      change,
    };
  },
};
