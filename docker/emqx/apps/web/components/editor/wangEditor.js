import { ref, watch, watchEffect, onBeforeUnmount, shallowRef } from 'vue';
import { useI18n } from 'vue-i18n';
import html from 'utils';
import { Editor, Toolbar } from '@/lib/@wangEditor/editor-for-vue/index.esm.js';
import { i18nChangeLanguage } from '@/lib/@wangEditor/index.esm.js';
import request from '@/utils/request.js';

export default {
  components: { Editor, Toolbar },
  template: html` <div style="border: 1px solid #ccc;z-index:1000;">
    <Toolbar style="border-bottom: 1px solid #ccc" :editor="editorRef" :defaultConfig="toolbarConfig" :mode="mode" />
    <Editor
      style="height: 500px; overflow-y: hidden;"
      v-model="model"
      :defaultConfig="editorConfig"
      :mode="mode"
      @onCreated="handleCreated"
    />
  </div>`,
  styles: html`<link rel="stylesheet" href="./lib/@wangEditor/style.css" />`,
  props: {
    modelValue: {
      type: String,
      default: '',
    },
    mode: {
      type: String,
      default: 'default', //simple
    },
    uploadUrl: {
      type: String,
      default: null,
    },
  },
  setup(props, context) {
    const model = ref(props.modelValue);
    watch(model, (value) => context.emit('update:modelValue', value));

    const customUpload = async (file, insertFn) => {
      const method = 'POST';
      const url = 'file/upload';
      const data = new FormData();
      data.append('file', file);
      const result = await request(method, url, data);
      insertFn(result.data.data);
    };

    const editorRef = shallowRef();
    const toolbarConfig = {};
    const editorConfig = {
      placeholder: '...',
      MENU_CONF: {
        uploadImage: { customUpload },
        uploadVideo: { customUpload },
      },
    };

    onBeforeUnmount(() => {
      const editor = editorRef.value;
      if (editor == null) return;
      editor.destroy();
    });

    const handleCreated = (editor) => {
      editorRef.value = editor;
    };

    const i18n = useI18n();
    i18nChangeLanguage(i18n.locale.value);

    return { model, editorRef, toolbarConfig, editorConfig, handleCreated, i18n };
  },
};
