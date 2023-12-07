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
      api: 'role',
      properties,
    },
    list: {
      type: 'object',
      properties,
    },
    edit: {
      type: 'object',
      properties,
    },
  },
};

export default function () {
  return schema;
}
