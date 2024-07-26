import { createApp } from "vue";
import router from './router.js';
import mixin from "./mixin.js";
import App from './app.js';

const app = createApp(App);

app.mixin(mixin);

app.use(router);

app.mount("#app");
