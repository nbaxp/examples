<template>
  <el-config-provider :locale="options[$i18n.locale]" :size="appStore.settings.size">
    <router-view></router-view>
  </el-config-provider>
</template>

<script setup>
  import en from 'element-plus/dist/locale/en.mjs';
  import zh from 'element-plus/dist/locale/zh-cn.mjs';
  import { ref, provide } from 'vue';

  import { useAppStore } from '@/store/index.js';

  provide('routeData', new Map());
  const options = ref({
    'zh-CN': zh,
    'en-US': en,
  });
  const appStore = useAppStore();
</script>
