import { getRules } from '@/utils/validation.js';
import AppFormInput from '@/views/components/form/form-input.js';
import html from 'utils';
import { reactive, watch } from 'vue';

export default {
  name: 'formItem',
  components: {
    AppFormInput,
  },
  template: html`<template v-if="!schema.meta.hidden&&showItem()">
    <template v-if="schema.type==='object'"></template>
    <template v-else-if="schema.type!=='array'||schema.items?.type!=='array'">
      <el-form-item
        :title="parentSchema.labelWidth===0?null:schema.title"
        :label="parentSchema.labelWidth===0?null:schema.title"
        :prop="getProp(prop)"
        :rules="getDisabled()?[]:getRules(parentSchema,schema,model,getProp(prop))"
        :error="mode==='query'?null:getError(prop)"
      >
        <app-form-input
          :schema="schema"
          :prop="prop"
          v-model="model"
          :mode="mode"
          :errors="errors"
        />
      </el-form-item>
    </template>
  </template>`,
  props: ['modelValue', 'mode', 'parentSchema', 'schema', 'prop', 'errors'],
  emit: ['update:modelValue'],
  setup(props, context) {
    const model = reactive(props.modelValue);
    watch(model, (value) => {
      context.emit('update:modelValue', value);
    });
    /*start*/
    const showItem = () => {
      if (props.mode === 'query') {
        return !props.schema.hideForQuery;
      }
      if (props.schema.hideForEdit) {
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
    /*end*/
    return {
      model,
      showItem,
      getProp,
      getError,
      getDisabled,
      getRules,
    };
  },
};
