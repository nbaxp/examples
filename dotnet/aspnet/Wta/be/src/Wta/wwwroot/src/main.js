import App from '@/app.js';
import style from '@/mixins/style.js';
import router from '@/router/index.js';
import store from '@/store/index.js';
import ElementPlus from 'element-plus';
import { delay } from 'utils';
import { createApp } from 'vue';
import * as ElementPlusIconsVue from '~/lib/element-plus/icons-vue/index.min.js';
//import useMock from '~/mock/index.js';
import i18n from '~/src/locales/index.js';

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
await delay(0);
app.mount('#app');
