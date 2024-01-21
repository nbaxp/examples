<template>
  <div style="display: inline-flex">
    <el-input v-model="model" />
    <el-image
      :title="$t('clickRefresh')"
      :src="src"
      @click="onClick"
      style="cursor: pointer; max-height: 30px; margin-left: 10px"
    />
  </div>
</template>
<script setup>
  import { ref, watch, onMounted } from 'vue';
  import request from '@/utils/request.js';

  const props = defineProps({
    modelValue: {
      type: String,
      default: '',
    },
    method: {
      type: String,
      default: 'POST',
    },
    url: {
      type: String,
      default: 'captcha/image',
    },
    code: {
      type: String,
      default: 'code',
    },
    codeHash: {
      type: String,
      default: 'codeHash',
    },
  });
  const emit = defineEmits(['update:modelValue', 'callback']);

  const model = ref(props.modelValue);
  watch(model, (value) => {
    emit('update:modelValue', value);
  });

  const src = ref('');
  const load = async () => {
    const result = await request(props.method, props.url);
    src.value = result.data[props.code];
    emit('callback', result.data[props.codeHash]);
  };

  const onClick = async () => {
    await load();
  };

  onMounted(async () => {
    await load();
  });
</script>
