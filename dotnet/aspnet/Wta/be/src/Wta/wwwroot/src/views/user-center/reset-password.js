import useSchema from '@/models/reset-password.js';
import AppForm from '@/views/components/form/index.js';
import { ElMessageBox } from 'element-plus';
import html from 'utils';
import { schemaToModel } from 'utils';
import { ref } from 'vue';
import { useI18n } from 'vue-i18n';

export default {
  components: { AppForm },
  template: html`
    <el-card>
      <app-form ref="formRef" v-model="model" :schema="schema" @success="success" />
    </el-card>
  `,
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
