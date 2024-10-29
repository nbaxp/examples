import en from './en-US.js';
import zh from './zh-CN.js';

export const DEFAULT_LANGUAGE = 'zh-CN';

export default {
  legacy: false,
  locale: DEFAULT_LANGUAGE,
  fallbackLocale: DEFAULT_LANGUAGE,
  options: [
    {
      key: 'zh-CN',
      value: '中文（中国）',
    },
    {
      key: 'en-US',
      value: 'English (United States)',
    },
  ],
  messages: {
    'zh-CN': zh,
    'en-US': en,
  },
};
