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
  return template.replace(/{([0-9]+)}/g, function (match, index) {
    return typeof args[index] === 'undefined' ? match : args[index];
  });
}

function camelCase(str) {
  return str
    .replace(/(?:^\w|[A-Z]|\b\w)/g, function (word, index) {
      return index == 0 ? word.toLowerCase() : word.toUpperCase();
    })
    .replace(/\s+/g, '');
}

function schemaToModel(schema) {
  const entity = {};
  if (schema && schema.properties) {
    Object.keys(schema.properties).forEach((propertyName) => {
      const property = schema.properties[propertyName];
      if (property.type === 'object') {
        entity[propertyName] = schemaToModel(property);
      } else if (property.type === 'array') {
        entity[propertyName] = [];
      } else if (property.type === 'boolean') {
        entity[propertyName] = false;
      } else if (property.type === 'number' || property.type === 'integer') {
        entity[propertyName] = null;
      } else if (property.type === 'string') {
        entity[propertyName] = null;
      } else {
        entity[propertyName] = null;
      }
      if ('default' in property) {
        entity[propertyName] = property.default;
      }
    });
  }

  return entity;
}

function listToTree(list, func = null, id = 'id', parentId = 'parentId', children = 'children') {
  const tree = [];
  list.forEach((item) => {
    if (!item[parentId]) {
      tree.push(item);
    } else {
      const parent = list.find((node) => node[id] === item[parentId]);
      if (parent) {
        parent[children] = parent[children] || [];
        parent[children].push(item);
      }
    }
    if (func) {
      func(item);
    }
  });
  return tree;
}

function treeToList(tree, children = 'children', list = []) {
  tree.forEach((o) => {
    list.push(o);
    if (o.children?.length) {
      treeToList(o[children], list);
    }
  });
  return list;
}

function pathToTree(paths, sep = '/', results = []) {
  return paths.reduce((currentResults, currentPath) => {
    const pathParts = currentPath.split(sep);
    const byPath = {};
    pathParts.reduce((nodes, name, index, arr) => {
      let node = nodes.find((o) => o.name === name);
      const path = arr.slice(0, index + 1).join(sep);
      const parentPath = arr.slice(0, index).join(sep);
      if (!node) {
        node = {
          name,
          path,
          parent: byPath[parentPath],
          children: [],
        };
        nodes.push(node);
      }
      byPath[path] = node;
      return node.children;
    }, currentResults);
    return currentResults;
  }, results);
}

function getProp(instance, propPath) {
  return get(instance, propPath);
}

function getFileNameFromContentDisposition(contentDisposition) {
  const filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
  const matches = filenameRegex.exec(contentDisposition);
  if (matches != null && matches[1]) {
    return matches[1].replace(/['"]/g, '');
  }
  return null;
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

async function importModule(input) {
  const dataUri = `data:text/javascript;charset=utf-8,${encodeURIComponent(input)}`;
  const result = await import(/* @vite-ignore */ dataUri);
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
  downloadFile,
  getProp,
  importFunction,
  json,
  listToTree,
  log,
  pathToTree,
  persentFormat,
  reload,
  schemaToModel,
  treeToList,
  camelCase,
};
