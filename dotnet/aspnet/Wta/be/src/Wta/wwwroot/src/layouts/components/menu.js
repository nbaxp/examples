import Icon from '@/components/icon/index.js';
import SvgIcon from '@/components/icon/index.js';
import { useAppStore, useTabsStore, useUserStore } from '@/store/index.js';
import html from 'utils';
import { computed, nextTick, reactive, ref, watch, watchEffect } from 'vue';
import { useRoute, useRouter } from 'vue-router';

export const HeadMenu = {
  components: { SvgIcon },
  template: html`
    <el-menu mode="horizontal" :ellipsis="false" :default-active="active" router>
      <template v-if="appStore.settings.showTopMenu">
        <template v-for="route in routes">
          <el-menu-item
            v-if="!route.meta?.hideInMenu"
            :key="route.meta.fullPath"
            :index="route.meta.fullPath"
            @click="onClick(route, $event)"
          >
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
  setup(props) {
    const appStore = useAppStore();
    const tabsStore = useTabsStore();
    const router = useRouter();
    const routes = computed(() => {
      const root = router.getRoutes().find((o) => o.name === 'root');
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
        window.open(props.node.path);
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
    };
  },
};

export const MenuItem = {
  name: 'menuItem',
  components: { SvgIcon },
  template: html`
    <template v-if="showItem(model)">
      <el-sub-menu :index="model.meta?.fullPath" v-if="model.children&&model.children.some(o=>!o.meta?.hideInMenu)">
        <template #title>
          <el-icon><svg-icon :name="model.meta?.icon??'folder'" /></el-icon>
          <span :title="model.meta?.fullPath">{{model.meta?.title??model.path}}</span>
        </template>
        <template v-for="item in model.children">
          <menu-item v-model="item" />
        </template>
      </el-sub-menu>
      <el-menu-item v-else :index="model.meta?.fullPath" @click.native="onClick(model, $event)">
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
    const userStore = useUserStore();
    watch(
      model,
      (value) => {
        context.emit('update:modelValue', value);
      },
      { deep: true },
    );
    //
    const onClick = (route, event) => {
      if (route.path.startsWith('http')) {
        event.preventDefault();
        window.open(props.node.path);
      }
    };
    //
    const showItem = (model) => {
      if (model.meta?.hideInMenu) {
        return false;
      }
      if (model.meta?.authorize) {
        return userStore.hasPermission(model.meta.authorize);
      }
      return true;
    };
    return {
      model,
      onClick,
      showItem,
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
      router
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
      list.value = appStore.settings.showTopMenu
        ? route.matched[1].children
        : router.getRoutes().find((o) => o.name === 'root').children;
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
