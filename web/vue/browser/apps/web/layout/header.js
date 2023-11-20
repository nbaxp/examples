import html from "utils";
import { ref } from "vue";
import { useAppStore, useTokenStore, useUserStore } from "~/store/index.js";
import { useDark, useFullscreen, useToggle } from "@vueuse/core";
import { ElMessage, ElMessageBox } from "element-plus";
import { useI18n } from "vue-i18n";
import { useRouter } from "vue-router";
import { treeToList } from "~/utils/index.js";
import SvgIcon from "~/components/icon/index.js";
import LayoutLogo from "./logo.js";
import LayoutLocale from "./locale.js";
import LayoutSettings from "./settings.js";

export default {
  components: {
    SvgIcon,
    LayoutLogo,
    LayoutLocale,
    LayoutSettings,
    ElMessage,
    ElMessageBox,
  },
  template: html`
    <div class="flex items-center justify-between">
      <div class="flex items-center justify-center">
        <layout-logo />
        <el-icon @click="toggleMenuCollapse" class="cursor-pointer" :size="18">
          <svg-icon name="unfold" v-if="appStore.isMenuCollapse" />
          <svg-icon name="fold" v-else />
        </el-icon>
      </div>
      <div class="flex">
        <el-space>
          <el-icon class="cursor-pointer" @click="clickSearch">
            <ep-search />
          </el-icon>
          <el-select
            ref="searchRef"
            placeholder="search"
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
              :key="item.meta.path"
              :value="item.meta.path"
              :label="item.meta.fullName"
              @click="searchChange(item)"
            />
          </el-select>
          <el-icon v-model="isDark" @click="toggleDark()" :size="18" class="cursor-pointer">
            <ep-sunny v-if="isDark" />
            <ep-moon v-else />
          </el-icon>
          <el-icon @click="toggleFullscreen" :size="18" class="cursor-pointer">
            <svg-icon name="fullscreen-exit" v-if="isFullscreen" />
            <svg-icon name="fullscreen" v-else />
          </el-icon>
          <el-dropdown class="cursor-pointer" v-if="tokenStore.accessToken">
            <span class="el-dropdown-link flex">
              <el-avatar v-if="userStore.avatar" class="el-icon--left" :size="18" :src="'./assets/icons/avatar.svg'" />
              <el-icon v-else class="el-icon--left" :size="18">
                <ep-user />
              </el-icon>
              {{ userStore.userName }}
              <el-icon class="el-icon--right">
                <ep-arrow-down />
              </el-icon>
            </span>
            <template #dropdown>
              <el-dropdown-menu>
                <el-dropdown-item>
                  <router-link to="/account">
                    <el-icon> <ep-user /> </el-icon>{{$t('userCenter')}}
                  </router-link>
                </el-dropdown-item>
                <el-dropdown-item divided @click="confirmLogout">
                  <el-icon> <ep-switch-button /> </el-icon>{{$t('logout')}}
                </el-dropdown-item>
              </el-dropdown-menu>
            </template>
          </el-dropdown>
          <el-link type="info" v-else>
            <router-link to="/register"> {{$t('register')}}</router-link>
          </el-link>

          <layout-locale />
          <layout-settings />
        </el-space>
      </div>
    </div>
  `,
  setup() {
    const router = useRouter();
    const i18n = useI18n();
    const appStore = useAppStore();
    const tokenStore = useTokenStore();
    const userStore = useUserStore();
    //
    const searchRef = ref(null);
    const searchLoading = ref(false);
    const searchModel = ref("");
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
          const menus = treeToList(router.getRoutes().find((o) => o.path === "/").children);
          searchOptions.value = menus
            .filter((o) => !o.children || o.children.length === 0)
            .filter((o) => o.meta.fullName.indexOf(query) > -1);
        } finally {
          searchLoading.value = false;
        }
      }
    };
    const searchChange = (route) => {
      if (!route.meta?.isExternal) {
        router.push(route.meta.path);
        searchModel.value = "";
        searchOptions.value = [];
        showSearch.value = false;
      } else {
        window.open(route.path);
      }
    };
    //
    const isDark = useDark();
    const toggleDark = useToggle(isDark);
    const toggleMenuCollapse = () => (appStore.isMenuCollapse = !appStore.isMenuCollapse);
    //
    const { isFullscreen, toggle: toggleFullscreen } = useFullscreen(document.documentElement);
    const confirmLogout = async () => {
      try {
        await ElMessageBox.confirm(i18n.t("confirmLogout"), i18n.t("tip"), { type: "warning" });
        await tokenStore.logout();
      } catch (error) {
        if (error === "cancel") {
          ElMessage({
            type: "info",
            message: i18n.t("cancel"),
          });
        }
      }
    };
    return {
      appStore,
      tokenStore,
      userStore,
      showSearch,
      hideSearch,
      clickSearch,
      searchRef,
      searchLoading,
      searchModel,
      searchOptions,
      searchMenu,
      searchChange,
      isDark,
      toggleDark,
      toggleMenuCollapse,
      isFullscreen,
      toggleFullscreen,
      confirmLogout,
    };
  },
};
