import html from "utils";
import { computed } from "vue";
import { useAppStore } from "@/store/index.js";
import SvgIcon from "@/components/icon/index.js";

export default {
  components: { SvgIcon },
  template: html`
    <el-icon
      v-model="isDark"
      @click="change()"
      :size="18"
      class="cursor-pointer"
    >
      <ep-sunny v-if="isDark" />
      <ep-moon v-else />
    </el-icon>
  `,
  setup() {
    const appStore = useAppStore();
    const isDark = computed(() => appStore.settings.theme !== "light");
    const change = () => {
      appStore.settings.theme = isDark.value ? "light" : "dark";
    };
    return {
      isDark,
      change,
    };
  },
};
