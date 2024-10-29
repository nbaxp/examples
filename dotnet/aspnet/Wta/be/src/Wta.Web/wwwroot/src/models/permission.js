import useExport from '@/models/export.js';
import useImport from '@/models/import.js';
import useQuery from '@/models/query.js';

const properties = {
  id: {
    hidden: true,
  },
  name: { title: '名称' },
  number: { title: '编号' },
  icon: {
    name: { title: '图标' },
    input: 'icon',
  },
  order: { title: '序号' },
  disabled: {
    title: '禁用',
    type: 'boolean',
  },
  parentId: {
    title: '上级',
  },
};

const schema = {
  properties: {
    query: useQuery(properties, true, 'order'),
    list: {
      isTree: true,
      key: 'name',
      properties,
    },
    details: {
      title: '详情',
      properties,
    },
    create: {
      title: '新建',
      properties,
    },
    update: {
      title: '更新',
      properties,
    },
    export: useExport(),
    import: useImport(),
  },
};

export default function () {
  const config = {
    query: {
      schema: {},
    },
    table: {
      schema: {},
    },
    edit: {
      schema: {},
    },
    export: {
      schema,
    },
    import: {
      schema,
    },
  };
  return config;
}
