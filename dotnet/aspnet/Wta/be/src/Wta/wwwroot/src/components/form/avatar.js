import html from 'utils';
import { useModel } from 'vue';
import { VueCropper } from 'vue-cropper';

export default {
  components: { VueCropper },
  template: html`
    <div class="w-full">
      <vue-cropper :img="img" :autoCrop="true" outputType="png" />
    </div>
  `,
  styles: html`
    <link rel="stylesheet" href="./lib/vue-cropper/index.css" />
  `,
  props: ['modelValue', 'config', 'img'],
  setup(props) {
    const model = useModel(props, 'modelValue');
    return {
      model,
    };
  },
};
