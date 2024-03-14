import html from "html";
import { defineAsyncComponent, reactive, watch } from "vue";
import { useRouter } from "vue-router";
import { useAppStore } from "../store/index.js";
import { ElMessageBox } from "element-plus";

export default {
  name: "menuItem",
  components: { SvgIcon: defineAsyncComponent(() => import("../components/icon/index.js")) },
  template: html`<template v-if="!modelValue.meta.isHidden">
    <el-sub-menu
      :index="modelValue.meta.path"
      v-if="modelValue.children&&modelValue.children.some(o=>!o.meta.isHidden)"
    >
      <template #title>
        <el-icon><svg-icon :name="modelValue.meta.icon??'folder'" /></el-icon>
        <span>{{modelValue.meta.title}}</span>
      </template>
      <menu-item v-for="item in modelValue.children" v-model="item" />
    </el-sub-menu>
    <el-menu-item
      v-else-if="modelValue.meta.type==='page'"
      :index="modelValue.meta.isExternal?null:modelValue.meta.path"
      @click.native="click(modelValue)"
    >
      <el-icon><svg-icon :name="modelValue.meta.icon??file" /></el-icon>
      <template #title>
        <span>{{modelValue.meta.title}}</span>
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
    const router = useRouter();
    const model = reactive(props.modelValue);
    watch(
      model,
      (value) => {
        context.emit("update:modelValue", value);
      },
      { deep: true }
    );
    //
    const click = (route) => {
      if (!route.meta.isExternal) {
        // if (appStore.routes.length >= 10) {
        //   ElMessageBox.alert(`已经页签数量`, `提示`);
        // } else {
        //   router.push(route.meta.path);
        // }
        router.push(route.meta.path);
      } else {
        window.open(route.path);
      }
    };
    //
    return {
      model,
      click,
    };
  },
};
