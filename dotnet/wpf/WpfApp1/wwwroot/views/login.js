import html, { schemaToModel } from "html";
import { reactive } from "vue";
import AppForm from "../components/form/index.js";
import { getUser, setAccessToken } from "../api/user.js";
import router, { refreshRouter } from "../router/index.js";
import request from "../request/index.js";
import LayoutLogo from "../layouts/logo.js";
import LayoutLocale from "../layouts/locale.js";
import LayoutFooter from "../layouts/footer.js";
import { useAppStore } from "../store/index.js";
import useLoginModel from "../models/login.js";
import { ElMessage } from "element-plus";
import { useI18n } from "vue-i18n";

export default {
  components: { AppForm, LayoutLogo, LayoutLocale, LayoutFooter },
  template: html`<el-container>
    <el-main class="flex justify-center">
      <div>
        <div class="flex items-center justify-center">
          <layout-logo />
          <layout-locale />
        </div>
        <el-card class="box-card" style="width:400px;">
          <app-form :schema="schema" v-model="model" @submit="submit">
            <template #submitText>{{$t('login')}}</template>
          </app-form>
        </el-card>
        <layout-footer />
      </div>
    </el-main>
  </el-container>`,
  setup() {
    const schema = reactive(useLoginModel());
    const model = reactive(schemaToModel(schema));
    const { t } = useI18n();
    const submit = async (callback, loading) => {
      try {
        const url = "base/token";
        const appStore = useAppStore();
        const result = await request(url, model, { method: "POST" });
        if (!result.errors) {
          if (result.data?.accessToken) {
            appStore.token = result.data.accessToken;
            if (appStore.token) {
              setAccessToken(appStore.token);
              //setRefreshToken(result.data.refresh_token);
              appStore.user = await getUser();
              await refreshRouter();
              const redirect = router.currentRoute.value.query?.redirect ?? "/";
              router.push(redirect);
            }
          } else {
            ElMessage.error(t(result.data.errorDescription));
          }
        }
        callback(result.errors);
      } catch (error) {
        callback(error);
      } finally {
        loading.value = false;
      }
    };
    return {
      schema,
      model,
      submit,
    };
  },
};
