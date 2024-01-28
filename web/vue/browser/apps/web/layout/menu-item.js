import html from 'utils';
import { reactive, watch } from 'vue';
import { useRouter } from 'vue-router';
import { useAppStore, useTabsStore } from '@/store/index.js';
import { ElMessageBox } from 'element-plus';
import SvgIcon from '@/components/icon/index.js';

export default {
  name: 'menuItem',
  components: { SvgIcon },
  template: html`<template v-if="!modelValue.meta?.hideInMenu">
    <el-sub-menu :index="path" v-if="modelValue.children&&modelValue.children.some(o=>!o.meta?.hideInMenu)">
      <template #title>
        <el-icon><svg-icon :name="modelValue.meta.icon??'folder'" /></el-icon>
        <span>{{modelValue.meta.title}}</span>
      </template>
      <template v-for="item in modelValue.children">
        <menu-item v-model="item" :parent="parent +'/'+modelValue.path" />
      </template>
    </el-sub-menu>
    <el-menu-item v-else :index="modelValue.meta?.isExternal?null:path" @click.native="click">
      <el-icon><svg-icon :name="modelValue.meta.icon??'file'" /></el-icon>
      <template #title>
        <span>{{modelValue.meta.title}}</span>
      </template>
    </el-menu-item>
  </template>`,
  props: {
    modelValue: {
      typeof: Object,
    },
    parent: {
      typeof: String,
      default: null,
    },
  },
  setup(props, context) {
    const appStore = useAppStore();
    const tabsStore = useTabsStore();
    const router = useRouter();
    const model = reactive(props.modelValue);
    watch(
      model,
      (value) => {
        context.emit('update:modelValue', value);
      },
      { deep: true }
    );
    //
    const path = `${props.parent}/${props.modelValue.path}`;
    const click = (route) => {
      if (!route.meta?.isExternal) {
        if (appStore.useTabs && tabsStore.routes.length >= (appStore.maxTabs ?? 10)) {
          ElMessageBox.alert(`页签达到最大限制${appStore.maxTabs ?? 10},请关闭不再使用的页签`, `提示`);
        } else {
          router.push(path);
        }
      } else {
        window.open(props.modelValue.path);
      }
    };
    //
    return {
      model,
      path,
      click,
    };
  },
};
