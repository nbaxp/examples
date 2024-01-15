import useQuery from './query.js';
import useExport from './export.js';
import useImport from './import.js';

const properties = {
  id: {
    hidden: true,
  },
  path: {
    width: 300,
  },
  order: {},
  // order: {
  //   title: 'Display Number',
  //   type: 'number',
  //   width: 80,
  // },
  // isReadonly: {
  //   title: 'Readonly',
  //   type: 'boolean',
  // },
  // createdAt: {
  //   title: 'Created Time',
  //   input: 'datetime',
  //   width: 150,
  // },
  // menus: {
  //   title: 'Menu',
  //   type: 'array',
  //   items: [],
  // },
};

const schema = {
  properties: {
    query: useQuery(properties, true, 'order'),
    list: {
      isTree: true,
      key: 'path',
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
