import { ref, watch, nextTick } from "vue";
import { useI18n } from "vue-i18n";
import html from "utils";
import WangEditor from "./wangEditor.js";

export default {
  components: { WangEditor },
  template: html`<wang-editor v-if="show" v-model="model" :mode="mode" />`,
  props: {
    modelValue: {
      type: String,
      default: "",
    },
    mode: {
      type: String,
      default: "default", //simple
    },
  },
  setup(props, context) {
    const model = ref(props.modelValue);
    watch(model, (value) => context.emit("update:modelValue", value));

    const show = ref(true);
    const i18n = useI18n();

    watch(i18n.locale, () => {
      show.value = false;
      nextTick(() => {
        show.value = true;
      });
    });

    return { model, show };
  },
};
