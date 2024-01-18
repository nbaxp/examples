import useQuery from './query.js';
import useExport from './export.js';
import useImport from './import.js';

const properties = {
  id: {
    hidden: true,
  },
  name: {},
  number: {},
  permissions: {
    type: 'array',
    input: 'cascader',
    multiple: true,
    checkStrictly: true,
    label: 'path',
    url: 'permission/search',
    hideInList: true,
  },
};

const schema = {
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

export default function () {
  return schema;
}
