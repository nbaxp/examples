import Mock from "../lib/better-mock/mock.browser.esm.js";
import routes from "../router/routes.js";

export default function () {
  Mock.mock("/api/menu", "POST", (request) => {
    return JSON.parse(JSON.stringify(routes));
  });
}
