import html from "utils";
import { useAppStore, useTabsStore } from "../store/index.js";
import { computed } from "vue";
import LayoutHeader from "./header.js";
import LayoutMenu from "./menu.js";
import LayoutTabs from "./tabs.js";
import LayoutBreadcrumb from "./breadcrumb.js";
import LayoutFooter from "./footer.js";

export default {
  components: {
    LayoutHeader,
    LayoutMenu,
    LayoutTabs,
    LayoutBreadcrumb,
    LayoutFooter,
  },
  template: html`<el-container>
    <el-header><layout-header /></el-header>
    <el-container>
      <el-aside width="auto">
        <el-scrollbar><layout-menu /></el-scrollbar>
      </el-aside>
      <el-container class="is-vertical main backtop">
        <layout-tabs v-if="appStore.useTabs" />
        <el-main>
          <layout-breadcrumb v-if="appStore.showBreadcrumb" />
          <router-view
            v-if="!tabsStore.isRefreshing"
            v-slot="{ Component, route }"
            class="router-view"
            :style="minHeight"
          >
            <component :is="Component" v-if="route.meta?.ignoreCache" :key="$route.fullPath" />
            <keep-alive :include="tabsStore.routes.map(o=>o.path)">
              <component :is="Component" v-if="!route.meta?.ignoreCache" :key="route.fullPath" />
            </keep-alive>
          </router-view>
          <el-footer v-if="appStore.showCopyright">
            <layout-footer />
          </el-footer>
        </el-main>
        <el-backtop target=".backtop > .el-main" />
      </el-container>
    </el-container>
  </el-container>`,
  setup() {
    const appStore = useAppStore();
    const tabsStore = useTabsStore();
    const path = computed(() => useRoute().matched[0].path);
    const items = computed(() => useRoute().matched[0].children);

    const minHeight = computed(() => {
      let height = 0;
      if (appStore.showBreadcrumb) {
        height += 40;
      }
      if (appStore.showCopyright) {
        height += 60;
      }
      const minHeight = height === 0 ? "" : `min-height:calc(100% - ${height}px);`;
      return minHeight;
    });
    return {
      appStore,
      tabsStore,
      path,
      items,
      minHeight,
    };
  },
};
