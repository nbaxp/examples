import { useAppStore, useTabsStore } from '@/store/index.js';
import Icon from '@/views/components/icon/index.js';
import SvgIcon from '@/views/components/icon/index.js';
import { ElMessageBox } from 'element-plus';
import html from 'utils';
import { computed, nextTick, reactive, ref, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';

export const HeadMenu = {
  components: { SvgIcon },
  template: html`<el-menu
    mode="horizontal"
    :default-active="$route.matched[1].path"
    :ellipsis="false"
    router
  >
    <template v-for="route in routes">
      <el-menu-item
        v-if="!route.meta?.hideInMenu"
        :key="route.path"
        :index="route.path"
        @click="onClick(route, $event)"
      >
        <template #title>
          <svg-icon v-if="route.meta?.icon" :name="route.meta.icon" />
          <span :title="route.path"
            >{{ $t(route.meta?.title ?? route.path) }}</span
          >
        </template>
      </el-menu-item>
    </template>
  </el-menu>`,
  setup() {
    const tabsStore = useTabsStore();
    const router = useRouter();
    const routes = computed(() =>
      router
        .getRoutes()
        .find((o) => o.name === 'root')
        .children//.filter((o) => o.path !== '/')
        .sort((a, b) => a.meta?.order > b.meta?.order),
    );
    const onClick = (route, event) => {
      if (route.path.startsWith('http')) {
        window.open(props.node.path);
      } else {
        const path = tabsStore.routes.findLast((o) => o.path === route.path)?.path ?? route.path;
        router.push(path);
      }
    };
    return {
      routes,
      onClick,
    };
  },
};

export const MenuItem = {
  name: 'menuItem',
  components: { SvgIcon },
  template: html`<template v-if="model&&!model.meta?.hideInMenu">
    <el-sub-menu
      :index="model.meta?.fullPath"
      v-if="model.children&&model.children.some(o=>!o.meta?.hideInMenu)"
    >
      <template #title>
        <el-icon><svg-icon :name="model.meta?.icon??'folder'" /></el-icon>
        <span :title="model.meta?.fullPath"
          >{{model.meta?.title??model.path}}</span
        >
      </template>
      <template v-for="item in model.children">
        <menu-item v-model="item" />
      </template>
    </el-sub-menu>
    <el-menu-item
      v-else
      :index="model.meta?.fullPath"
      @click.native="onClick(model, $event)"
    >
      <el-icon><svg-icon :name="model.meta?.icon??'file'" /></el-icon>
      <template #title>
        <span :title="model.meta?.fullPath"
          >{{model.meta?.title??model.path}}</span
        >
      </template>
    </el-menu-item>
  </template>`,
  props: {
    modelValue: {
      typeof: Object,
    },
  },
  setup(props, context) {
    const appStore = useAppStore();
    const tabsStore = useTabsStore();
    const model = reactive(props.modelValue);
    watch(
      model,
      (value) => {
        context.emit('update:modelValue', value);
      },
      { deep: true },
    );
    //
    const onClick = (route, event) => {
      if (!route.path.startsWith('http')) {
        if (appStore.settings.useTabs && tabsStore.routes.length >= (appStore.settings.maxTabs ?? 10)) {
          ElMessageBox.alert(`页签达到最大限制${appStore.settings.maxTabs ?? 10},请关闭不再使用的页签`, `提示`);
          event.preventDefault();
        }
      } else {
        event.preventDefault();
        window.open(props.node.path);
      }
    };
    //
    return {
      model,
      onClick,
    };
  },
};

export default {
  components: { Icon, MenuItem },
  template: html`<el-menu
    :collapse="appStore.settings.isMenuCollapse"
    :collapse-transition="false"
    :default-active="$route.path"
    router
    v-if="show"
  >
    <template v-for="item in list">
        <menu-item v-model="item" />
    </template>
  </el-menu>`,
  setup() {
    const appStore = useAppStore();
    const route = useRoute();
    const list = ref([]);
    const show = ref(false);
    watch(
      () => route.path,
      () => {
        list.value = route.matched[1].children;
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
      show,
    };
  },
};
