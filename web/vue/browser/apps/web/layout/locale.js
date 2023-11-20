import html from "utils";
import { useI18n } from "vue-i18n";
import SvgIcon from "../components/icon/index.js";
import { options } from "../locale/index.js";

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
          {{options.find(o=>o.value===locale).label}}
          <el-icon class="el-icon--right" v-if="locale===$i18n.locale">
            <ep-select />
          </el-icon>
        </el-dropdown-item>
      </el-dropdown-menu>
    </template>
  </el-dropdown>`,
  setup() {
    const i18n = useI18n();
    const change = (locale) => {
      i18n.locale.value = locale;
    };

    return {
      options,
      change,
    };
  },
};
