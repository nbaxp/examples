import useLoginModel from '@/models/login.js';
import { useTokenStore, useUserStore } from '@/store/index.js';
import html, { schemaToModel } from 'utils';
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import AppForm from './components/form/index.js';
import LayoutFooter from './components/layout/footer.js';
import LayoutLocale from './components/layout/lang.js';
import LayoutLogo from './components/layout/logo.js';

export default {
  components: { AppForm, LayoutLogo, LayoutLocale, LayoutFooter },
  template: html`<el-container>
    <el-main class="flex items-center justify-center">
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
    model.value.userName = 'admin';
    model.value.password = '123456';
    const router = useRouter();
    const success = async (data) => {
      const tokenStore = useTokenStore();
      tokenStore.setToken(data.access_token, data.refresh_token);
      const redirect = router.currentRoute.value.query?.redirect ?? '/';
      router.push(redirect);
    };
    return {
      schema,
      model,
      success,
    };
  },
};
