import useSchema from '@/models/forgot-password.js';
import AppForm from '@/views/components/form/index.js';
import LayoutFooter from '@/views/layout/footer.js';
import LayoutLocale from '@/views/layout/lang.js';
import LayoutLogo from '@/views/layout/logo.js';
import html, { schemaToModel } from 'utils';
import { ref } from 'vue';
import { useRouter } from 'vue-router';

export default {
  components: { AppForm, LayoutFooter, LayoutLocale, LayoutLogo },
  template: html`
    <el-container>
      <el-main style="display: flex; align-items: center; justify-content: center">
        <div class="login">
          <div class="flex items-center justify-center pb-4">
            <el-space><layout-logo /> <layout-locale /></el-space>
          </div>
          <el-card class="box-card">
            <el-row :gutter="40">
              <el-col>
                <app-form v-model="model" :schema="schema" @success="success" />
                <div style="display: flex; align-items: center; justify-content: space-between; height: 50px">
                  <router-link style to="/login">{{ $t('登录') }}</router-link>
                  <router-link style to="/register">{{ $t('注册') }}</router-link>
                </div>
              </el-col>
            </el-row>
          </el-card>
          <layout-footer />
        </div>
      </el-main>
    </el-container>
  `,
  setup() {
    const schema = useSchema();
    const model = ref(schemaToModel(schema));
    const router = useRouter();
    const success = (result) => {
      router.push('/login');
    };
    return {
      schema,
      model,
      success,
    };
  },
};