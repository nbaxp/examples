import AppForm from '@/components/form/index.js';
import settings from '@/config/settings.js';
import useSchema from '@/models/settings.js';
import useAppStore from '@/store/app.js';
import { useClipboard, useMediaQuery } from '@vueuse/core';
import { ElMessage } from 'element-plus';
import html from 'utils';
import { ref, watchEffect } from 'vue';

export default {
  components: { AppForm },
  template: html`
    <el-icon v-model="show" :size="18" class="cursor-pointer" @click="show=!show">
      <ep-setting />
    </el-icon>
    <el-drawer v-model="show" :title="$t('设置')" append-to-body destroy-on-close size="auto">
      <app-form ref="formRef" :schema="schema" v-model="appStore.settings" :hide-button="true" />
      <template #footer>
        <el-button type="primary" @click="copySettings">{{$t('复制')}}</el-button>
        <el-button type="primary" @click="reset">{{$t('重置')}}</el-button>
        <el-button type="primary" @click="save">{{$t('保存')}}</el-button>
      </template>
    </el-drawer>
  `,
  setup() {
    const show = ref(false);
    const formRef = ref(null);
    const appStore = useAppStore();
    const { copy } = useClipboard();
    const schema = ref(useSchema());
    const darkClass = 'dark';
    const copySettings = async () => {
      try {
        await formRef.value.validate();
        const text = JSON.stringify(appStore.settings, null, 2);
        await copy(text);
        ElMessage({
          message: 'config/settings.js',
          type: 'success',
        });
      } catch (error) {
        console.log(error);
      }
    };
    const reset = async () => {
      appStore.settings = { ...settings };
    };
    const save = () => {
      localStorage.setItem('settings', JSON.stringify(appStore.settings));
    };
    const toDark = () => document.documentElement.classList.add(darkClass);
    const toLight = () => document.documentElement.classList.remove(darkClass);
    watchEffect(() => {
      document.documentElement.classList[appStore.settings.useDarkNav ? 'add' : 'remove']('dark-nav');
    });
    watchEffect(() => {
      document.documentElement.style.setProperty('--el-color-primary', appStore.settings.color);
    });
    watchEffect(() => {
      const isDarkNow = useMediaQuery('(prefers-color-scheme: dark)');
      if (appStore.settings.mode === 'auto') {
        isDarkNow.value ? toDark() : toLight();
      } else if (appStore.settings.mode === 'dark') {
        toDark();
      } else if (appStore.settings.mode === 'light') {
        toLight();
      }
    });
    return {
      show,
      formRef,
      schema,
      appStore,
      copySettings,
      reset,
      save,
    };
  },
};
