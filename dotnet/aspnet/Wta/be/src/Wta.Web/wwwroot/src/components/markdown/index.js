import html from 'utils';
import { onMounted, ref } from 'vue';
import Cherry from '~/lib/cherry-markdown/cherry-markdown.esm.js';
import '~/lib/cherry-markdown/echarts/echarts.min.js';
import '~/lib/cherry-markdown/addons/cherry-code-block-mermaid-plugin.js';
import '~/lib/cherry-markdown/mermaid/mermaid.min.js';

Cherry.usePlugin(window.CherryCodeBlockMermaidPlugin, {
  mermaid: window.mermaid,
});

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

    onMounted(async () => {
      let mdText = tplRef.value.querySelector('.source pre')?.innerText;
      if (props.name !== null) {
        const response = await fetch(`./src/assets/docs/${props.name}.md`);
        mdText = await response.text();
      }
      new Cherry({
        el: tplRef.value.querySelector('.cherry-markdown'),
        value: mdText,
        toolbars: {
          toolbar: false,
        },
        editor: {
          defaultModel: 'previewOnly',
        },
        previewer: {
          className: 'cherry-markdown',
        },
      });
      tplRef.value.querySelector('.source').remove();
    });
    return {
      tplRef,
    };
  },
};
