import settings from '@/config/settings.js';
import { get } from 'lodash';

export function log(message) {
  if (settings.isDebug) {
    console.log(message);
  }
}

export async function delay(ms = 500) {
  return new Promise((resolve, _) => setTimeout(resolve, ms));
}

// format html`...` by vscode lit-html
export default function html(strings, ...values) {
  let output = '';
  let index;
  for (index = 0; index < values.length; index += 1) {
    output += strings[index] + values[index];
  }
  output += strings[index];
  return output;
}

// format %
export function persentFormat(number) {
  return `${Number.parseFloat(number * 100).toFixed(2)} %`;
}
// format bytes
export function bytesFormat(bytes) {
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
export function format(template, ...args) {
  if (typeof template !== 'string') {
    throw new TypeError('Expected a string template');
  }
  if (args.length === 0) {
    return template;
  }
  return template.replace(/{(\d+)}/g, (match, number) => {
    const index = Number.parseInt(number, 10);
    const param = args[index];
    if (typeof param === 'string') {
      return param.replace(/</g, '&lt;').replace(/>/g, '&gt;');
    }
    return typeof param !== 'undefined' ? param : match;
  });
}

export function listToTree(list) {
  const tree = [];
  for (const item of list) {
    if (!item.parentId) {
      tree.push(item);
    } else {
      const parent = list.find((node) => node.id === item.parentId);
      if (parent) {
        parent.children = parent.children || [];
        parent.children.push(item);
      } else {
        tree.push(item);
      }
    }
  }
  return tree;
}

export function treeToList(tree, list = []) {
  for (const o of tree) {
    list.push(o);
    if (o.children?.length) {
      treeToList(o.children, list);
    }
  }
  return list;
}

export function getProp(instance, propPath) {
  return get(instance, propPath);
}

export function getFileName(contentDisposition) {
  return decodeURIComponent(contentDisposition.match(/UTF-8''(.+)/)[1]);
}

async function importModule(input) {
  const dataUri = `data:text/javascript;charset=utf-8,${encodeURIComponent(input)}`;
  const result = await import(dataUri /* @vite-ignore */);
  return result.default;
}

// await importFunction('()=>console.log(123)');
export async function importFunction(input) {
  const src = input ?? '()=>{}';
  const result = await importModule(`export default ${src}`);
  return result;
}

export function reload() {
  location.href = `${location.protocol}//${location.host}${location.pathname}`;
}

export function downloadFile(file, name) {
  const url = window.URL.createObjectURL(file);
  const link = document.createElement('a');
  link.href = url;
  link.setAttribute('download', name);
  document.body.appendChild(link);
  try {
    link.click();
  } finally {
    document.body.removeChild(link);
  }
}

export function traverseTree(list, func) {
  for (const node of list) {
    func(node);
    if (node.children?.length) {
      traverseTree(node.children, func);
    }
  }
}

//JSON
export function fromJSON(str) {
  return JSON.parse(str, (key, val) => {
    if (typeof val === 'string') {
      if (val.indexOf('function') === 0 || val.indexOf('async function') === 0) {
        return eval(`(${val})`);
      }
    }
    return val;
  });
}

export function toJSON(obj) {
  return JSON.stringify(obj, (key, val) => {
    if (typeof val === 'function') {
      return `${val}`;
    }
    return val;
  });
}

export function findPath(tree, predicate, path = []) {
  if (!tree) return [];
  for (let i = 0; i < tree.length; i += 1) {
    const node = tree[i];
    path.push(node);
    if (predicate(node)) return path;
    if (node.children) {
      const findChildren = findPath(node.children, predicate, path);
      if (findChildren.length) return findChildren;
    }
    path.pop();
  }
  return [];
}
