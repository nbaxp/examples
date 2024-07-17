export function schemaToModel(schema, isQueryForm = false) {
  if (!schema) {
    return null;
  }
  const result = {};
  for (const propertyName in schema.properties) {
    const property = schema.properties[propertyName];
    const type = property.type;
    let value = null;
    if (type === 'object') {
      value = schemaToModel(property);
    } else if (!isQueryForm) {
      if ('default' in property) {
        value = property.default;
      } else if (type === 'array') {
        value = [];
      } else if (property.type === 'boolean') {
        value = false;
      } else if (property.type === 'number') {
        value = 0;
      }
    }
    result[propertyName] = value;
  }
  return result;
}

export function normalize(schema) {
  if (!schema) {
    return null;
  }
  const { title, type = 'string', properties = {}, input, default: defaultValue, rules, ...meta } = schema;
  const result = {
    title,
    type,
    properties,
    input,
    default: defaultValue,
    rules,
    meta,
  };
  if (!input) {
    if (type === 'number') {
      result.input = 'number';
    } else if (type === 'boolean') {
      result.input = 'switch';
    } else {
      result.input = 'text';
    }
  }
  for (const propertyName in properties) {
    properties[propertyName] = normalize(properties[propertyName]);
  }
  return result;
}
