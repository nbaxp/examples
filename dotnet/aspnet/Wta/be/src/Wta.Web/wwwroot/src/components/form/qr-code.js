import html from 'utils';
import { onMounted, ref } from 'vue';
import QRCode from '~/lib/qrcode/index.esm.js';

export default {
  template: html`
    <div class="el-input__inner flex">
      <el-image fit="fill" preview-teleported :src="model" :preview-src-list="[model]" />
    </div>
  `,
  props: ['modelValue'],
  setup(props) {
    const model = ref(null);
    onMounted(async () => {
      model.value = props.modelValue
        ? await QRCode.toDataURL(props.modelValue, {
            width: 300,
          })
        : null;
    });
    return {
      model,
    };
  },
};
