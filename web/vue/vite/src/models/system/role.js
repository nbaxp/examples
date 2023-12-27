const properties = {
  name: {
    title: 'Name',
  },
  number: {
    title: 'Number',
  },
  order: {
    title: 'Display Number',
    type: 'number',
    width: 80,
  },
  isReadonly: {
    title: 'Readonly',
    type: 'boolean',
  },
  createdAt: {
    title: 'Created Time',
    input: 'datetime',
    width: 150,
  },
  // menus: {
  //   title: 'Menu',
  //   type: 'array',
  //   items: [],
  // },
};

const schema = {
  type: 'Object',
  title: 'Role',
  properties: {
    query: {
      type: 'object',
      method: 'POST',
      url: 'role',
      properties,
    },
    list: {
      type: 'object',
      properties,
    },
    export: {
      type: 'object',
      title: 'Export',
      method: 'POST',
      url: 'role/export',
      properties: {
        all: {
          title: 'Export All',
          type: 'boolean',
        },
      },
    },
    import: {
      type: 'object',
      title: 'Import',
      method: 'POST',
      url: 'role/import',
      properties: {
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
    },
    create: {
      type: 'object',
      title: 'Create',
      properties,
    },
    update: {
      type: 'object',
      title: 'Update',
      properties,
    },
  },
};

export default function () {
  return schema;
}
