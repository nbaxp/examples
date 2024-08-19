import { delay } from '@/utils/index.js';
import request from '@/utils/request.js';
import html from 'utils';
import { computed, ref, watch } from 'vue';

export default {
  components: {},
  template: html`
    <div style="display: inline-flex; width: 100%">
      <el-input v-model="model" style="min-width: 50%" :placeholder="$t('验证码')">
        <template #prefix>
          <el-icon v-if="icon" class="el-input__icon"><svg-icon :name="icon" /></el-icon>
        </template>
      </el-input>
      <el-button
        :title="$t('点击刷新')"
        @click="onClick"
        style="max-height: 30px; margin-left: 10px"
        :disabled="disabled"
      >
        <template v-if="!loading">{{ $t('发送验证码') }}</template>
        <template v-else>{{ $t('秒后重新发送', [seconds]) }}</template>
      </el-button>
    </div>
  `,
  props: ['modelValue', 'icon', 'method', 'url', 'code', 'codeHash', 'timeout', 'query', 'regexp'],
  emit: ['update:modelValue', 'callback'],
  setup(props, context) {
    const model = ref(props.modelValue);
    watch(model, (value) => {
      context.emit('update:modelValue', value);
    });

    const loading = ref(false);
    const seconds = ref(props.timeout);
    const disabled = computed(() => {
      return new RegExp(props.regexp).test(props.query) === false || loading.value;
    });

    const onClick = async () => {
      if (!loading.value) {
        loading.value = true;
        try {
          const result = await request(props.method, props.url, props.query);
          props.emit('callback', result.data[props.codeHash]);
          const expires = new Date(result.data.expires);
          seconds.value = props.timeout;
          let now = new Date();
          while (now < expires) {
            await delay(500);
            seconds.value = Number.parseInt((expires - now) / 1000);
            now = new Date();
          }
        } finally {
          loading.value = false;
        }
      }
    };
    return {
      disabled,
      onClick,
    };
  },
};
