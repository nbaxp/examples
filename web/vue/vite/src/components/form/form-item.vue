<template>
  <template v-if="!schema.hidden && showItem()">
    <template v-if="schema.type === 'object'"></template>
    <template v-else-if="schema.type !== 'array' || schema.items?.type !== 'array'">
      <el-form-item
        :title="prop"
        :label="parentSchema.labelWidth === 0 ? null : $t(schema.title ?? prop)"
        :prop="getProp(prop)"
        :rules="getDisabled() ? [] : getRules(parentSchema, schema, model)"
        :error="mode === 'query' ? null : getError(prop)"
      >
        <app-form-input :schema="schema" :prop="prop" v-model="model" :mode="mode" />
      </el-form-item>
    </template>
  </template>
</template>

<script setup>
  import { reactive, watch } from 'vue';

  import AppFormInput from '@/components/form/form-input.vue';
  import { getRules } from '@/utils/validation.js';

  const props = defineProps(['modelValue', 'mode', 'parentSchema', 'schema', 'prop', 'errors']);
  const emit = defineEmits(['update:modelValue']);

  const model = reactive(props.modelValue);
  watch(model, (value) => {
    emit('update:modelValue', value);
  });
  /* start */
  const showItem = () => {
    if (props.schema.hidden) {
      return false;
    }
    if (props.schema.hidden && (props.mode === 'create' || props.mode === 'update')) {
      return false;
    }
    return true;
  };
  const getDisabled = () => {
    if (props.mode === 'details') {
      return true;
    }
    if (props.mode === 'update' && props.schema.readOnly) {
      return true;
    }
    return false;
  };
  //
  const getProp = (prop) => {
    return prop;
  };
  //
  const getError = (prop) => {
    return props.errors[prop];
  };
</script>
