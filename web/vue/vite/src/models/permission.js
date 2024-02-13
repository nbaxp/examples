import useQuery from './query.js';
import useExport from './export.js';
import useImport from './import.js';

const properties = {
  id: {
    hidden: true,
  },
  number: {},
  name: {},
  icon: {
    input: 'icon',
  },
  order: {},
  disabled: {
    type: 'boolean',
  },
  parentId: {},
};

const schema = {
  properties: {
    query: useQuery(properties, true, 'order'),
    list: {
      isTree: true,
      key: 'number',
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
