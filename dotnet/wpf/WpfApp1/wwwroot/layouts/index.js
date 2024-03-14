import html from "html";
import { useAppStore } from "../store/index.js";
import { defineAsyncComponent, computed } from "vue";

export default {
  components: {
    LayoutHeader: defineAsyncComponent(() => import("./header.js")),
    LayoutMenu: defineAsyncComponent(() => import("./menu.js")),
    LayoutTabs: defineAsyncComponent(() => import("./tabs.js")),
    LayoutFooter: defineAsyncComponent(() => import("./footer.js")),
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
            <router-view v-if="!isRefreshing" v-slot="{ Component, route }">
              <component :is="Component" v-if="route.meta?.disableCaching" :key="$route.fullPath" />
              <keep-alive :include="appStore.routes.map(o=>o.path)">
                <component :is="Component" v-if="!route.meta?.disableCaching" :key="route.fullPath" />
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
    const isRefreshing = computed(() => appStore.isRefreshing);
    const path = computed(() => useRoute().matched[0].path);
    const items = computed(() => useRoute().matched[0].children);
    return {
      appStore,
      isRefreshing,
      path,
      items,
    };
  },
};
