import settings from '@/config/settings.js';
import { get } from 'lodash';

function log(message) {
  if (settings.isDebug) {
    console.log(message);
  }
}

async function delay(ms) {
  return new Promise((resolve, _) => setTimeout(resolve, ms));
}

// format html`...` by vscode lit-html
function html(strings, ...values) {
  let output = '';
  let index;
  for (index = 0; index < values.length; index += 1) {
    output += strings[index] + values[index];
  }
  output += strings[index];
  return output;
}

// format %
function persentFormat(number) {
  return `${Number.parseFloat(number * 100).toFixed(2)} %`;
}
// format bytes
function bytesFormat(bytes) {
  // 输入验证：确保bytes是一个数字，且不为负
  if (Number.isNaN(bytes) || bytes < 0) {
    return 'Invalid input';
  }
  if (bytes === 0) {
    return '0 bytes';
  }

  const symbols = ['bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
  let exp = Math.floor(Math.log2(bytes));
  const maxUnitIndex = symbols.length - 1;
  exp = Math.min(exp, maxUnitIndex * 10);
  const unitIndex = Math.floor(exp / 10);
  const unit = symbols[unitIndex];
  let formattedBytes = (bytes / 2 ** (10 * (exp - unitIndex * 10))).toFixed(2);
  formattedBytes = Number.parseFloat(formattedBytes);
  return `${formattedBytes} ${unit}`;
}

// string format
function format(template, ...args) {
  const formatRegExp = /%[sdj%]/g;
  let counter = 0;
  return template.replace(formatRegExp, (match) => {
    const index = counter;
    counter += 1;
    if (match === '%%') {
      return '%';
    }
    if (index > args.length - 1) {
      return match;
    }
    if (match === '%s') {
      return String(args[index]);
    }
    if (match === '%d') {
      return Number(args[index]);
    }
    if (match === '%j') {
      return JSON.stringify(args[index]);
    }
    return match;
  });
}

function schemaToModel(schema, defProp = 'default') {
  const entity = {};
  const propertyNames = Object.keys(schema.properties ?? {});
  for (const propertyName of propertyNames) {
    const property = schema.properties[propertyName];
    if (property.type === 'object') {
      entity[propertyName] = schemaToModel(property);
    } else if (defProp in property) {
      entity[propertyName] = property.default;
    } else if (property.type === 'array') {
      entity[propertyName] = [];
    } else if (property.type === 'boolean') {
      entity[propertyName] = false; //property.nullable ? null : false;
    } else if (property.type === 'number' || property.type === 'integer') {
      entity[propertyName] = null; //property.nullable ? null : 0;
    } else if (property.type === 'string') {
      entity[propertyName] = null;
    } else {
      entity[propertyName] = null;
    }
  }
  return entity;
}

function listToTree(list, config) {
  const options = Object.assign(
    {
      parentId: 'parentId',
      id: 'id',
      children: 'children',
      func: null,
    },
    config,
  );
  const tree = [];
  for (const item of list) {
    if (!item[options.parentId]) {
      tree.push(item);
    } else {
      const parent = list.find((node) => node[options.id] === item[options.parentId]);
      if (parent) {
        parent[options.children] = parent[options.children] || [];
        parent[options.children].push(item);
      }
    }
    if (options.func) {
      func(item);
    }
  }
  return tree;
}

function treeToList(tree, list = []) {
  for (const o of tree) {
    list.push(o);
    if (o.children?.length) {
      treeToList(o.children, list);
    }
  }
  return list;
}

function getProp(instance, propPath) {
  return get(instance, propPath);
}

function getFileNameFromContentDisposition(contentDisposition) {
  const filenameRegex = /filename[^;\n]*=(UTF-\d['"]*)?((['"]).*?[.]$\2|[^;\n]*)?/gi;
  const matches = filenameRegex.exec(contentDisposition);
  if (matches?.[2]) {
    return decodeURIComponent(matches?.[2]);
  }
  return null;
}

async function importModule(input) {
  const dataUri = `data:text/javascript;charset=utf-8,${encodeURIComponent(input)}`;
  const result = await import(dataUri /* @vite-ignore */);
  return result.default;
}

// await importFunction('()=>console.log(123)');
async function importFunction(input) {
  const src = input ?? '()=>{}';
  const result = await importModule(`export default ${src}`);
  return result;
}

function reload() {
  location.href = `${location.protocol}//${location.host}${location.pathname}`;
}

function downloadFile(file, name) {
  const url = window.URL.createObjectURL(file);
  const link = document.createElement('a');
  link.href = url;
  link.setAttribute('download', name);
  document.body.appendChild(link);
  try {
    link.click();
  } finally {
    document.body.removeChild(link);
    url.revokeObjectURL();
  }
}

export default html;
export {
  log,
  delay,
  persentFormat,
  bytesFormat,
  format,
  schemaToModel,
  listToTree,
  treeToList,
  getProp,
  getFileNameFromContentDisposition,
  importFunction,
  reload,
  downloadFile,
};
