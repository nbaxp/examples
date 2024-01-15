import useQuery from './query.js';
import useExport from './export.js';
import useImport from './import.js';

const properties = {
  id: {
    hidden: true,
  },
  userName: {},
  roles: {
    type: 'array',
    input: 'select',
    multiple: true,
    url: 'role/search',
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
