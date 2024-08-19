import { normalize } from '@/utils/schema.js';

export default function () {
  return normalize({
    type: 'object',
    title: '导入',
    width: '500px',
    properties: {
      update: {
        title: '已存在',
        input: 'radio',
        default: 'skip',
        options: [
          { label: '跳过', value: 'skip' },
          { label: '更新', value: 'update' },
        ],
      },
      file: {
        title: '文件',
        input: 'file',
        accept: '.xlsx',
        limit: 1,
        size: 100 * 1024 * 1024,
        rules: [
          {
            validator: 'file',
          },
        ],
      },
    },
  });
}
