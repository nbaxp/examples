import { defineStore } from "pinia";
import settings from "../config/settings.js";
import { getUrl } from "~/utils/request.js";
import { log } from "utils";

const useAppStore = defineStore("app", {
  state: () => ({
    ...settings,
  }),
  actions: {
    async getSiteInfo() {
      log("fetch site info");
      const response = await fetch(getUrl("locale"), { method: "POST" });
      const result = response.json();
      return result;
    },
  },
});

export default useAppStore;
