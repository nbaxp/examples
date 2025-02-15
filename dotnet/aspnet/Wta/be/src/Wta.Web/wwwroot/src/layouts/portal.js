import { useAppStore, useTabsStore } from '@/store/index.js';
import html from 'utils';
import { computed } from 'vue';
import { useRoute } from 'vue-router';
import LayoutBreadcrumb from './components/breadcrumb.js';
import LayoutFooter from './components/footer.js';
import LayoutHeader from './components/header.js';
import LayoutMenu from './components/menu.js';
import LayoutTabs from './components/tabs.js';

export default {
  components: {
    LayoutHeader,
    LayoutMenu,
    LayoutTabs,
    LayoutBreadcrumb,
    LayoutFooter,
  },
  template: html`
    <el-container class="home">
      <el-header style="display:flex;flex:1;position:sticky;overflow:visible"><layout-header /></el-header>
      <el-container class="flex100 flex-direction-row">
        <el-aside v-if="!isHomePage" width="auto" class="flex1" :class="{open:!appStore.settings.isMenuCollapse}">
          <layout-menu />
        </el-aside>
        <el-container class="is-vertical main backtop flex100 flex-dir-col">
          <el-main class="flex100 flex-dir-col">
            <div class="router-view flex100">
              <layout-breadcrumb v-if="!isHomePage&&appStore.settings.showBreadcrumb" />
              <div class="w-full h-full">
                <router-view v-if="!tabsStore.isRefreshing" />
              </div>
            </div>
            <el-footer v-if="appStore.settings.showCopyright" class="flex1 h-full overflow-visible align-center">
              <layout-footer />
            </el-footer>
          </el-main>
          <el-backtop target=".backtop > .el-main" />
        </el-container>
      </el-container>
    </el-container>
  `,
  setup() {
    const appStore = useAppStore();
    const tabsStore = useTabsStore();
    const route = useRoute();
    const cacheList = computed(() => {
      return tabsStore.routes.filter((o) => !o.meta?.noCache).map((o) => o.name);
    });
    const isHomePage = computed(() => route.matched[1].children.length === 0);
    return {
      appStore,
      tabsStore,
      cacheList,
      isHomePage,
    };
  },
};
