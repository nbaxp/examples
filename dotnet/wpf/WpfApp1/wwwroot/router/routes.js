import baseData from "./base-data.js";
import business from "./business.js";

export default [
  {
    path: "home",
    meta: {
      type: "page",
      title: "首页",
      icon: "home",
      public: true,
    },
  },
  ...baseData,
  ...business,
];
