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
import { M } from '~/node_modules/vite/dist/node/types.d-aGj9QkWt';

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
            <template #header>
              <el-alert type="warning" show-icon :closable="false">
                {{model?.provider}} {{$t('第三方账号绑定')}}
              </el-alert>
            </template>
            <el-tabs>
              <el-tab-pane :label="$t('登录绑定')">
                <app-form
                  v-if="schema1"
                  :schema="schema1"
                  v-model="model1"
                  @success="success"
                />
              </el-tab-pane>
              <el-tab-pane :label="$t('注册绑定')">
                <app-form
                  v-if="schema2"
                  :schema="schema2"
                  v-model="model2"
                  @success="success"
                />
              </el-tab-pane>
            </el-tabs>
          </el-card>
          <layout-footer />
        </div>
      </el-main>
    </el-container>
  `,
  setup() {
    const schema1 = ref(null);
    const model1 = ref(null);
    const schema2 = ref(null);
    const model2 = ref(null);
    const router = useRouter();
    const tokenStore = useTokenStore();
    const isLogin = ref(false);
    const success = async (result) => {
      const data = result.data;
      tokenStore.update(data.access_token, data.refresh_token);
      const redirect = router.currentRoute.value.query?.redirect ?? '/';
      router.push({ path: '/redirect', query: { redirect } });
    };
    onMounted(async () => {
      isLogin.value = await tokenStore.isLogin();
      if (isLogin.value) {
        router.push('/user-center/user-info');
      } else {
        const params = new URLSearchParams(window.location.search);
        const result1 = await request('GET', 'token/create');
        schema1.value = normalize(result1.data.data);
        schema1.value.meta.hideReset = true;
        model1.value = schemaToModel(schema1.value);
        model1.value.provider = params.get('provider');
        model1.value.open_id = params.get('open_id');
        const result2 = await request('GET', 'user/register');
        schema2.value = normalize(result2.data.data);
        schema2.value.meta.hideReset = true;
        model2.value = schemaToModel(schema2.value);
        model2.value.provider = params.get('provider');
        model2.value.open_id = params.get('open_id');
      }
    });
    return {
      schema1,
      model1,
      schema2,
      model2,
      success,
      isLogin,
    };
  },
};
