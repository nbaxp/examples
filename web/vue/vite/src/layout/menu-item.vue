<template>
  <template v-if="!node.meta?.hideInMenu">
    <el-sub-menu v-if="node.children && node.children.some((o) => !o.meta?.hideInMenu)" :index="path">
      <template #title>
        <el-icon><svg-icon :name="node.meta.icon ?? 'folder'" /></el-icon>
        <span>{{ node.meta?.title }}</span>
      </template>
      <template v-for="item in node.children" :key="item.path">
        <menu-item :node="item" :parent="parent + '/' + node.path" />
      </template>
    </el-sub-menu>
    <el-menu-item v-else :index="node.meta?.isExternal ? null : path" @click="click">
      <el-icon><svg-icon :name="node.meta.icon ?? 'file'" /></el-icon>
      <template #title>
        <span>{{ node.meta?.title }}</span>
      </template>
    </el-menu-item>
  </template>
</template>

<script setup>
  import { ElMessageBox } from 'element-plus';
  import { useRouter } from 'vue-router';

  import SvgIcon from '@/components/icon/index.vue';
  import { useAppStore, useTabsStore } from '@/store/index.js';

  const props = defineProps({
    node: {
      type: Object,
      default: null,
    },
    parent: {
      type: String,
      default: null,
    },
  });

  const appStore = useAppStore();
  const tabsStore = useTabsStore();
  const router = useRouter();

  const path = `${props.parent}/${props.node.path}`;
  const click = (route) => {
    if (!route.meta?.isExternal) {
      if (appStore.useTabs && tabsStore.routes.length >= (appStore.maxTabs ?? 10)) {
        ElMessageBox.alert(`页签达到最大限制${appStore.maxTabs ?? 10},请关闭不再使用的页签`, `提示`);
      } else {
        router.push(path);
      }
    } else {
      window.open(props.node.path);
    }
  };
</script>
