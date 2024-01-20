<template><el-input v-model="model" /><el-button>发送</el-button></template>
<script setup>
  import { ref, watch } from 'vue';

  const props = defineProps(['modelValue', 'schema', 'prop', 'isReadOnly', 'mode']);
  const emit = defineEmits(['update:modelValue']);

  const model = ref(props.modelValue);
  watch(model, (value) => {
    emit('update:modelValue', value);
  });
</script>
