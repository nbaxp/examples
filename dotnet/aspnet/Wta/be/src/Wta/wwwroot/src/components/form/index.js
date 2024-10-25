import request from '@/utils/request.js';
import { ElMessageBox } from 'element-plus';
import html from 'utils';
import { nextTick, reactive, ref, watch } from 'vue';
import AppFormItem from './form-item.js';
import SvgIcon from '@/components/icon/index.js';
import { schemaToModel } from '@/utils/schema.js';

export default {
  components: {
    SvgIcon,
    AppFormItem,
  },
  name: 'AppForm',
  template: html`
    <el-form
      v-loading="loading"
      ref="formRef"
      :model="model"
      label-width="auto"
      :inline="inline"
      :class="mode"
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
          <!--对象属性-->
          <template v-if="value.type==='object'"></template>
          <!--对象列表-->
          <template v-else-if="value.meta?.items?.type==='object'">
            <el-form-item :label="value.title">
              <el-icon
                v-if="mode!=='query'&&model[prop]?.length===0"
                class="cursor-pointer"
              >
                <svg-icon
                  name="ep-plus"
                  @click="addItem(prop,value.meta.items)"
                />
              </el-icon>
              <div
                class="el-form-content-2col pb-2"
                v-for="(item,index) in model[prop]"
              >
                <app-form
                  inline
                  :hideButton="true"
                  :disabled="mode==='details'"
                  :mode="mode"
                  :schema="value.meta.items"
                  v-model="model[prop][index]"
                />
                <el-icon>
                  <svg-icon
                    name="ep-plus"
                    @click="addItem(prop,value.meta.items)"
                    class="cursor-pointer"
                  />
                </el-icon>
                <el-icon>
                  <svg-icon
                    name="ep-close"
                    @click="removeItem(model[prop],index)"
                    class="cursor-pointer"
                  />
                </el-icon>
                <br />
              </div>
            </el-form-item>
          </template>
          <!--简单属性||简单属性列表-->
          <app-form-item
            v-else
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
        :label="schema.meta?.submitStyle?'':' '"
      >
        <slot name="submit">
          <el-button
            type="primary"
            @click="submit"
            :disabled="loading"
            :style="schema.meta?.submitStyle"
          >
            {{$t('确定')}}
          </el-button>
          <el-button
            v-if="!schema.meta.hideReset"
            @click="reset"
            :disabled="loading"
          >
            {{$t('重置')}}
          </el-button>
        </slot>
      </el-form-item>
    </el-form>
  `,
  props: [
    'modelValue',
    'inline',
    'schema',
    'action',
    'hideButton',
    'showReset',
    'mode',
  ],
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
          const result = await request(method, url, model);
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
      return Object.fromEntries(
        Object.entries(properties).sort(([, a], [, b]) => a.order - b.order),
      );
    };
    const setErrors = (modelErrors) => {
      errors.value = {};
      nextTick(() => {
        errors.value = modelErrors;
        //Object.assign(errors.value, result.data);
      });
    };
    context.expose({ reset, validate, submit, setErrors });
    const addItem = (prop, schema) => {
      model[prop].push(schemaToModel(schema));
    };
    const removeItem = (items, index) => {
      items.splice(index, 1);
    };
    return {
      model,
      formRef,
      loading,
      getProperties,
      errors,
      errorMessage,
      reset,
      submit,
      addItem,
      removeItem,
    };
  },
};
