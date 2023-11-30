<template>
  <el-icon style="margin-right: 5px">
    <component :is="svg" />
  </el-icon>
</template>

<script setup>
  import { computed, defineAsyncComponent } from 'vue';

  import { log } from '@/utils/index.js';

  const props = defineProps({
    name: {
      type: String,
      default: '',
    },
  });
  log(`../../assets/icons/${props.name}.svg`);
  const components = import.meta.glob('../../assets/icons/**/*.svg');
  const svg = computed(() => {
    return defineAsyncComponent(
      components[`../../assets/icons/${props.name}.svg`] ?? components['../../assets/icons/file.svg'],
    );
  });
</script>
