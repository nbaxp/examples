<template>
  <el-menu :collapse="appStore.isMenuCollapse" :collapse-transition="false" :default-active="$route.fullPath" router>
    <menu-item v-for="item in menus" :key="item.path" :node="item" parent="" />
  </el-menu>
</template>

<script setup>
  import { useRouter } from 'vue-router';

  import { useAppStore } from '../store/index.js';
  import MenuItem from './menu-item.vue';

  const appStore = useAppStore();
  const router = useRouter();
  const menus = router.getRoutes().find((o) => o.path === '/').children;
</script>
