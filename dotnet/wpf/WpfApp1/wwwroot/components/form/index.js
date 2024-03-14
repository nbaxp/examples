import html from "html";
import { defineAsyncComponent, ref, reactive, watch, onMounted } from "vue";

export default {
  components: {
    AppFormItem: defineAsyncComponent(() => import("./form-item.js")),
  },
  name: "AppForm",
  template: html`<div v-loading="loading">
    <el-form ref="formRef" :model="model" label-width="auto" :inline="inline" @keyup.enter.native="submit">
      <template v-if="schema && schema.properties">
        <template v-for="(value, prop) in getProperties(schema.properties)">
          <app-form-item :parentSchema="schema" :schema="value" v-model="model" :prop="prop" :mode="mode" :errors="errors" />
        </template>
      </template>
      <slot></slot>
      <el-form-item v-if="!hideButton">
        <template #label></template>
        <el-button type="primary" @click="submit" :disabled="loading">
          <slot name="submitText">$t('confirm')</slot>
        </el-button>
      </el-form-item>
    </el-form>
  </div>`,
  props: ["modelValue", "inline", "schema", "action", "hideButton", "isQueryForm", "mode"],
  emits: ["update:modelValue", "submit"],
  setup(props, context) {
    // init
    const model = reactive(props.modelValue);
    watch(model, (value) => {
      context.emit("update:modelValue", value);
    });
    // ref
    const formRef = ref(null);
    const loading = ref(false);
    //
    const errors = ref({});
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
          context.emit(
            "submit",
            (serverErrors) => {
              if (serverErrors) {
                errors.value = serverErrors;
              }
            },
            loading
          );
        }
      } catch (error) {
        console.log(error);
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
      reset,
      submit,
    };
  },
};
