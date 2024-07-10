import { useAppStore } from "@/store/index.js";
import Icon from "@/views/components/icon/index.js";
import html from "utils";
import { useRouter } from "vue-router";
import MenuItem from "./menu-item.js";

export default {
	components: { Icon, MenuItem },
	template: html`<el-menu
    :collapse="appStore.isMenuCollapse"
    :collapse-transition="false"
    :default-active="$route.fullPath"
    router
  >
    <menu-item v-for="item in menus" v-model="item" parent="" />
  </el-menu>`,
	setup() {
		const appStore = useAppStore();
		const router = useRouter();
		const menus = router.getRoutes().find((o) => o.path === "/").children;
		return {
			appStore,
			menus,
		};
	},
};
