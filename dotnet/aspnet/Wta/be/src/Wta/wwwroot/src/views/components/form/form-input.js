import { bytesFormat, importFunction } from "@/utils/index.js";
import request from "@/utils/request.js";
import SvgIcon from "@/views/components/icon/index.js";
import { dayjs } from "element-plus";
import { ElMessage, useFormItem } from "element-plus";
import html from "utils";
import { getProp } from "utils";
import { onMounted, reactive, ref, watch } from "vue";
import { useRoute } from "vue-router";
import ImageCaptcha from "./image-captcha.js";

export default {
  components: { SvgIcon, ImageCaptcha },
  template: html`<template v-if="getDisabled()">
      <template
        v-if="model[prop]!==null&&(schema.type!=='array'||model[prop].length>0)"
      >
        <template v-if="schema.type==='boolean'||model[prop]!==false">
          <template v-if="schema.type==='boolean'">
            <el-button v-if="schema.input==='select'" link type="primary">
              {{options?.find(o=>o.value==model[prop])?.label??model[prop]}}
            </el-button>
            <el-switch v-else disabled v-model="model[prop]" type="checked" />
          </template>
          <template v-else-if="schema.input==='year'"
            >{{dayjs(model[prop]).format('YYYY')}}</template
          >
          <template v-else-if="schema.input==='date'"
            >{{dayjs(model[prop]).format('YYYY-MM-DD')}}</template
          >
          <template v-else-if="schema.input==='datetime'">
            {{dayjs(model[prop]).format('YYYY-MM-DD HH:mm:ss')}}
          </template>
          <template v-else-if="schema.input==='password'">******</template>
          <template v-else-if="schema.input==='select'||schema.input==='tabs'">
            <template v-if="!schema.multiple">
              <el-button link type="primary">
                {{options?.find(o=>o.value==model[prop])?.label??model[prop]}}
              </el-button>
            </template>
            <template v-else>
              <el-button
                link
                type="primary"
                v-for="item in model[prop]"
                :key="item"
              >
                {{options?.find(o=>o.value==item)?.label??item}}
              </el-button>
            </template>
          </template>
          <template v-else-if="schema.input==='base64image'">
            <img
              :src="'data:image/png;base64,'+model[prop]"
              style="max-height:18px;"
            />
          </template>
          <template v-else><span>{{model[prop]}}</span></template>
        </template>
      </template>
    </template>
    <template v-else>
      <template v-if="getInput(schema)==='tabs'&&mode==='query'">
        <el-tabs
          type="card"
          v-model="model[prop]"
          style="height:24px;margin:0;"
          class="form"
        >
          <el-tab-pane label="全部" key="all" :name="''" />
          <el-tab-pane
            v-for="item in options"
            :label="item.label"
            :name="item.value"
          />
        </el-tabs>
      </template>
      <template v-if="getInput(schema)==='color'">
        <el-color-picker v-model="model[prop]" />
      </template>
      <template
        v-else-if="getInput(schema)==='select'||getInput(schema)==='tabs'"
      >
        <el-select
          v-model="model[prop]"
          :placeholder="schema.placeholder??schema.title"
          :multiple="!!schema.multiple"
          :value-on-clear="null"
          clearable
        >
          <el-option
            v-for="item in options"
            :key="item.key"
            :label="item.label"
            :value="item.value"
          >
            <span style="display:flex;align-items:center;">
              <el-icon v-if="item.icon" class="el-icon--left">
                <svg-icon :name="item.icon" />
              </el-icon>
              {{item.label}}
            </span>
          </el-option>
        </el-select>
      </template>
      <template
        v-else-if="getInput(schema)==='month'||getInput(schema)==='datetime'||getInput(schema)==='datetimerange'"
      >
        <el-date-picker
          v-model="model[prop]"
          :type="schema.input"
          :value-format="schema.format??'YYYY-MM-DD HH:mm:ss'"
          :placeholder="schema.placeholder??schema.title"
          clearable
        />
      </template>
      <template v-else-if="getInput(schema)==='number'">
        <el-input
          :disabled="getDisabled()"
          :placeholder="schema.placeholder??schema.title"
          v-model="model[prop]"
          type="number"
        />
      </template>
      <template v-else-if="getInput(schema)==='integer'">
        <el-input-number
          :disabled="getDisabled()"
          :placeholder="schema.placeholder??schema.title"
          v-model="model[prop]"
          :precision="0"
        />
      </template>
      <template v-else-if="getInput(schema)==='boolean'">
        <el-select
          :value-on-clear="null"
          clearable
          :disabled="getDisabled()"
          v-model="model[prop]"
          :placeholder="schema.placeholder??schema.title"
          v-if="schema.nullable"
        >
          <el-option prop="select" value="" :label="$t('select')" />
          <el-option prop="true" :value="true" :label="$t('true')" />
          <el-option prop="false" :value="false" :label="$t('false')" />
        </el-select>
        <el-switch v-model="model[prop]" type="checked" v-else />
      </template>
      <template v-else-if="schema.input === 'image-captcha'">
        <image-captcha
          v-model="model[prop]"
          :url="schema.url"
          :codeHash="schema.codeHash"
          @callback="updateCodeHash"
        />
      </template>
      <template v-else-if="getInput(schema)==='base64image'">
        <el-upload
          ref="uploadRef"
          :show-file-list="false"
          :auto-upload="false"
          :on-change="handleChange"
        >
          <img
            v-if="model[prop]"
            :src="'data:image/png;base64,'+model[prop]"
            style="max-height:18px;"
          />
          <el-icon v-else class="avatar-uploader-icon"><ep-plus /></el-icon>
        </el-upload>
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
                单个文件大小限制：{{ bytesFormat(size) }}，上传数量限制：{{
                limit }}
                <template v-if="schema.accept"
                  >，上传文件类型：{{ schema.accept }}</template
                >
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
          :prefix-icon="schema.icon"
        />
      </template>
    </template>`,
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
    const route = useRoute();
    /*start*/
    const getDisabled = () => {
      if (props.mode === "details") {
        return true;
      }
      if (props.schema.readOnly) {
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

    //
    const updateCodeHash = (data) => {
      model[props.schema.codeHash ?? "codeHash"] = data;
    };

    //upload
    const fileList = ref([]);
    const limit = props.schema.multiple ? props.schema.limit ?? 5 : 1;
    const size = props.schema.size ?? 1024 * 1024;
    const fileTypes =
      props.schema.accept?.split(",").map((o) => o.toLowerCase()) ?? [];
    const { formItem } = useFormItem();
    const handleChange = async (uploadFile, uploadFiles) => {
      const ext = uploadFile.name.substr(uploadFile.name.lastIndexOf("."));
      const index = uploadFiles.findIndex((o) => o.uid !== uploadFile.uid);
      if (props.schema.accept && !fileTypes.some((o) => o === ext)) {
        ElMessage.error(
          `当前文件 ${uploadFile.name} 不是可选文件类型 ${props.schema.accept}`
        );
        uploadFiles.splice(index, 1);
        return false;
      }
      if (uploadFile.size > size) {
        ElMessage.error(
          `当前文件大小 ${bytesFormat(uploadFile.size)} 已超过 ${bytesFormat(
            size
          )}`
        );
        uploadFiles.splice(index, 1);
        return false;
      }
      //
      if (props.schema.input === "base64image") {
        if (uploadFiles.length) {
          const reader = new FileReader();
          reader.onload = (o) => {
            model[props.prop] = o.target.result.split(",")[1];
          };
          reader.readAsDataURL(uploadFile.raw);
        } else {
          model[props.prop] = null;
        }
      } else {
        if (uploadFiles.length) {
          model[props.prop] = props.schema.multiple
            ? uploadFiles
            : uploadFiles[0];
        } else {
          model[props.prop] = props.schema.multiple ? [] : null;
        }
        try {
          await formItem.validate();
        } catch (error) {
          console.log(error);
        }
      }
    };

    //

    const fetchOptions = async () => {
      route.meta.cache ||= new Map();
      const map = route.meta.cache;
      const url = `${props.schema.url}`;
      let postData = props.schema.data;
      if (props.schema.data instanceof Function) {
        postData = props.schema.data(model[props.schema.dependsOn]);
      }
      const key = JSON.stringify({
        url,
        postData,
      });
      options.value = map.get(key);
      if (!options.value) {
        const method = props.schema.method || "post";
        const data = (await request(method, url, postData)).data;
        if (!data.error) {
          options.value = getProp(data, props.schema.path ?? "data.items").map(
            (o) => {
              if (Array.isArray(o)) {
                return {
                  value: o[0],
                  label: o[1],
                };
              }
              if (o instanceof Object) {
                return {
                  value: o[props.schema.value ?? "value"],
                  label: o[props.schema.label ?? "label"],
                };
              }
              return {
                value: o,
                label: o,
              };
            }
          );
          map.set(key, options.value);
        } else {
          options.value = [];
        }
        if (props.schema.selected && options.value.length) {
          model[props.prop] = options.value[0].value;
        }
      }
    };

    watch(
      () => model[props.schema.dependsOn],
      async () => {
        if (props.schema.options) {
          options.value = props.schema.options;
        } else if (props.schema.url && props.schema.input === "select") {
          if (!props.schema.dependsOn || model[props.schema.dependsOn]) {
            await fetchOptions();
          } else {
            options.value = [];
          }
          if (
            (model[props.prop] || model[props.prop] === false) &&
            !options.value.find((o) => o.value === model[props.prop])
          ) {
            model[props.prop] = null;
          }
        }
      },
      { immediate: true }
    );

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

    onMounted(async () => {});
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
      fetchOptions,
      updateCodeHash,
    };
  },
};
