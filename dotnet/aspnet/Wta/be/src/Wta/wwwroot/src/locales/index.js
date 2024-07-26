import { format } from 'utils';
import { createI18n } from 'vue-i18n';
import config from './config.js';

const i18n = createI18n(config);

// const t = i18n.global.t;

// i18n.global.t = (...args) => {
//   if (i18n.global.locale.value === 'zh-CN') {
//     return format(args.shift(), args);
//   }
//   return t(...args);
// };

export default i18n;
