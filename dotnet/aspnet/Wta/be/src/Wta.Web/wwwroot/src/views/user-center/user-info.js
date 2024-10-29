import request from '@/utils/request.js';
import { normalize } from '@/utils/schema.js';
import AppForm from '@/components/form/index.js';
import { ElMessageBox } from 'element-plus';
import html from 'utils';
import { onMounted, ref } from 'vue';
import { useI18n } from 'vue-i18n';

export default {
  components: { AppForm },
  template: html`
    <div class="flex100">
      <el-card>
        <app-form v-if="model" v-model="model" :schema="schema" @success="success" />
      </el-card>
    </div>
  `,
  setup() {
    const schema = ref(null);
    const model = ref(null);
    const { t } = useI18n();
    const success = () => {
      ElMessageBox.alert(t('success'), t('tip'));
    };
    onMounted(async () => {
      const result = await request('GET', 'user-info/schema');
      schema.value = normalize(result.data.data);
      const result2 = await request('GET', 'user-info/index');
      model.value = result2.data.data;
    });
    return {
      schema,
      model,
      success,
    };
  },
};
