import request from '@/utils/request.js';
import { ElMessage, ElMessageBox } from 'element-plus';
import html from 'utils';
import { nextTick, reactive, ref, watch } from 'vue';
import AppFormItem from './form-item.js';

export default {
  components: {
    AppFormItem,
  },
  name: 'AppForm',
  template: html`<div v-loading="loading">
    <el-form
      ref="formRef"
      :model="model"
      label-width="auto"
      :inline="inline"
      @keyup.enter.native="submit"
    >
      <el-form-item
        v-if="errorMessage"
        :label-width="0"
        style="margin-bottom:0;"
      >
        <el-text type="danger">{{errorMessage}}</el-text>
      </el-form-item>
      <template v-if="schema && schema.properties">
        <template v-for="(value, prop) in getProperties(schema.properties)">
          <app-form-item
            :parentSchema="schema"
            :schema="value"
            v-model="model"
            :prop="prop"
            :mode="mode"
            :errors="errors"
          />
        </template>
      </template>
      <slot></slot>
      <el-form-item
        v-if="!hideButton"
        :label-width="0"
        style="margin-bottom:0;"
      >
        <slot name="submit">
          <el-button
            type="primary"
            @click="submit"
            :disabled="loading"
            :style="schema.submitStyle"
          >
            {{$t(schema.title??'confirm')}}
          </el-button>
          <el-button v-if="showReset" @click="reset" :disabled="loading">
            {{$t('reset')}}
          </el-button>
        </slot>
      </el-form-item>
    </el-form>
  </div>`,
  props: ['modelValue', 'inline', 'schema', 'action', 'hideButton', 'showReset', 'isQueryForm', 'mode'],
  emits: ['update:modelValue', 'success', 'error'],
  setup(props, context) {
    // init
    const model = reactive(props.modelValue);
    watch(model, (value) => {
      context.emit('update:modelValue', value);
    });
    // ref
    const formRef = ref(null);
    const loading = ref(false);
    //
    const errors = ref({});
    const errorMessage = ref(null);
    // reset
    const reset = () => {
      formRef.value.resetFields();
    };
    // validate
    const validate = async () => {
      return formRef.value.validate();
    };
    // submit
    const submit = async () => {
      try {
        const valid = await validate();
        if (valid) {
          loading.value = true;
          const url = props.schema.meta.url;
          const method = props.schema.meta.method;
          errorMessage.value = null;
          const result = await request(url, model, method);
          if (!result.error) {
            context.emit('success', result.data);
          } else {
            //errorMessage.value = result.message;
            if (result.data instanceof Object && !Array.isArray(result.data)) {
              errors.value = {};
              nextTick(() => {
                errors.value = result.data;
                //Object.assign(errors.value, result.data);
              });
            } else {
              await ElMessageBox.alert(result.message, '提示', {
                type: 'warning',
              });
            }
            context.emit('error', result.data);
          }
        }
      } catch (error) {
        console.log(error);
      } finally {
        loading.value = false;
      }
    };
    const getProperties = (properties) => {
      return Object.fromEntries(Object.entries(properties).sort(([, a], [, b]) => a.order - b.order));
    };
    context.expose({ validate, reset });
    return {
      model,
      formRef,
      loading,
      getProperties,
      errors,
      errorMessage,
      reset,
      submit,
    };
  },
};
