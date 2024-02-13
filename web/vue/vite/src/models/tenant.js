import useQuery from './query.js';
import useExport from './export.js';
import useImport from './import.js';

export default function () {
  const properties = {
    id: {
      hidden: true,
    },
    name: {
      rules: [
        {
          required: true,
        },
        {
          pattern: '[^u4e00-u9fa5_a-zA-Z0-9$]+',
        },
      ],
    },
    number: {
      readonly: true,
      rules: [
        {
          required: true,
        },
        {
          pattern: '[\\w]{4,64}',
        },
      ],
    },
    disabled: {
      type: 'boolean',
    },
    permissions: {
      type: 'array',
      input: 'cascader',
      multiple: true,
      checkStrictly: true,
      label: 'path',
      url: 'permission/search',
      label: 'name',
      value: 'number',
      hideInList: true,
      hideInQuery: true,
    },
  };
  return {
    properties: {
      query: useQuery(properties),
      list: {
        properties,
      },
      details: {
        title: 'details',
        properties,
      },
      create: {
        title: 'create',
        properties,
      },
      update: {
        title: 'update',
        properties,
      },
      export: useExport(),
      import: useImport(),
    },
  };
}
