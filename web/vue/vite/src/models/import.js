export default function () {
  return {
    type: 'object',
    title: 'import',
    properties: {
      update: {
        type: 'boolean',
      },
      files: {
        title: 'Files',
        type: 'array',
        multiple: true,
        input: 'file',
        accept: '.xlsx',
        default: [],
        limit: 10,
        size: 100 * 1024 * 1024,
        rules: [
          {
            required: true,
            trigger: 'change',
          },
        ],
      },
    },
  };
}
