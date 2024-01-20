import useQuery from './query.js';
import useExport from './export.js';
import useImport from './import.js';

const properties = {
  id: {
    hidden: true,
  },
  userName: {
    readonly: true,
  },
  password: {
    input: 'password',
  },
  avatar: {
    input: 'upload',
    isImage: true,
    url: 'file/upload',
    accept: '.svg,.png',
  },
  departmentId: {
    input: 'cascader',
    checkStrictly: true,
    url: 'department/search',
  },
  roles: {
    type: 'array',
    input: 'select',
    multiple: true,
    url: 'role/search',
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
