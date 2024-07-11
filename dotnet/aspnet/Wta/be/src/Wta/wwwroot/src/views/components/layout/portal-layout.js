import { useAppStore } from "@/store/index.js";
import AppForm from "@/views/components/form/index.js";
import html from "utils";
import { ref } from "vue";
import LayoutHeader from "./footer.js";
import LayoutFooter from "./footer.js";

export default {
	components: { AppForm, LayoutHeader,LayoutFooter },
	template: html`
    <el-container class="is-vertical main backtop">
      <el-header><layout-header /></el-header>
      <el-main class="flex items-center justify-center">
        <div>
          <router-view></router-view>
        </div>
      </el-main>
      <el-footer v-if="appStore.settings.showCopyright">
        <layout-footer />
      </el-footer>
      <el-backtop target=".backtop > .el-main" />
    </el-container>
  `,
	setup() {
		const appStore = useAppStore();
		return {
			appStore,
		};
	},
};
