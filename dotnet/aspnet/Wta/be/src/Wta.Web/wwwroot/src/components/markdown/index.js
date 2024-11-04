import html from "utils";
import { onMounted, ref,watchEffect } from "vue";
import Cherry from "~/lib/cherry-markdown/cherry-markdown.esm.js";
import "~/lib/cherry-markdown/echarts/echarts.min.js";
import "~/lib/cherry-markdown/addons/advance/cherry-table-echarts-plugin.js";
import { useAppStore } from "@/store/index.js";

export default {
  template: html`
    <div ref="tplRef" style="width:100%;">
      <div class="source" style="display:none;"><slot /></div>
      <div class="cherry-markdown"></div>
    </div>
  `,
  styles: html`
    <link
      rel="stylesheet"
      href="./lib/cherry-markdown/cherry-markdown.min.css"
    />
  `,
  props: {
    name: {
      default: null,
    },
  },
  setup(props) {
    const tplRef = ref(null);
    const appStore = useAppStore();
    let cherry;
    watchEffect(()=>{
      if(appStore.settings.theme==='light')
      {
        cherry?.setTheme('default');
        cherry?.setCodeBlockTheme('default');
      }
      else{
        cherry?.setTheme('dark');
        cherry?.setCodeBlockTheme('dark');
      }
      localStorage.removeItem(`${cherry?.options.nameSpace}-theme`);
      localStorage.removeItem(`${cherry?.options.nameSpace}-codeTheme`);
    });
    onMounted(async () => {
      let mdText = tplRef.value.querySelector(".source pre")?.innerText;
      if (props.name !== null) {
        const response = await fetch(`./src/assets/docs/${props.name}.md`);
        mdText = await response.text();
      }
      cherry = new Cherry({
        el: tplRef.value.querySelector(".cherry-markdown"),
        value: mdText,
        themeSettings: {
          // 目前应用的主题
          mainTheme: appStore.settings.theme,
          // 目前应用的代码块主题
          codeBlockTheme: appStore.settings.theme,
        },
        externals: {
          // echarts: window.echarts,
          // katex: window.katex,
          MathJax: window.MathJax,
        },
        engine: {
          syntax: {
            header: {
              anchorStyle: "autonumber",
            },
            toc: {
              showAutoNumber: true,
              // updateLocationHash: true,
              // position: 'fixed',
            },
            mathBlock: {
              engine: "MathJax", // katex或MathJax
              src: "./lib/mathjax/tex-svg.js",
            },
          },
        },
        isPreviewOnly: true,
        toolbars: {
          toolbar: false,
        },
        editor: {
          defaultModel: "previewOnly",
        },
        previewer: {
          className: "cherry-markdown",
          enablePreviewerBubble: false,
        },
        // event: {
        //   changeMainTheme: (theme) => alert(theme),
        //   changeCodeBlockTheme: (theme) => alert(theme),
        // },
      });
      tplRef.value.querySelector(".source").remove();
    });
    return {
      tplRef,
    };
  },
};
