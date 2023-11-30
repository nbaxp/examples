<template>
  <el-container>
    <el-header><layout-header /></el-header>
    <el-container>
      <el-aside width="auto">
        <layout-menu />
      </el-aside>
      <el-container class="is-vertical main backtop">
        <layout-tabs v-if="appStore.useTabs" />
        <el-main>
          <layout-breadcrumb v-if="appStore.showBreadcrumb" />
          <div class="router-view" :style="style">
            <router-view v-if="!tabsStore.isRefreshing" v-slot="{ Component, route }">
              <component :is="Component" v-if="route.meta?.ignoreCache" :key="$route.fullPath" />
              <keep-alive :include="tabsStore.routes.map((o) => o.path)">
                <component :is="Component" v-if="!route.meta?.ignoreCache" :key="$route.fullPath" />
              </keep-alive>
            </router-view>
          </div>
          <el-footer v-if="appStore.showCopyright">
            <layout-footer />
          </el-footer>
        </el-main>
        <el-backtop target=".backtop > .el-main" />
      </el-container>
    </el-container>
  </el-container>
</template>

<script setup>
  import { computed } from 'vue';

  import { useAppStore, useTabsStore } from '../store/index.js';
  import LayoutBreadcrumb from './breadcrumb.vue';
  import LayoutFooter from './footer.vue';
  import LayoutHeader from './header.vue';
  import LayoutMenu from './menu.vue';
  import LayoutTabs from './tabs.vue';

  const appStore = useAppStore();
  const tabsStore = useTabsStore();

  const style = computed(() => {
    let height = 0;
    if (appStore.showBreadcrumb) {
      height += 40;
    }
    if (appStore.showCopyright) {
      height += 60;
    }
    const minHeight = height === 0 ? '' : `min-height:calc(100% - ${height}px);`;
    return minHeight;
  });
</script>
