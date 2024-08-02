import { normalize } from '@/utils/schema.js';

export default function () {
  return normalize({
    type: 'object',
    title: '导出',
    properties: {
      includeAll: {
        title: '全部记录',
        type: 'boolean',
      },
      format: {
        title: '格式',
        input: 'radio',
        default: 'xlsx',
        options: [
          { label: 'excel', value: 'xlsx' },
          { label: 'csv', value: 'csv' },
        ],
      },
    },
  });
}
