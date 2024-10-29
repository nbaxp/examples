import Icon from '@/components/icon/index.js';
import SvgIcon from '@/components/icon/index.js';
import { useAppStore, useTabsStore, useUserStore } from '@/store/index.js';
import html from 'utils';
import { computed, nextTick, reactive, ref, watch, watchEffect } from 'vue';
import { useRoute, useRouter } from 'vue-router';

const show = (menu) => {
  if (menu.meta.hidden) {
    return false;
  }
  if (!menu.meta?.authType) {
    return true;
  } else {
    return !menu.meta?.hidden && useUserStore().hasPermission(menu.meta);
  }
};

export const HeadMenu = {
  components: { SvgIcon },
  template: html`
    <el-menu mode="horizontal" ellipsis :default-active="active" router>
      <template v-if="appStore.settings.showTopMenu">
        <template v-for="route in routes">
          <el-menu-item v-if="show(route)" :index="route.meta?.fullPath" @click="onClick(route)">
            <template #title>
              <el-icon v-if="route.meta?.icon">
                <svg-icon :name="route.meta.icon" />
              </el-icon>
              <span :title="route.meta.fullPath">{{route.meta.title }}</span>
            </template>
          </el-menu-item>
        </template>
      </template>
    </el-menu>
  `,
  setup() {
    const appStore = useAppStore();
    const tabsStore = useTabsStore();
    const router = useRouter();
    const routes = computed(() => {
      const name = router.currentRoute.value.matched[0].name;
      const root = router.getRoutes().find((o) => o.name === name);
      const result =
        router.currentRoute.value.matched[1].path === '/'
          ? root.children.find((o) => o.path === '/').children
          : root.children.filter((o) => o.path !== '/');
      return result.sort((a, b) => a.meta?.order > b.meta?.order);
    });
    const active = computed(() => {
      return router.currentRoute.value.matched[1].meta.fullPath;
    });
    const onClick = (route) => {
      if (route.path.startsWith('http')) {
        window.open(route.path);
      } else {
        const path = tabsStore.routes.findLast((o) => o.matched[1].path === route.path)?.path ?? route.path;
        router.push(path);
      }
    };
    return {
      appStore,
      routes,
      active,
      onClick,
      show,
    };
  },
};

export const MenuItem = {
  name: 'menuItem',
  components: { SvgIcon },
  template: html`
    <template v-if="show(model)">
      <el-sub-menu :index="model.meta?.fullPath" v-if="model.children&&model.children.some(o=>!o.meta?.hideInMenu)">
        <template #title>
          <el-icon><svg-icon :name="model.meta?.icon??'folder'" /></el-icon>
          <span :title="model.meta?.fullPath">{{model.meta?.title??model.path}}</span>
        </template>
        <template v-for="item in model.children">
          <menu-item v-model="item" />
        </template>
      </el-sub-menu>
      <el-menu-item v-else :index="model.meta?.fullPath" @click="onClick(model,$event)">
        <el-icon><svg-icon :name="model.meta?.icon??'file'" /></el-icon>
        <template #title>
          <span :title="model.meta?.fullPath">{{model.meta?.title??model.path}}</span>
        </template>
      </el-menu-item>
    </template>
  `,
  props: {
    modelValue: {
      typeof: Object,
    },
  },
  setup(props, context) {
    const model = reactive(props.modelValue);
    watch(
      model,
      (value) => {
        context.emit('update:modelValue', value);
      },
      { deep: true },
    );
    //
    const router = useRouter();
    const onClick = (route) => {
      if (route.path.startsWith('http')) {
        window.open(route.path);
      } else {
        router.push(route.meta.fullPath);
      }
    };
    return {
      model,
      onClick,
      show,
    };
  },
};

export default {
  components: { Icon, MenuItem },
  template: html`
    <el-menu
      :collapse="appStore.settings.isMenuCollapse"
      :collapse-transition="false"
      :default-active="active"
      v-if="show"
    >
      <template v-for="item in list">
        <menu-item v-model="item" />
      </template>
    </el-menu>
  `,
  setup() {
    const appStore = useAppStore();
    const route = useRoute();
    const router = useRouter();
    const list = ref([]);
    const show = ref(false);
    const active = ref(null);
    watchEffect(() => {
      active.value = route.path;
      const name = router.currentRoute.value.matched[0].name;
      list.value = appStore.settings.showTopMenu
        ? route.matched[1].children
        : router.getRoutes().find((o) => o.name === name).children;
    });
    watch(
      list,
      () => {
        show.value = false;
        nextTick(() => {
          show.value = true;
        });
      },
      { immediate: true },
    );
    return {
      appStore,
      list,
      active,
      show,
    };
  },
};
