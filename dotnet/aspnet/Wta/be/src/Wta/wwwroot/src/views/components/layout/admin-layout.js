import { useAppStore, useTabsStore } from '@/store/index.js';
import html from 'utils';
import { computed } from 'vue';
import LayoutBreadcrumb from './breadcrumb.js';
import LayoutFooter from './footer.js';
import LayoutHeader from './header.js';
import LayoutMenu from './menu.js';
import LayoutTabs from './tabs.js';

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
      <el-aside width="auto" :class="{open:!appStore.settings.isMenuCollapse}">
        <layout-menu />
      </el-aside>
      <el-container class="is-vertical main backtop">
        <layout-tabs v-if="appStore.settings.useTabs" />
        <el-main>
          <layout-breadcrumb v-if="appStore.settings.showBreadcrumb" />
          <div class="router-view" :class="$route.path" style="flex: 1; overflow: auto">
            <router-view v-if="!tabsStore.isRefreshing" v-slot="{ Component, route }">
              <component
                :is="Component"
                v-if="!appStore.settings.useTabs || route.meta?.noCache"
                :key="route.fullPath"
              />
              <keep-alive :max="appStore.settings.maxTabs" :include="include">
                <component :is="Component" :key="route.fullPath" />
              </keep-alive>
            </router-view>
          </div>
          <el-footer v-if="appStore.settings.showCopyright">
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
    const include = computed(() => tabsStore.routes.map((o) => o.fullPath));
    const style = computed(() => {
      let height = 0;
      if (appStore.settings.showBreadcrumb) {
        height += 40;
      }
      if (appStore.settings.showCopyright) {
        height += 60;
      }
      const minHeight = height === 0 ? '' : `min-height:calc(100% - ${height}px);`;
      return minHeight;
    });

    return {
      appStore,
      tabsStore,
      include,
      path,
      items,
      style,
    };
  },
};
