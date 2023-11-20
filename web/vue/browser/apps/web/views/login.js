import html, { schemaToModel } from "utils";
import { ref } from "vue";
import AppForm from "../components/form/index.js";
import { useRouter } from "vue-router";
import LayoutLogo from "../layout/logo.js";
import LayoutLocale from "../layout/locale.js";
import LayoutFooter from "../layout/footer.js";
import { useTokenStore, useUserStore } from "../store/index.js";
import useLoginModel from "../models/login.js";

export default {
  components: { AppForm, LayoutLogo, LayoutLocale, LayoutFooter },
  template: html`<el-container>
    <el-main class="flex justify-center">
      <div>
        <div class="flex items-center justify-center">
          <layout-logo />
          <layout-locale />
        </div>
        <el-card class="box-card">
          <app-form :schema="schema" v-model="model" @success="success" />
        </el-card>
        <layout-footer />
      </div>
    </el-main>
  </el-container>`,
  setup() {
    const schema = ref(useLoginModel());
    const model = ref(schemaToModel(schema.value));
    model.value.userName = "admin";
    model.value.password = "123456";
    const router = useRouter();
    const success = async (data) => {
      const tokenStore = useTokenStore();
      tokenStore.setToken(data.access_token, data.refresh_token);
      const userStore = useUserStore();
      await userStore.getUserInfo();
      const redirect = router.currentRoute.value.query?.redirect ?? "/";
      router.push(redirect);
    };
    return {
      schema,
      model,
      success,
    };
  },
};
