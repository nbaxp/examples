import useSchema from "@/models/reset-password.js";
import AppForm from "@/views/components/form/index.js";
import { ElMessageBox } from "element-plus";
import html from "utils";
import { schemaToModel } from "utils";
import { ref } from "vue";
import { useI18n } from "vue-i18n";

export default {
  components: { AppForm },
  template: html`
    <el-card>
      <app-form v-model="model" :schema="schema" @success="success" />
    </el-card>
  `,
  setup() {
    const schema = ref(useSchema());
    const model = ref(schemaToModel(schema.value));
    const { t } = useI18n();
    const success = (result) => {
      ElMessageBox.alert(t("success"), t("tip"));
    };
    return {
      schema,
      model,
      success,
    };
  },
};
