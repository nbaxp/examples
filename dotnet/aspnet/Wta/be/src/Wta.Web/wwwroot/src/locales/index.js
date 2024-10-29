import { format } from 'utils';
import { createI18n } from 'vue-i18n';
import config from './config.js';

const i18n = createI18n(config);

//前端直接使用key作为message时的format参数化处理
const t = i18n.global.t;
i18n.global.t = (...args) => {
  let result = t(...args);
  const [template, ...params] = args;
  if (result === template && params?.length) {
    result = format(template, ...args[1]);
  }
  return result;
};

export default i18n;
