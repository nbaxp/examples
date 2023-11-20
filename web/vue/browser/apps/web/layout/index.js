import html from "utils";
import { useAppStore, useTabsStore } from "../store/index.js";
import { computed } from "vue";
import LayoutHeader from "./header.js";
import LayoutMenu from "./menu.js";
import LayoutTabs from "./tabs.js";
import LayoutFooter from "./footer.js";

export default {
  components: {
    LayoutHeader,
    LayoutMenu,
    LayoutTabs,
    LayoutFooter,
  },
  template: html`<el-container>
    <el-header><layout-header /></el-header>
    <el-container>
      <el-aside width="auto">
        <el-scrollbar><layout-menu /></el-scrollbar>
      </el-aside>
      <el-container class="backtop">
        <el-scrollbar>
          <layout-tabs />
          <el-main>
            <router-view v-if="!tabsStore.isRefreshing" v-slot="{ Component, route }">
              <component :is="Component" v-if="route.meta?.ignoreCache" :key="$route.fullPath" />
              <keep-alive :include="tabsStore.routes.map(o=>o.path)">
                <component :is="Component" v-if="!route.meta?.ignoreCache" :key="route.fullPath" />
              </keep-alive>
            </router-view>
          </el-main>
          <el-footer>
            <layout-footer />
          </el-footer>
          <el-backtop target=".backtop > .el-scrollbar > .el-scrollbar__wrap" />
        </el-scrollbar>
      </el-container>
    </el-container>
  </el-container>`,
  setup() {
    const appStore = useAppStore();
    const tabsStore = useTabsStore();
    const path = computed(() => useRoute().matched[0].path);
    const items = computed(() => useRoute().matched[0].children);
    return {
      appStore,
      tabsStore,
      path,
      items,
    };
  },
};
