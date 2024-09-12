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
        <div>
          <div class="flex items-center justify-center pb-4">
            <layout-logo />
            <layout-locale />
          </div>
          <el-card class="box-card">
            <app-form v-if="schema" :schema="schema" v-model="model" @success="success" />
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
      const tokenStore = useTokenStore();
      tokenStore.update(data.access_token, data.refresh_token);
      const redirect = router.currentRoute.value.query?.redirect ?? '/';
      router.push({ path: '/redirect', query: { redirect } });
    };
    onMounted(async () => {
      const result = await request('GET', 'token/create');
      schema.value = normalize(result.data.data);
      model.value = schemaToModel(schema.value);
      //
      schema.value.meta.hideReset = true;
      model.value.userName = 'admin';
      model.value.password = '123456';
    });
    return {
      schema,
      model,
      success,
    };
  },
};
