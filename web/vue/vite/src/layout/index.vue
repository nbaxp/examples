<template>
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
              <component :is="Component" v-if="route.meta?.ignoreCache" :key="$route.fullPath" />
              <keep-alive :include="tabsStore.routes.map((o) => o.path)">
                <component :is="Component" v-if="!route.meta?.ignoreCache" :key="$route.fullPath" />
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
</template>

<script setup>
  import { useAppStore, useTabsStore } from '../store/index.js';
  import LayoutBreadcrumb from './breadcrumb.vue';
  import LayoutFooter from './footer.vue';
  import LayoutHeader from './header.vue';
  import LayoutMenu from './menu.vue';
  import LayoutTabs from './tabs.vue';

  const appStore = useAppStore();
  const tabsStore = useTabsStore();
</script>
