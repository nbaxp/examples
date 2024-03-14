import { createI18n } from "vue-i18n";
import { getLocalizationAsync } from "../api/site.js";

const localization = await getLocalizationAsync();
const i18n = createI18n(localization);

export default i18n;
