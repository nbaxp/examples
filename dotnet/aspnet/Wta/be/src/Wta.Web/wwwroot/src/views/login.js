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
import SvgIcon from '@/components/icon/index.js';

export default {
  components: { SvgIcon, AppForm, LayoutLogo, LayoutLocale, LayoutFooter },
  template: html`
    <el-container class="login">
      <el-main class="flex items-center justify-center">
        <div style="min-width:300px;min-height:500px;">
          <div class="flex items-center justify-center pb-4">
            <layout-logo />
          </div>
          <el-card class="box-card">
            <template v-if="model?.client_id" #header>
              <el-alert type="warning" show-icon :closable="false">
                {{client}} {{$t('请求登录')}}
              </el-alert>
            </template>
            <app-form
              v-if="schema"
              :schema="schema"
              v-model="model"
              @success="success"
            />
            <div
              v-if="false"
              style="display: flex; align-items: center; justify-content: space-between; height: 50px"
            >
              <router-link style to="/register">{{ $t('注册') }}</router-link>
              <router-link style to="/forgot-password">
                {{ $t('忘记密码') }}
              </router-link>
            </div>
            <template v-if="!model?.client_id&&providers.length">
              <el-divider>{{$t("其他登录方式")}}</el-divider>
              <el-space style="display:flex;justify-content: center;">
                <el-icon
                  style="font-size:28px;"
                  v-for="item in providers"
                  @click="redirect(item.number)"
                  class="cursor-pointer"
                  :title="item.name"
                >
                  <img
                    :alt="item.name"
                    style="max-width:28px;max-height:28px;"
                    :src="item.icon"
                  />
                </el-icon>
              </el-space>
            </template>
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
    const client = ref('');
    const providers = ref([]);
    const isLogin = ref(false);
    const tokenStore = useTokenStore();
    const success = async (result) => {
      const data = result.data;
      if (result.isRedirect) {
        window.location = result.data;
      } else {
        tokenStore.update(data.access_token, data.refresh_token);
        const redirect = router.currentRoute.value.query?.redirect ?? '/';
        router.push({ path: '/redirect', query: { redirect } });
      }
    };
    const redirect = async (provider) => {
      const result = await request('GET', 'oauth/external-login', { provider });
      window.location = result.data.data;
    };
    onMounted(async () => {
      const result = await request('GET', 'token/create');
      schema.value = normalize(result.data.data);
      model.value = schemaToModel(schema.value);
      //
      schema.value.meta.hideReset = true;
      const params = new URLSearchParams(window.location.search);
      model.value.tenantNunmber = params.get('tenant');
      if (params.has('client_id')) {
        model.value.client_id = params.get('client_id');
        model.value.return_to = params.get('return_to');
        model.value.anti_token = params.get('anti_token');
        client.value = new URL(
          new URL(model.value.return_to).searchParams.get('redirect_uri'),
        ).origin;
      } else {
        const result = await request('GET', 'oauth/providers');
        providers.value = result.data.data;
      }
      model.value.userName = 'admin';
      model.value.password = '123456';
      isLogin.value = await tokenStore.isLogin();
      if (isLogin.value) {
        const url =
          params.get('return_to') + '&access_token=' + tokenStore.accessToken;
        window.location = url;
      }
    });
    return {
      schema,
      model,
      success,
      providers,
      redirect,
      client,
    };
  },
};
