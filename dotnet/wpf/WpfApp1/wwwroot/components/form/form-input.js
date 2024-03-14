import html from "html";
import { ref, reactive, watch, onMounted, nextTick } from "vue";
import { dayjs } from "element-plus";
import request from "../../request/index.js";
import { importFunction, bytesFormat } from "../../utils/index.js";
import { ElMessage, useFormItem } from "element-plus";

export default {
  template: html`
    <template v-if="getDisabled()">
      <template v-if="model[prop]!==null">
        <el-switch disabled v-model="model[prop]" type="checked" v-if="schema.type==='boolean'" />
        <template v-else-if="schema.input==='year'">{{dayjs(model[prop]).format('YYYY')}}</template>
        <template v-else-if="schema.input==='date'">{{dayjs(model[prop]).format('YYYY-MM-DD')}}</template>
        <template v-else-if="schema.input==='datetime'">{{dayjs(model[prop]).format('YYYY-MM-DD HH:mm:ss')}}</template>
        <template v-else-if="schema.input==='password'">******</template>
        <template v-else-if="schema.input==='select'||schema.input==='tabs'">{{options.find(o=>o.value==model[prop])?.label??model[prop]}}</template>
        <template v-else><pre>{{model[prop]}}</pre></template>
      </template>
    </template>
    <template v-else>
      <template v-if="getInput(schema)==='tabs'&&mode==='query'">
        <el-tabs type="card" v-model="model[prop]" style="height:24px;margin:0;" class="form">
          <el-tab-pane label="全部" key="all" :name="''" />
          <el-tab-pane v-for="item in options" :label="item.label" :name="item.value" />
        </el-tabs>
      </template>
      <template v-else-if="getInput(schema)==='select'||getInput(schema)==='tabs'">
        <el-select v-model="model[prop]" :placeholder="schema.placeholder??schema.title" :multiple="!!schema.multiple" :clearable="!!schema.clearable">
          <el-option v-for="item in options" :key="item.key" :label="item.label" :value="item.value" />
        </el-select>
      </template>
      <template v-else-if="getInput(schema)==='month'||getInput(schema)==='datetime'||getInput(schema)==='datetimerange'">
        <el-date-picker v-model="model[prop]" :type="schema.input" :value-format="schema.format??'YYYY-MM-DD HH:mm:ss'" :clearable="!!schema.clearable" />
      </template>
      <template v-else-if="getInput(schema)==='number'">
        <el-input :disabled="getDisabled()" :placeholder="schema.placeholder??schema.title" v-model="model[prop]" type="number" />
      </template>
      <template v-else-if="getInput(schema)==='integer'">
        <el-input-number :disabled="getDisabled()" :placeholder="schema.placeholder??schema.title" v-model="model[prop]" :precision="0" />
      </template>
      <template v-else-if="getInput(schema)==='boolean'">
        <el-select :disabled="getDisabled()" v-model="model[prop]" :placeholder="schema.placeholder??schema.title" v-if="schema.nullable">
          <el-option prop="select" value="" :label="$t('select')" />
          <el-option prop="true" :value="true" :label="$t('true')" />
          <el-option prop="false" :value="false" :label="$t('false')" />
        </el-select>
        <el-switch v-model="model[prop]" type="checked" v-else />
      </template>
      <template v-else-if="getInput(schema)==='file'">
        <el-upload
          ref="uploadRef"
          v-model:file-list="model[prop]"
          class="upload"
          drag
          :accept="schema.accept"
          :multiple="schema.multiple"
          :limit="limit"
          :auto-upload="false"
          :on-change="handleChange"
        >
          <template #trigger>
            <el-icon style="font-size:4em;">
              <ep-upload-filled />
            </el-icon>
          </template>
          <template #tip>
            <div class="el-upload__tip">
              <div>
                单个文件大小限制：{{ bytesFormat(size) }}，上传数量限制：{{ limit }}
                <template v-if="schema.accept">，上传文件类型：{{ schema.accept }}</template>
              </div>
            </div>
          </template>
        </el-upload>
      </template>
      <template v-else>
        <el-input
          clearable
          :disabled="getDisabled()"
          :placeholder="schema.placeholder??schema.title"
          v-model="model[prop]"
          :type="schema.input??'text'"
          :show-password="schema.input==='password'"
        />
      </template>
    </template>
  `,
  styles: html`<style>
    .form .el-tabs__header,
    .form .el-tabs__item {
      height: 24px;
    }
  </style>`,
  props: ["modelValue", "schema", "prop", "isReadOnly", "mode"],
  emit: ["update:modelValue"],
  setup(props, context) {
    const model = reactive(props.modelValue);
    watch(model, (value) => {
      context.emit("update:modelValue", value);
    });
    /*start*/
    const getDisabled = () => {
      if (props.mode === "details") {
        return true;
      }
      if (props.mode === "update" && props.schema.readOnly) {
        return true;
      }
      return false;
    };
    const getInput = (schema) => {
      return schema.input ?? schema.type;
    };
    /*end*/

    //options
    const selectProps = ref({});
    const selectValues = ref([]);
    const options = ref([]);

    //upload
    const fileList = ref([]);
    const limit = props.schema.multiple ? props.schema.limit ?? 5 : 1;
    const size = props.schema.size ?? 1024 * 1024;
    const fileTypes = props.schema.accept?.split(",").map((o) => o.toLowerCase()) ?? [];
    const { formItem } = useFormItem();
    const handleChange = async (uploadFile, uploadFiles) => {
      const ext = uploadFile.name.substr(uploadFile.name.lastIndexOf("."));
      const index = uploadFiles.findIndex((o) => o.uid !== uploadFile.uid);
      if (props.schema.accept && !fileTypes.some((o) => o === ext)) {
        ElMessage.error(`当前文件 ${uploadFile.name} 不是可选文件类型 ${props.schema.accept}`);
        uploadFiles.splice(index, 1);
        return false;
      }
      if (uploadFile.size > size) {
        ElMessage.error(`当前文件大小 ${bytesFormat(uploadFile.size)} 已超过 ${bytesFormat(size)}`);
        uploadFiles.splice(index, 1);
        return false;
      }
      if (uploadFiles.length) {
        model[props.prop] = props.schema.multiple ? uploadFiles : uploadFiles[0];
      } else {
        model[props.prop] = props.schema.multiple ? [] : null;
      }
      try {
        await formItem.validate();
      } catch (error) {
        console.log(error);
      }
    };

    //watch
    watch(
      () => model[props.prop],
      async (value) => {
        if (props.schema.watch) {
          console.log(value);
          if (props.schema.watch?.constructor === String) {
            props.schema.watch = await importFunction(props.schema.watch);
          }
          if (props.schema.watch?.constructor === Function) {
            props.schema.watch(model, value);
          }
        }
      }
    );
    onMounted(async () => {
      if (props.schema.options) {
        options.value = props.schema.options;
      } else if (props.schema.url) {
        try {
          const url = `${props.schema.url}`;
          const result = await request(url, null, { method: props.schema.method ?? "POST" });
          options.value = result.data?.items.map((o) => ({
            value: o[props.schema.value],
            label: o[props.schema.label],
          }));
          if (props.schema.defaultSelected && options.value.length) {
            model[props.prop] = options.value[0].value;
          }
        } catch (error) {
          console.log(error);
        }
      }
    });
    return {
      model,
      getDisabled,
      getInput,
      dayjs,
      selectProps,
      selectValues,
      options,
      bytesFormat,
      fileList,
      limit,
      size,
      handleChange,
    };
  },
};
