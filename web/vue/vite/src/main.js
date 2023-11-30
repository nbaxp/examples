import 'element-plus/theme-chalk/dark/css-vars.css';
import 'element-plus/es/components/message/style/css';
import '@/styles/site.css';
import 'virtual:uno.css';

import { createApp } from 'vue';

import App from '@/app.vue';
import useLocale from '@/locale/index.js';
import useMock from '@/mock/index.js';
import useRouter from '@/router/index.js';
import store from '@/store/index.js';

useMock();
const app = createApp(App);
app.use(store);
app.use(await useLocale());
app.use(await useRouter());
app.mount('#app');
