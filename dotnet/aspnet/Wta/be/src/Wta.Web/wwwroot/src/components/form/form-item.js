import AppFormInput from '@/components/form/form-input.js';
import { getRules } from '@/utils/validation.js';
import html from 'utils';
import { computed, reactive, watch } from 'vue';

export default {
  name: 'formItem',
  components: {
    AppFormInput,
  },
  template: html`
    <template v-if="!schema.meta?.hidden&&showItem()">
      <el-form-item
        :label="getLabel(prop)"
        :prop="getProp(prop)"
        :rules="rules"
        :error="mode==='query'?null:getError(prop)"
      >
        <app-form-input :schema="schema" :prop="prop" v-model="model" :mode="mode" :errors="errors" />
      </el-form-item>
    </template>
  `,
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
        return !props.schema.meta.hideForQuery;
      }
      if (props.mode !== 'details' && props.schema.meta.hideForEdit) {
        return false;
      }
      return true;
    };
    const getDisabled = () => {
      if (props.mode === 'details') {
        return true;
      }
      if (props.mode === 'update' && props.schema.meta.readOnly) {
        return true;
      }
      return false;
    };
    //
    const getProp = (prop) => {
      return prop;
    };
    //
    const getLabel = () => {
      if (props.parentSchema.meta?.labelWidth === 0) {
        return 0;
      }
      if (props.schema.meta?.showLabel) {
        return 0;
      }
      return props.schema.title;
    };
    //
    const getError = (prop) => {
      return props.errors[prop];
    };
    //
    const rules = computed(() => {
      if (props.mode === 'query' || props.mode === 'details') {
        return [];
      }
      return getRules(props.parentSchema, props.schema, model, props.prop);
    });
    /*end*/
    return {
      model,
      showItem,
      getProp,
      getLabel,
      getError,
      getDisabled,
      rules,
    };
  },
};
