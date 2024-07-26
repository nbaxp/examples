import html from "utils";
import { ref, onMounted } from "vue";
import Cherry from "../../lib/cherry-markdown/cherry-markdown.esm.js";

export default {
  template: html`<div ref="tplRef">
    <div class="source" style="display:none;"><slot /></div>
    <div class="markdown-body"></div>
  </div>`,
  styles: html`<link rel="stylesheet" href="./lib/cherry-markdown/cherry-markdown.min.css" />`,
  props: {
    name: {
      default: null,
    },
  },
  setup(props) {
    const tplRef = ref(null);
    onMounted(async () => {
      let mdText = tplRef.value.querySelector(".source pre")?.innerText;
      if (props.name !== null) {
        const response = await fetch(`./assets/docs/${props.name}.md`);
        mdText = await response.text();
      }
      new Cherry({
        el: tplRef.value.querySelector(".markdown-body"),
        value: mdText,
        toolbars: {
          toolbar: false,
        },
        editor: {
          defaultModel: "previewOnly",
        },
        previewer: {
          className: 'cherry-markdown head-num'
        },
      });
      tplRef.value.querySelector(".source").remove();
    });
    return {
      tplRef,
    };
  },
};
