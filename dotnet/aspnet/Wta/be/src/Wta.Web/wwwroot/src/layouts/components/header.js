import SvgIcon from '@/components/icon/index.js';
import { useAppStore, useTabsStore, useTokenStore, useUserStore } from '@/store/index.js';
import { useFullscreen } from '@vueuse/core';
import { ElMessage, ElMessageBox } from 'element-plus';
import html from 'utils';
import { nextTick, ref } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRouter } from 'vue-router';
import LayoutTheme from './theme.js';
import LayoutLocale from './locale.js';
import LayoutLogo from './logo.js';
import { HeadMenu } from './menu.js';
import LayoutSettings from './settings.js';

export default {
  components: {
    SvgIcon,
    LayoutLogo,
    LayoutTheme,
    LayoutLocale,
    LayoutSettings,
    ElMessage,
    ElMessageBox,
    HeadMenu,
  },
  template: html`
    <layout-logo />
    <el-icon
      v-if="!isPortal"
      @click="toggleMenuCollapse"
      class="collapse-button cursor-pointer mr-5"
      :size="18"
      style="flex:1;"
    >
      <svg-icon name="unfold" v-if="appStore.settings.isMenuCollapse" />
      <svg-icon name="fold" v-else />
    </el-icon>
    <head-menu />
    <el-space :size="appStore.settings.size">
      <el-icon v-if="!isPortal" class="cursor-pointer" @click="clickSearch" :title="$t('点击搜索')">
        <ep-search />
      </el-icon>
      <el-select
        class="search"
        ref="searchRef"
        :placeholder="$t('搜索')"
        v-show="showSearch"
        @blur="hideSearch"
        filterable
        remote
        :remote-method="searchMenu"
        v-model="searchModel"
        :loading="searchLoading"
      >
        <el-option
          v-for="item in searchOptions"
          :key="item.path"
          :value="item.path"
          :label="item.meta.title"
          @click="searchChange(item)"
        />
      </el-select>
      <layout-theme />
      <el-icon @click="toggleFullscreen" :size="18" class="cursor-pointer">
        <svg-icon name="fullscreen-exit" v-if="isFullscreen" />
        <svg-icon name="fullscreen" v-else />
      </el-icon>
      <layout-locale />
      <el-dropdown class="cursor-pointer" v-if="tokenStore.accessToken">
        <span class="el-dropdown-link flex">
          <el-avatar v-if="userStore.avatar" class="el-icon--left" :size="18" :src="'./src/assets/icons/avatar.svg'" />
          <el-icon v-else class="el-icon--left" :size="18">
            <ep-user />
          </el-icon>
          {{ tokenStore.name }}
          <el-icon class="el-icon--right">
            <ep-arrow-down />
          </el-icon>
        </span>
        <template #dropdown>
          <el-dropdown-menu>
            <el-dropdown-item>
              <router-link to="/user-center">
                <el-icon><ep-user /></el-icon>
                {{$t('我的')}}
              </router-link>
            </el-dropdown-item>
            <el-dropdown-item divided @click="confirmLogout">
              <el-icon><ep-switch-button /></el-icon>
              {{$t('退出')}}
            </el-dropdown-item>
          </el-dropdown-menu>
        </template>
      </el-dropdown>
      <template v-else>
        <el-link type="info">
          <router-link to="/register">{{$t('注册')}}</router-link>
        </el-link>
        <el-link type="info">
          <router-link to="/login">{{$t('登录')}}</router-link>
        </el-link>
      </template>
      <layout-settings v-if="!isPortal" />
    </el-space>
  `,
  setup() {
    const router = useRouter();
    const i18n = useI18n();
    const appStore = useAppStore();
    const tokenStore = useTokenStore();
    const tabsStore = useTabsStore();
    const userStore = useUserStore();
    //
    const searchRef = ref(null);
    const searchLoading = ref(false);
    const searchModel = ref('');
    const searchOptions = ref([]);
    const showSearch = ref(false);
    const isPortal = ref(router.currentRoute.value.matched[0].name === 'portal');

    const hideSearch = () => {
      showSearch.value = false;
    };
    const clickSearch = () => {
      showSearch.value = !showSearch.value;
      if (showSearch.value) {
        nextTick(() => {
          searchRef.value.focus();
        });
      }
    };
    const searchMenu = (query) => {
      if (query) {
        try {
          searchLoading.value = true;
          searchOptions.value = router
            .getRoutes()
            .filter(
              (o) =>
                o.meta?.fullPath && !o.meta?.hideInMenu && !o.children?.length && o.meta?.title.indexOf(query) >= 0,
            );
        } finally {
          searchLoading.value = false;
        }
      }
    };
    const searchChange = (route) => {
      if (!route.meta?.isExternal) {
        router.push(route.path);
        searchModel.value = '';
        searchOptions.value = [];
        showSearch.value = false;
      } else {
        window.open(route.path);
      }
    };
    const toggleMenuCollapse = () => {
      appStore.settings.isMenuCollapse = !appStore.settings.isMenuCollapse;
    };
    //
    const { isFullscreen, toggle: toggleFullscreen } = useFullscreen(document.documentElement);
    const confirmLogout = async () => {
      try {
        await ElMessageBox.confirm(i18n.t('确认退出'), i18n.t('提示'), {
          type: 'warning',
        });
        await userStore.logout();
        await tokenStore.clear();
        tabsStore.clear();
        //router.push({ path: 'login', query: { redirect: router.currentRoute.value.fullPath } });
        router.push(`/logout?redirect=${router.currentRoute.value.fullPath}`);
      } catch (error) {
        if (error === 'cancel') {
          ElMessage({
            type: 'info',
            message: i18n.t('cancel'),
          });
        }
      }
    };
    const refresh = async () => {
      tabsStore.isRefreshing = true;
      router.currentRoute.value.meta?.cache?.clear();
      nextTick(() => {
        tabsStore.isRefreshing = false;
      });
    };
    return {
      appStore,
      tokenStore,
      userStore,
      tabsStore,
      showSearch,
      hideSearch,
      clickSearch,
      searchRef,
      searchLoading,
      searchModel,
      searchOptions,
      searchMenu,
      searchChange,
      toggleMenuCollapse,
      isFullscreen,
      toggleFullscreen,
      confirmLogout,
      refresh,
      isPortal,
    };
  },
};
