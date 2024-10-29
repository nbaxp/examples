import AppForm from '@/components/form/index.js';
import LayoutFooter from '@/layouts/components/footer.js';
import LayoutLocale from '@/layouts/components/locale.js';
import LayoutLogo from '@/layouts/components/logo.js';
import { useTokenStore } from '@/store/index.js';
import request from '@/utils/request.js';
import { normalize, schemaToModel } from '@/utils/schema.js';
import html from 'utils';
import { onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';

export default {
  components: { AppForm, LayoutLogo, LayoutLocale, LayoutFooter },
  template: html`
    <el-container>
      <el-main class="flex items-center justify-center">
        <div style="min-width:333px;min-height:514px;">
          <div class="flex items-center justify-center pb-4">
            <layout-logo />
            <layout-locale />
          </div>
          <el-card class="box-card">
            <template v-if="model?.provider" #header>
              <el-alert type="warning" show-icon :closable="false">{{model?.provider}} {{$t('创建绑定账号')}}</el-alert>
            </template>
            <app-form v-if="schema" :schema="schema" v-model="model" @success="success" />
            <div style="display: flex; align-items: center; justify-content: space-between; height: 50px">
              <router-link style to="/login">{{ $t('登录') }}</router-link>
              <router-link style to="/forgot-password">{{ $t('忘记密码') }}</router-link>
            </div>
          </el-card>
          <layout-footer />
        </div>
      </el-main>
    </el-container>
  `,
  setup() {
    const schema = ref(null);
    const model = ref(null);
    const router = useRouter();
    const success = async (result) => {
      const data = result.data;
      if (data.isRedirect) {
      }
      else if (result.isRedirect) {
        window.location = result.data;
      }
      const tokenStore = useTokenStore();
      tokenStore.update(data.access_token, data.refresh_token);
      const redirect = router.currentRoute.value.query?.redirect ?? '/';
      router.push({ path: '/redirect', query: { redirect } });
    };
    onMounted(async () => {
      const result = await request('GET', 'user/register');
      schema.value = normalize(result.data.data);
      model.value = schemaToModel(schema.value);
      //
      schema.value.meta.hideReset = true;
      const params = new URLSearchParams(window.location.search);
      if (params.has('provider')) {
        model.value.provider = params.get('provider');
        model.value.open_id = params.get('open_id');
      }
    });
    return {
      schema,
      model,
      success,
    };
  },
};
