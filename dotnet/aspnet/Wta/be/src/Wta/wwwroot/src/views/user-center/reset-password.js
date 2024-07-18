import useSchema from '@/models/user-center/reset-password.js';
import { schemaToModel } from '@/utils/schema.js';
import AppForm from '@/views/components/form/index.js';
import { ElMessageBox } from 'element-plus';
import html from 'utils';
import { ref } from 'vue';
import { useI18n } from 'vue-i18n';

export default {
  components: { AppForm },
  template: html`<div class="flex100">
  <el-card>
    <app-form ref="formRef" v-model="model" :schema="schema" @success="success" />
  </el-card>
</div>`,
  setup() {
    const formRef = ref(null);
    const schema = ref(useSchema());
    const model = ref(schemaToModel(schema.value));
    const { t } = useI18n();
    const success = (result) => {
      formRef.value.reset();
      ElMessageBox.alert(t('success'), t('tip'));
    };
    return {
      formRef,
      schema,
      model,
      success,
    };
  },
};
