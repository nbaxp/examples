import App from '@/app.js';
import i18n from '@/locale/index.js';
import style from '@/mixins/style.js';
import router from '@/router/index.js';
import store from '@/store/index.js';
import * as ElementPlusIconsVue from '@element-plus/icons-vue';
import ElementPlus from 'element-plus';
import { createApp } from 'vue';
import useMock from '../mock/index.js';

//useMock();
const app = createApp(App);
app.mixin(style);
app.use(store);
app.use(i18n);
app.use(router);
app.use(ElementPlus);
for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
  app.component(`Ep${key}`, component);
}

app.mount('#app');
