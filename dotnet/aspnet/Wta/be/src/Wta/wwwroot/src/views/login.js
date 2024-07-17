import useLoginModel from '@/models/login.js';
import { useTokenStore, useUserStore } from '@/store/index.js';
import html, { schemaToModel } from 'utils';
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import AppForm from './components/form/index.js';
import LayoutFooter from './layout/footer.js';
import LayoutLocale from './layout/lang.js';
import LayoutLogo from './layout/logo.js';

export default {
  components: { AppForm, LayoutLogo, LayoutLocale, LayoutFooter },
  template: html`<el-container>
    <el-main class="flex items-center justify-center">
      <div>
        <div class="flex items-center justify-center pb-4">
          <layout-logo />
          <layout-locale />
        </div>
        <el-card class="box-card">
          <app-form :schema="schema" v-model="model" @success="success" />
          <div style="display: flex; align-items: center; justify-content: space-between; height: 50px">
            <router-link style to="/register">{{ $t('注册') }}</router-link>
            <router-link style to="/forgot-password">{{ $t('忘记密码') }}</router-link>
          </div>
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
    const success = async (result) => {
      const data = result.data;
      const tokenStore = useTokenStore();
      tokenStore.update(data.access_token, data.refresh_token);
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
