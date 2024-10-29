import SvgIcon from '@/components/icon/index.js';
import request from '@/utils/request.js';
import html from 'utils';
import { onMounted, ref, useModel, watch } from 'vue';

export default {
  components: { SvgIcon },
  template: html`
    <el-input v-model="model">
      <template #prefix>
        <el-icon class="el-input__icon">
          <svg-icon name="auth" />
        </el-icon>
      </template>
      <template #append>
        <el-image
          :title="$t('点击刷新')"
          :src="src"
          @click="onClick"
          style="cursor: pointer;"
        >
          <template #placeholder><span></span></template>
          <template #error><span></span></template>
        </el-image>
      </template>
    </el-input>
  `,
  props: [
    'modelValue',
    'url',
    'method',
    'authCode',
    'codeHash',
    'errors',
    'prop',
  ],
  emit: ['callback'],
  setup(props, context) {
    const model = useModel(props, 'modelValue');
    const src = ref('');
    const load = async () => {
      const result = await request(props.method, props.url);
      src.value = result.data.data[props.authCode ?? 'authCode'];
      context.emit('callback', result.data.data[props.codeHash ?? 'codeHash']);
    };

    const onClick = async () => {
      await load();
    };
    watch(
      () => props.errors[props.prop],
      async () => {
        if (props.errors[props.prop]) {
          await load();
        }
      },
    );
    onMounted(async () => {
      await load();
    });
    return { model, src, onClick };
  },
};
