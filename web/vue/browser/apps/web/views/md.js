import html from "utils";
import Md from "~/components/markdown/index.js";

export default function (name) {
  return {
    components: { Md },
    template: html`<md name="${name}" />`,
  };
}
