import useQuery from './query.js';
import useExport from './export.js';
import useImport from './import.js';

const properties = {
  id: {
    hidden: true,
  },
  name: {
    rules: [
      {
        required: true,
      },
    ],
  },
  number: {
    readonly: true,
    rules: [
      {
        required: true,
      },
    ],
  },
  parentId: {
    type: 'string',
    input: 'cascader',
    checkStrictly: true,
    url: 'dict/search',
    hideInList: true,
  },
};

const schema = {
  properties: {
    query: useQuery(properties, true),
    list: {
      isTree: true,
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

export default function () {
  return schema;
}
