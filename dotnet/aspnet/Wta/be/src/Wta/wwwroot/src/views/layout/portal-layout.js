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
  template: html`
<el-container class="portal-layout backtop">
  <el-header class="flex justify-center">
    <div class="container xl"><layout-header /></div>
  </el-header>
  <el-main class="flex justify-center" style="padding-top:60px">
    <div class="container xl">
      <router-view></router-view>
      <el-backtop target=".backtop > .el-main" />
    </div>
  </el-main>
  <el-footer class="flex justify-center" v-if="appStore.settings.showCopyright">
    <div class="container xl"><layout-footer /></div>
  </el-footer>
</el-container>
`,
  setup() {
    const appStore = useAppStore();
    const tabsStore = useTabsStore();
    const path = computed(() => useRoute().matched[0].path);
    const items = computed(() => useRoute().matched[0].children);
    const include = computed(() => tabsStore.routes.filter((o) => o.name));
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
