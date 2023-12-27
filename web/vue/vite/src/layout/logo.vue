<template>
  <a href="javascript:;" class="logo" @click="reload">
    <div class="flex items-center">
      <img src="../assets/logo.svg" />
      <h1 v-if="!appStore.settings.isMenuCollapse">{{ $t('application') }}</h1>
    </div>
  </a>
</template>
<script setup>
  import { reload } from '@/utils/index.js';

  import { useAppStore } from '../store/index.js';

  const appStore = useAppStore();
</script>
