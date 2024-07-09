import html from 'utils';
import { computed } from 'vue';
import { useAppStore, useTabsStore } from '../store/index.js';
import LayoutBreadcrumb from './breadcrumb.js';
import LayoutFooter from './footer.js';
import LayoutHeader from './header.js';
import LayoutMenu from './menu.js';
import LayoutTabs from './tabs.js';

const template = html`
<el-container>
    <el-header><layout-header /></el-header>
    <el-container>
        <el-aside width="auto">
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
                <el-backtop target=".backtop > .el-main" />
            </el-main>
        </el-container>
    </el-container>
</el-container>
`;

export default {
  components: {
    LayoutHeader,
    LayoutMenu,
    LayoutTabs,
    LayoutBreadcrumb,
    LayoutFooter,
  },
  template,
  setup() {
    const appStore = useAppStore();
    const tabsStore = useTabsStore();
    const include = computed(() => tabsStore.routes.map((o) => o.fullPath));
    return { appStore, include };
  },
};
