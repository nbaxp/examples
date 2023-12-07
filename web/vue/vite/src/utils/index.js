import { get } from 'lodash-es';

import settings from '@/config/settings.js';

function log(message) {
  if (settings.isDebug) {
    console.log(message);
  }
}

async function delay(ms) {
  return new Promise((resolve, _) => {
    setTimeout(resolve, ms);
  });
}

function json(obj) {
  return JSON.parse(JSON.stringify(obj));
}

// format %
function persentFormat(number) {
  return `${(number * 100).toFixed(2)} %`;
}
// format bytes
function bytesFormat(bytes) {
  if (Number.isNaN(bytes)) {
    return '';
  }
  const symbols = ['bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
  let exp = Math.floor(Math.log(bytes) / Math.log(2));
  if (exp < 1) {
    exp = 0;
  }
  const i = Math.floor(exp / 10);
  bytes /= 2 ** (10 * i);

  if (bytes.toString().length > bytes.toFixed(2).toString().length) {
    bytes = bytes.toFixed(2);
  }
  return `${bytes} ${symbols[i]}`;
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

function schemaToModel(schema) {
  const entity = {};
  if (schema && schema.properties) {
    Object.keys(schema.properties).forEach((propertyName) => {
      const property = schema.properties[propertyName];
      if (property.type === 'object') {
        entity[propertyName] = schemaToModel(property);
      } else if ('default' in property) {
        entity[propertyName] = property.default;
      } else if (property.type === 'array') {
        entity[propertyName] = [];
      } else if (property.type === 'boolean') {
        entity[propertyName] = property.nullable ? null : false;
      } else if (property.type === 'number' || property.type === 'integer') {
        entity[propertyName] = property.nullable ? null : 0;
      } else if (property.type === 'string') {
        entity[propertyName] = null;
      } else {
        entity[propertyName] = null;
      }
    });
  }

  return entity;
}

function listToTree(list, func) {
  const tree = [];
  list.forEach((item) => {
    if (!item.parentId) {
      tree.push(item);
    } else {
      const parent = list.find((node) => node.id === item.parentId);
      if (parent) {
        parent.children = parent.children || [];
        parent.children.push(item);
      }
    }
    if (func) {
      func(item);
    }
  });
  return tree;
}

function treeToList(tree, list = []) {
  tree.forEach((o) => {
    list.push(o);
    if (o.children?.length) {
      treeToList(o.children, list);
    }
  });
  return list;
}

function getProp(instance, propPath) {
  return get(instance, propPath);
}

function getFileNameFromContentDisposition(contentDisposition) {
  return decodeURIComponent(/filename\*=UTF-8''([^\s]*)/i.exec(contentDisposition)[1]);
}

async function importModule(input) {
  const dataUri = `data:text/javascript;charset=utf-8,${encodeURIComponent(input)}`;
  const result = await import(dataUri);
  return result.default;
}

// await importFunction('()=>console.log(123)');
async function importFunction(input) {
  const src = input ?? `()=>{}`;
  const result = await importModule(`export default ${src}`);
  return result;
}

function reload() {
  // eslint-disable-next-line no-restricted-globals
  location.href = `${location.protocol}//${location.host}${location.pathname}`;
}

export {
  bytesFormat,
  delay,
  format,
  getFileNameFromContentDisposition,
  getProp,
  importFunction,
  json,
  listToTree,
  log,
  persentFormat,
  reload,
  schemaToModel,
  treeToList,
};
