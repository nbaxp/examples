import useQuery from './query.js';
import useExport from './export.js';
import useImport from './import.js';
import { emailRegex, phoneNumberRegex } from '@/utils/constants.js';

const properties = {
  id: {
    hidden: true,
  },
  userName: {
    readonly: true,
    rules: [{ required: true }],
  },
  password: {
    input: 'password',
    hideInList: true,
  },
  email: {
    roles: [
      {
        pattern: emailRegex,
      },
    ],
  },
  phoneNumber: {
    roles: [
      {
        pattern: phoneNumberRegex,
      },
    ],
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
    hideInQuery:true
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
      properties: Object.assign({}, properties, {
        password: {
          rules: [
            {
              required: true,
            },
          ],
        },
      }),
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
