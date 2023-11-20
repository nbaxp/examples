import Mock from "../lib/better-mock/mock.browser.esm.js";
import config from "../locale/config.js";

export default function () {
  Mock.mock("/api/locale", "POST", (request) => {
    //服务端返回JSON格式对象
    return JSON.parse(JSON.stringify(config));
  });
}
