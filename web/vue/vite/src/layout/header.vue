<template>
  <div class="flex items-center justify-between">
    <div class="flex items-center justify-center">
      <layout-logo />
      <el-icon @click="toggleMenuCollapse">
        <i v-if="appStore.isMenuCollapse" class="i-uiw-menu-unfold" />
        <i v-else class="i-uiw-menu-fold" />
      </el-icon>
    </div>
    <div class="flex">
      <el-space>
        <el-icon @click="clickSearch">
          <i class="i-ep-search" />
        </el-icon>
        <el-select
          v-show="showSearch"
          ref="searchRef"
          v-model="searchModel"
          placeholder="search"
          filterable
          remote
          :remote-method="searchMenu"
          :loading="searchLoading"
          @blur="hideSearch"
        >
          <el-option
            v-for="item in searchOptions"
            :key="item.path"
            :value="item.path"
            :label="item.meta.title"
            @click="searchChange(item)"
          />
        </el-select>
        <el-icon @click="toggleFullscreen">
          <i v-if="isFullscreen" class="i-dashicons-fullscreen-exit-alt" />
          <i v-else class="i-dashicons-fullscreen-alt" />
        </el-icon>
        <el-dropdown v-if="tokenStore.accessToken">
          <span class="el-dropdown-link flex">
            <el-avatar v-if="userStore.avatar" :size="18" class="el-icon--left" :src="'./assets/icons/avatar.svg'" />
            <el-icon v-else class="el-icon--left">
              <i class="i-ep-user" />
            </el-icon>
            {{ userStore.userName }}
            <el-icon class="el-icon--right">
              <i class="i-ep-arrow-down" />
            </el-icon>
          </span>
          <template #dropdown>
            <el-dropdown-menu>
              <el-dropdown-item>
                <router-link to="/account">
                  <el-icon> <i class="i-ep-user" /> </el-icon>{{ $t('userCenter') }}
                </router-link>
              </el-dropdown-item>
              <el-dropdown-item divided @click="confirmLogout">
                <el-icon> <i class="i-ep-switch-button" /> </el-icon>{{ $t('logout') }}
              </el-dropdown-item>
            </el-dropdown-menu>
          </template>
        </el-dropdown>
        <el-link v-else type="info">
          <router-link to="/register"> {{ $t('register') }}</router-link>
        </el-link>
        <layout-locale />
        <layout-settings />
      </el-space>
    </div>
  </div>
</template>

<script setup>
  import { useFullscreen } from '@vueuse/core';
  import { ElMessage, ElMessageBox } from 'element-plus';
  import { ref } from 'vue';
  import { useI18n } from 'vue-i18n';
  import { useRouter } from 'vue-router';

  import { useAppStore, useTokenStore, useUserStore } from '@/store/index.js';

  import LayoutLocale from './locale.vue';
  import LayoutLogo from './logo.vue';
  import LayoutSettings from './settings.vue';

  const router = useRouter();
  const i18n = useI18n();
  const appStore = useAppStore();
  const tokenStore = useTokenStore();
  const userStore = useUserStore();
  //
  const searchRef = ref(null);
  const searchLoading = ref(false);
  const searchModel = ref('');
  const searchOptions = ref([]);
  const showSearch = ref(false);
  const hideSearch = () => {
    showSearch.value = false;
  };
  const clickSearch = () => {
    showSearch.value = !showSearch.value;
    if (showSearch.value) {
      searchRef.value.focus();
    }
  };
  const searchMenu = (query) => {
    if (query) {
      try {
        searchLoading.value = true;
        searchOptions.value = router
          .getRoutes()
          .filter((o) => !o.meta?.hideInMenu && !o.children?.length && o.meta?.title.indexOf(query) >= 0);
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
  //
  const toggleMenuCollapse = () => {
    appStore.isMenuCollapse = !appStore.isMenuCollapse;
  };
  //
  const { isFullscreen, toggle: toggleFullscreen } = useFullscreen(document.documentElement);
  const confirmLogout = async () => {
    try {
      await ElMessageBox.confirm(i18n.t('confirmLogout'), i18n.t('tip'), { type: 'warning' });
      await tokenStore.logout();
    } catch (error) {
      if (error === 'cancel') {
        ElMessage({
          type: 'info',
          message: i18n.t('cancel'),
        });
      }
    }
  };
</script>
