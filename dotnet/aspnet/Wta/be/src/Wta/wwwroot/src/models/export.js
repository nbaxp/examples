import { normalize } from '@/utils/schema.js';

export default function () {
  return normalize({
    type: 'object',
    title: '导出',
    properties: {
      exportAll: {
        title: '全部记录',
        type: 'boolean',
      },
      format: {
        title: 'csv格式',
        type: 'boolean',
      },
    },
  });
}
