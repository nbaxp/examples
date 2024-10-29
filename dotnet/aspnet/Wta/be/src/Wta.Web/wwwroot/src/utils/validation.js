import i18n from '@/locales/index.js';
import { format } from './index.js';
import request from './request.js';

const messages = {
  default: '{0}验证失败',
  required: '{0}是必填项',
  enum: '{0}必须是{1}之一',
  whitespace: '{0}不能为空',
  // date: {
  //   format: '{0} date {0} is invalid for format {0}',
  //   parse: '{0} date could not be parsed, {0} is invalid ',
  //   invalid: '{0} date {0} is invalid',
  // },
  types: {
    string: '{0}不是有效的字符串',
    method: '{0}不是有效的函数',
    array: '{0}不是有效的数组',
    object: '{0}不是有效的对象',
    number: '{0}不是有效的数字',
    date: '{0}不是有效的日期',
    boolean: '{0}不是有效的布尔值',
    integer: '{0}不是有效的整数',
    float: '{0}不是有效的浮点数',
    regexp: '{0}不是有效的正则表达式',
    email: '{0}不是有效的邮箱',
    url: '{0}不是有效的 url',
    hex: '{0}不是有效的十六进制',
  },
  string: {
    len: '{0}长度必须是{1}',
    min: '{0}最小长度为{1}',
    max: '{0}最大长度为{1}',
    range: '{0}长度必须在{1}和{2}之间',
  },
  number: {
    len: '{0}必须等于{1}',
    min: '{0}不小于{1}',
    max: '{0}不大于{1}',
    range: '{0}必须在{1}和{2}之间',
  },
  array: {
    len: '{0}的数量必须是{1}',
    min: '{0}的数量不小于{1}',
    max: '{0}的数量不大于{1}',
    range: '{0}的数量必须在{1}和{2}之间',
  },
  pattern: {
    mismatch: '{0}的值必须是正确的格式',
  },
  clone: function clone() {
    const cloned = JSON.parse(JSON.stringify(this));
    cloned.clone = this.clone;
    return cloned;
  },
  //
  compare: '{0} 和 {0} 输入必须一致',
  true: '{0}必须选中',
  remote: '{0}远程验证失败',
  file: '{0}是必填项',
};

const { t } = i18n.global;

const validators = {
  compare(rule, value, callback) {
    const errors = [];
    if (value && value !== rule.data[rule.compare]) {
      const message = format(messages.compare, rule.title, rule.schema.properties[rule.compare].title);
      errors.push(new Error(message));
    }
    callback(errors);
  },
  true(rule, value, callback) {
    const errors = [];
    if (!value) {
      const message = format(messages.true, rule.title);
      errors.push(new Error(message));
    }
    callback(errors);
  },
  remote(rule, value, callback) {
    const errors = [];
    //const message = format(rule.message ?? messages.remote, rule.title);
    if (!value) {
      callback(errors);
    } else {
      const { url } = rule;
      const method = rule.method ?? 'POST';
      const data = { [rule.field]: value };
      request(method, url, data, null, true)
        .then((result) => {
          if (result.data.data === false) {
            const message = i18n.global.t(result.data.message ?? rule.message, i18n.global.t(rule.title ?? rule.field));
            errors.push(new Error(message));
          }
          callback(errors);
        })
        .catch((o) => {
          errors.push(o);
          callback(errors);
        });
    }
  },
  file(rule, value, callback) {
    const errors = [];
    if (!value) {
      errors.push(new Error(t(messages.file, [rule.title])));
    }
    callback(errors);
  },
};

//
const getRules = (parentSchema, property, data, prop) => {
  if (!property.rules) {
    return null;
  }
  const rules = [...(Array.isArray(property.rules) ? property.rules : [property.rules])].map((o) =>
    Object.assign({}, o),
  );
  for (const rule of Object.values(rules)) {
    rule.data = data;
    rule.schema = parentSchema;
    rule.title ??= t(property.title ?? prop);
    if (!rule.type && property.type !== 'object') {
      if (property.type !== 'number' || property.input !== 'select') {
        rule.type = property.type;
      }
    }
    if (rule.validator) {
      if (rule.validator.constructor === String) {
        rule.validator = validators[rule.validator];
      }
    }
    if (!rule.message) {
      if (rule.required) {
        rule.message = format(messages.required, rule.title);
      } else if (rule.type === 'email') {
        rule.message = format(messages.types.email, rule.title);
      } else if (rule.pattern) {
        rule.message = format(messages.pattern.mismatch, property.title);
      } else if (property.type === 'string' || property.type === 'number' || property.type === 'array') {
        if (rule.len) {
          rule.message = format(messages[property.type].len, rule.title, rule.len);
        } else if (rule.min) {
          rule.message = format(messages[property.type].min, rule.title, rule.min);
        } else if (rule.max) {
          rule.message = format(messages[property.type].max, rule.title, rule.max);
        } else if (rule.range) {
          rule.message = format(messages[property.type].range, rule.title, rule.range);
        }
      }
    } else {
      rule.message = t(rule.message, [rule.title]);
    }
  }
  return rules;
};

//Object.assign(Schema.messages, messages);
//Object.assign(Schema.validators, validators);

function required() {
  return { required: true };
}

function trim(message) {
  return {
    pattern: '^(?!\\s).*(?<!\\s)$',
    message: message ?? '无效的空格字符',
    trigger: 'blur',
  };
}

export { getRules, required, trim };
