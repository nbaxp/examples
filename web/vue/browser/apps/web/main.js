import { createApp } from 'vue';
import ElementPlus from 'element-plus';
import * as ElementPlusIconsVue from '@element-plus/icons-vue';
import style from '@/mixins/style.js';
import store from '@/store/index.js';
import useLocale from '@/locale/index.js';
import useRouter from '@/router/index.js';
import App from '@/app.js';
import useMock from '@/mock/index.js';

useMock();
const app = createApp(App);
app.mixin(style);
app.use(store);
app.use(await useLocale());
app.use(await useRouter());
app.use(ElementPlus);
for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
  app.component(`Ep${key}`, component);
}
app.mount('#app');
