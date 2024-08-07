import uuidv7 from '~/lib/uuid/v7.js';
export function schemaToModel(schema, isQueryForm = false) {
  if (!schema) {
    return null;
  }
  const result = {};
  const entries = Object.entries(schema.properties).sort((a, b) => (a[1]?.order ?? 0) - (b[1]?.order ?? 0));
  for (const [propertyName, property] of entries) {
    const type = property.type;
    let value = null;
    if (type === 'object') {
      value = null; // schemaToModel(property, isQueryForm);
    } else if (!isQueryForm) {
      if (property.default) {
        value = property.default;
      } else {
        if (property.type === 'boolean') {
          if (!property.meta?.isNullable) {
            value = false;
          }
        } else if (property.meta?.required) {
          if (type === 'array') {
            value = [];
          } else if (property.type === 'number') {
            if (property.meta?.options) {
              value = property.meta.options[0].value;
            } else {
              value = 0;
            }
          } else if (type === 'string') {
            if (property.meta?.hidden) {
              if (property.meta?.format === 'datetime') {
                value = new Date().toISOString();
              } else {
                value = uuidv7();
              }
            }
          }
        }
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

export function toQuerySchema(schema) {
  const result = JSON.parse(JSON.stringify(schema));
  const keys = Object.keys(result.properties);
  for (const key of keys) {
    const property = result.properties[key];
    const input = property.input;
    if (input === 'date') {
      property.input = 'daterange';
    } else if (input === 'datetime') {
      property.input = 'datetimerange';
    }
  }
  return result;
}
