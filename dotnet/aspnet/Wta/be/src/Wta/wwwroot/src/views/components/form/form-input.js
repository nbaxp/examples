import { bytesFormat, importFunction } from '@/utils/index.js';
import request from '@/utils/request.js';
import SvgIcon from '@/views/components/icon/index.js';
import { dayjs } from 'element-plus';
import { ElMessage, useFormItem } from 'element-plus';
import html from 'utils';
import { getProp } from 'utils';
import { onMounted, reactive, ref, watch } from 'vue';
import { useRoute } from 'vue-router';
import CodeCaptcha from './code-captcha.js';
import ImageCaptcha from './image-captcha.js';

export default {
  components: { SvgIcon, ImageCaptcha, CodeCaptcha },
  template: html`<template v-if="getDisabled()">
  <template v-if="model[prop]!==null&&(schema.type!=='array'||model[prop].length>0)">
    <template v-if="schema.type==='boolean'||model[prop]!==false">
      <template v-if="schema.type==='boolean'">
        <el-button v-if="schema.input==='select'" link type="primary">
          {{options?.find(o=>o.value==model[prop])?.label??model[prop]}}
        </el-button>
        <el-switch v-else disabled v-model="model[prop]" type="checked" />
      </template>
      <template v-else-if="schema.input==='year'">{{dayjs(model[prop]).format('YYYY')}}</template>
      <template v-else-if="schema.input==='date'">{{dayjs(model[prop]).format('YYYY-MM-DD')}}</template>
      <template v-else-if="schema.input==='datetime'">
        {{dayjs(model[prop]).format('YYYY-MM-DD HH:mm:ss')}}
      </template>
      <template v-else-if="schema.input==='password'">******</template>
      <template v-else-if="schema.input==='select'||schema.input==='tabs'">
        <template v-if="!schema.meta?.multiple">
          <el-button link type="primary">
            {{options?.find(o=>o.value==model[prop])?.label??model[prop]}}
          </el-button>
        </template>
        <template v-else>
          <el-button link type="primary" v-for="item in model[prop]" :key="item">
            {{options?.find(o=>o.value==item)?.label??item}}
          </el-button>
        </template>
      </template>
      <template v-else-if="schema.input==='image-inline'">
        <img :src="model[prop]" style="max-height:18px;" />
      </template>
      <template v-else><span>{{model[prop]}}</span></template>
    </template>
  </template>
</template>
<!--display-edit-->
<template v-else>
  <template v-if="schema.input==='color'">
    <el-color-picker v-model="model[prop]" />
  </template>
  <template v-else-if="schema.input==='select'">
    <el-select
      v-model="model[prop]"
      :placeholder="schema.meta?.placeholder??schema.meta?.title"
      :multiple="!!schema.meta?.multiple"
      :value-on-clear="null"
      clearable
    >
      <el-option v-for="item in options" :key="item.key" :label="item.label" :value="item.value">
        <span style="display:flex;align-items:center;">
          <el-icon v-if="item.icon" class="el-icon--left">
            <svg-icon :name="item.icon" />
          </el-icon>
          {{item.label}}
        </span>
      </el-option>
    </el-select>
  </template>
  <template v-else-if="schema.input==='month'||schema.input==='datetime'||schema.input==='datetimerange'">
    <el-date-picker
      v-model="model[prop]"
      :type="schema.input"
      :value-format="schema.format??'YYYY-MM-DD HH:mm:ss'"
      :placeholder="schema.placeholder??schema.title"
      clearable
    />
  </template>
  <template v-else-if="schema.input==='number'">
    <el-input
      :disabled="getDisabled()"
      :placeholder="schema.placeholder??schema.title"
      v-model="model[prop]"
      type="number"
    />
  </template>
  <template v-else-if="schema.input==='integer'">
    <el-input-number
      :disabled="getDisabled()"
      :placeholder="schema.placeholder??schema.title"
      v-model="model[prop]"
      :precision="0"
    />
  </template>
  <template v-else-if="schema.input==='switch'">
    <el-switch v-model="model[prop]" type="checked" />
    <span v-if="schema.meta.showLabel" class="pl-4">{{schema.title}}</span>
  </template>
  <template v-else-if="schema.input==='boolean'">
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
    <template v-else>
      <el-switch v-model="model[prop]" type="checked" />
      <span v-if="schema.showLabel" class="pl-4">{{schema.title}}</span>
    </template>
  </template>
  <template v-else-if="schema.input === 'image-captcha'">
    <image-captcha
      v-model="model[prop]"
      :url="schema.meta.url"
      :codeHash="schema.codeHash"
      @callback="updateCodeHash"
      :errors="errors"
      :prop="prop"
      :icon="schema.meta.icon"
    />
  </template>
  <template v-else-if="schema.input === 'code-captcha'">
    <code-captcha
      v-model="model[prop]"
      :icon="schema.icon"
      :url="schema.url"
      :codeHash="schema.codeHash"
      @callback="updateCodeHash"
      :query="model[schema.query]"
      :regexp="schema.regexp"
    />
  </template>
  <template v-else-if="schema.input==='image-upload'||schema.input==='image-inline'">
    <el-upload
      class="el-input__inner flex"
      :show-file-list="false"
      :auto-upload="false"
      :limit="1"
      :accept="schema.meta?.accept"
      v-model:file-list="fileList"
      :on-change="uploadOnChange"
    >
      <template #trigger>
        <el-icon v-if="!model[prop]"><ep-plus /></el-icon>
      </template>
      <template #tip>
        <el-image v-if="model[prop]" :src="model[prop]" :preview-src-list="[model[prop]]" />
        <el-icon v-if="model[prop]" class="cursor-pointer h-full" @click="()=>{model[prop]=null;fileList.value=[]}">
          <ep-close />
        </el-icon>
      </template>
    </el-upload>
  </template>
  <template v-else-if="schema.input==='file'">
    <el-upload
      v-model:file-list="model[prop]"
      class="upload"
      drag
      :accept="schema.meta?.accept"
      :multiple="schema.meta?.multiple"
      :limit="limit"
      :auto-upload="false"
      :on-change="uploadOnChange"
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
            <template v-if="schema.meta?.accept">，上传文件类型：{{ schema.meta?.accept }}</template>
          </div>
        </div>
      </template>
    </el-upload>
  </template>
  <template v-else-if="schema.input==='checkbox'">
    <el-checkbox v-model="model[prop]" :label="schema.title" size="large" />
  </template>
  <template v-else>
    <el-input
      clearable
      :disabled="getDisabled()"
      :placeholder="prop"
      v-model="model[prop]"
      :type="schema.input"
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
  props: ['modelValue', 'schema', 'prop', 'isReadOnly', 'mode', 'errors'],
  emit: ['update:modelValue'],
  setup(props, context) {
    const model = reactive(props.modelValue);
    watch(model, (value) => {
      context.emit('update:modelValue', value);
    });
    const route = useRoute();
    /*start*/
    const getDisabled = () => {
      if (props.mode === 'details') {
        return true;
      }
      if (props.schema.meta?.readOnly) {
        return true;
      }
      return false;
    };
    /*end*/

    //options
    const selectProps = ref({});
    const selectValues = ref([]);
    const options = ref([]);

    //
    const updateCodeHash = (data) => {
      model[props.schema.codeHash ?? 'codeHash'] = data;
    };

    //upload
    const fileList = ref([]);
    const limit = props.schema.meta?.multiple ? props.schema.meta?.limit ?? 5 : 1;
    const size = props.schema.meta?.size ?? 1024 * 1024;
    const fileTypes = props.schema.meta?.accept?.split(',').map((o) => o.toLowerCase()) ?? [];
    const { formItem } = useFormItem();

    const uploadOnChange = async (uploadFile, uploadFiles) => {
      const ext = uploadFile.name.substr(uploadFile.name.lastIndexOf('.'));
      const index = uploadFiles.findIndex((o) => o.uid !== uploadFile.uid);
      uploadFiles.splice(index, 1);
      if (props.schema.accept && !fileTypes.some((o) => o === ext)) {
        ElMessage.error(`当前文件 ${uploadFile.name} 不是可选文件类型 ${props.schema.accept}`);
        return false;
      }
      if (uploadFile.size > size) {
        ElMessage.error(`当前文件大小 ${bytesFormat(uploadFile.size)} 已超过 ${bytesFormat(size)}`);
        return false;
      }
      if (props.schema.input === 'image-upload') {
        const formData = new FormData();
        formData.append('file', uploadFile.raw);
        const data = await request('POST', props.schema.meta.url, formData);
        model[props.prop] = data.data.data;
        await formItem.validate();
      } else if (props.schema.input === 'image-inline') {
        const reader = new FileReader();
        reader.onload = async (o) => {
          model[props.prop] = o.target.result;
          await formItem.validate();
        };
        reader.readAsDataURL(uploadFile.raw);
      } else {
        if (uploadFiles.length) {
          model[props.prop] = props.schema.meta?.multiple ? uploadFiles : uploadFiles[0];
        } else {
          model[props.prop] = props.schema.meta?.multiple ? [] : null;
        }
        await formItem.validate();
      }
    };

    //

    const fetchOptions = async () => {
      route.meta.cache ||= new Map();
      const map = route.meta.cache;
      const url = `${props.schema.meta.url}`;
      let postData = props.schema.meta?.data ?? {
        includeAll: true,
      };
      if (props.schema.data instanceof Function) {
        postData = props.schema.data(model[props.schema.dependsOn]);
      }
      const key = JSON.stringify({
        url,
        postData,
      });
      options.value = map.get(key);
      if (!options.value) {
        const method = props.schema.meta?.method || 'POST';
        const data = (await request(method, url, postData)).data;
        if (!data.error) {
          options.value = getProp(data, props.schema.meta?.path ?? 'data.items').map((o) => {
            if (Array.isArray(o)) {
              return {
                value: o[0],
                label: o[1],
              };
            }
            if (o instanceof Object) {
              return {
                value: o[props.schema.meta?.value ?? 'value'],
                label: o[props.schema.meta?.label ?? 'label'],
              };
            }
            return {
              value: o,
              label: o,
            };
          });
          map.set(key, options.value);
        } else {
          options.value = [];
        }
        if (!model[props.prop] && props.schema.meta.selected && options.value.length) {
          model[props.prop] = options.value[0].value;
        }
      }
    };

    //if (props.schema?.dependsOn) {
    watch(
      () => model[props.schema.meta?.dependsOn],
      async () => {
        if (props.schema.meta?.options) {
          options.value = props.schema.meta?.options;
        } else if (props.schema.meta?.url && props.schema.input === 'select') {
          if (!props.schema.meta?.dependsOn || model[props.schema.meta?.dependsOn]) {
            await fetchOptions();
          } else {
            options.value = [];
          }
          // if (
          //   (model[props.prop] || model[props.prop] === false) &&
          //   !options.value.find((o) => o.value === model[props.prop])
          // ) {
          //   model[props.prop] = null;
          // }
        }
      },
      { immediate: true },
    );
    //}

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
      },
    );

    onMounted(async () => {
      //await fetchOptions();
    });
    return {
      model,
      getDisabled,
      dayjs,
      selectProps,
      selectValues,
      options,
      bytesFormat,
      fileList,
      limit,
      size,
      fetchOptions,
      updateCodeHash,
      uploadOnChange,
    };
  },
};
