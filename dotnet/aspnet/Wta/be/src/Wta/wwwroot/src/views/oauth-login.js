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
    <div class="el-loading-mask">
      <div class="el-loading-spinner">
        <svg class="circular" viewBox="0 0 50 50">
          <circle class="path" cx="25" cy="25" r="20" fill="none"></circle>
        </svg>
        <p class="el-loading-text">Loading...</p>
      </div>
    </div>
  `,
  setup() {
    const router = useRouter();
    const params = new URLSearchParams(window.location.search);
    const tokenStore = useTokenStore();
    tokenStore.update(params.get('access_token'), params.get('access_token'));
    router.push('/');
  },
};
