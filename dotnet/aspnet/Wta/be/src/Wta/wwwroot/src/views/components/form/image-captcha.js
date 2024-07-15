import request from "@/utils/request.js";
import SvgIcon from "@/views/components/icon/index.js";
import html from "utils";
import { onMounted, ref, useModel, watch } from "vue";

export default {
  components: { SvgIcon },
  template: html`
    <div style="display: inline-flex">
      <el-input v-model="model">
        <template #prefix>
          <el-icon v-if="icon" class="el-input__icon">
            <svg-icon :name="icon" />
          </el-icon>
        </template>
      </el-input>
      <el-image
        :title="$t('clickRefresh')"
        :src="src"
        @click="onClick"
        style="cursor: pointer; max-height: 30px; margin-left: 10px"
      >
        <template #placeholder><span></span></template>
        <template #error><span></span></template>
      </el-image>
    </div>
  `,
  props: ["modelValue", "url", "method", "authCode", "codeHash"],
  emit: ["callback"],
  setup(props, context) {
    const model = useModel(props, "modelValue");
    const src = ref("");
    const load = async () => {
      const result = await request(props.method, props.url);
      src.value = result.data.data[props.authCode ?? "authCode"];
      context.emit("callback", result.data.data[props.codeHash ?? "codeHash"]);
    };

    const onClick = async () => {
      await load();
    };

    onMounted(async () => {
      await load();
    });
    return { model, src, onClick };
  },
};
