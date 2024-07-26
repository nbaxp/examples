import zh from "./zh-CN.js";
import en from "./en-US.js";

export default {
  legacy: false,
  fallbackLocale: "en-US",
  options: [
    {
      value: "zh-CN",
      label: "中文",
    },
    {
      value: "en-US",
      label: "English",
    },
  ],
  messages: {
    "zh-CN": zh,
    "en-US": en,
  },
};
