<template>
  <template v-if="getDisabled()">
    <div class="el-input" v-if="model[prop] !== null">
      <el-switch v-if="schema.type === 'boolean'" v-model="model[prop]" disabled type="checked" />
      <template v-else-if="schema.input === 'year'">{{ dayjs(model[prop]).format('YYYY') }}</template>
      <template v-else-if="schema.input === 'date'">{{ dayjs(model[prop]).format('YYYY-MM-DD') }}</template>
      <template v-else-if="schema.input === 'datetime'">{{
        dayjs(model[prop]).format('YYYY-MM-DD HH:mm:ss')
      }}</template>
      <template v-else-if="schema.input === 'password'">******</template>
      <template v-else-if="schema.input === 'select' || schema.input === 'tabs'">
        {{ options.find((o) => o.value == model[prop])?.label ?? model[prop] }}
      </template>
      <template v-else>
        <pre>{{ model[prop] }}</pre>
      </template>
    </div>
  </template>
  <template v-else>
    <template v-if="getInput(schema) === 'tabs' && mode === 'query'">
      <el-tabs v-model="model[prop]" type="card" style="height: 24px; margin: 0" class="form">
        <el-tab-pane key="all" label="全部" :name="''" />
        <el-tab-pane v-for="item in options" :key="item.value" :label="item.label" :name="item.value" />
      </el-tabs>
    </template>
    <template v-if="getInput(schema) === 'color'">
      <el-color-picker v-model="model[prop]" />
    </template>
    <template v-else-if="getInput(schema) === 'select' || getInput(schema) === 'tabs'">
      <el-select
        v-model="model[prop]"
        :placeholder="placeholder"
        :multiple="!!schema.multiple"
        :clearable="!!schema.clearable"
      >
        <template #prefix>
          <svg-icon
            v-if="options?.find((o) => o.value == model[prop])?.icon"
            :name="options.find((o) => o.value == model[prop])?.icon"
          />
        </template>
        <el-option v-for="item in options" :key="item.key" :label="item.label" :value="item.value">
          <span style="display: flex; align-items: center">
            <svg-icon v-if="item.icon" :name="item.icon" class="el-icon--left" />
            <span>{{ item.label }}</span>
          </span>
        </el-option>
      </el-select>
    </template>
    <template
      v-else-if="
        getInput(schema) === 'month' || getInput(schema) === 'datetime' || getInput(schema) === 'datetimerange'
      "
    >
      <el-date-picker
        v-model="model[prop]"
        :type="schema.input"
        :value-format="schema.format ?? 'YYYY-MM-DD HH:mm:ss'"
        :clearable="!!schema.clearable"
      />
    </template>
    <template v-else-if="getInput(schema) === 'number'">
      <el-input v-model="model[prop]" :disabled="getDisabled()" :placeholder="placeholder" type="number" />
    </template>
    <template v-else-if="getInput(schema) === 'integer'">
      <el-input-number v-model="model[prop]" :disabled="getDisabled()" :placeholder="placeholder" :precision="0" />
    </template>
    <template v-else-if="getInput(schema) === 'boolean'">
      <el-select v-if="schema.nullable" v-model="model[prop]" :disabled="getDisabled()" :placeholder="placeholder">
        <el-option prop="select" value="" :label="$t('select')" />
        <el-option prop="true" :value="true" :label="$t('true')" />
        <el-option prop="false" :value="false" :label="$t('false')" />
      </el-select>
      <el-checkbox v-else v-model="model[prop]" :label="schema.showLabel ? $t(placeholder) : ''" />
    </template>
    <template v-else-if="getInput(schema) === 'file'">
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
          <el-icon style="font-size: 4em">
            <i class="i-ep-upload-filled" />
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
        v-model="model[prop]"
        clearable
        :disabled="getDisabled()"
        :placeholder="placeholder"
        :type="schema.input ?? 'text'"
        :show-password="schema.input === 'password'"
      >
        <template #prefix>
          <el-icon v-if="schema.icon" class="el-input__icon"><svg-icon :name="schema.icon" /></el-icon>
        </template>
      </el-input>
    </template>
  </template>
</template>

<script setup>
  import { dayjs, ElMessage, useFormItem } from 'element-plus';
  import { computed, onMounted, reactive, ref, watch } from 'vue';
  import { useI18n } from 'vue-i18n';

  import SvgIcon from '@/components/icon/index.vue';
  import { bytesFormat, importFunction } from '@/utils/index.js';
  import request from '@/utils/request.js';

  const props = defineProps(['modelValue', 'schema', 'prop', 'isReadOnly', 'mode']);
  const emit = defineEmits(['update:modelValue']);

  const model = reactive(props.modelValue);
  watch(model, (value) => {
    emit('update:modelValue', value);
  });
  /* start */
  const { t } = useI18n();
  const placeholder = computed(() => {
    return t(props.schema.placeholder ?? props.schema.title ?? props.prop);
  });
  const getDisabled = () => {
    if (props.mode === 'details') {
      return true;
    }
    if (props.mode === 'update' && props.schema.readOnly) {
      return true;
    }
    return false;
  };
  const getInput = (schema) => {
    return schema.input ?? schema.type;
  };
  /* end */

  // options
  const selectProps = ref({});
  const selectValues = ref([]);
  const options = ref([]);

  // upload
  const fileList = ref([]);
  const limit = props.schema.multiple ? props.schema.limit ?? 5 : 1;
  const size = props.schema.size ?? 1024 * 1024;
  const fileTypes = props.schema.accept?.split(',').map((o) => o.toLowerCase()) ?? [];
  const { formItem } = useFormItem();
  const handleChange = async (uploadFile, uploadFiles) => {
    const ext = uploadFile.name.substr(uploadFile.name.lastIndexOf('.'));
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

  // watch
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
    if (props.schema.options) {
      options.value = props.schema.options;
    } else if (props.schema.url) {
      try {
        const url = `${props.schema.url}`;
        const result = await request(props.schema.method ?? 'POST', url);
        options.value = result.data?.map((o) => ({
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
</script>
