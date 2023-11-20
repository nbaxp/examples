import settings from "~/config/settings.js";
import Mock from "~/lib/better-mock/mock.browser.esm.js";
import { log } from "utils";
import useLocale from "./locale.js";
import useToken from "./token.js";
import useUser from "./user.js";
import useMenu from "./menu.js";

Mock.setup({
  timeout: "200-600",
});

export default function () {
  if (!settings.useMock) {
    return;
  }
  log("init mock");
  useLocale();
  useToken();
  useUser();
  useMenu();
}
