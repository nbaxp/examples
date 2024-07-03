# vue select 联动

关键点：配置下级的 dependsOn ，watch 上级的数据变化来设定下级的 options

1. 假设 modle.prop1  -> model.prop2 联动
1. 在 props2 的 schema 上配置 dependsOn = prop1
1. 在 select 组件上 watch props1 的变动，props1 有值时，初始化prop2 的 options,否则置空 []

```js
watch(
   () => model[props.schema.dependsOn],
   async (value) => {
    if (props.schema.options) {
     options.value = props.schema.options;
    } else if (props.schema.url) {
     if (
      !props.schema.dependsOn ||
      model[props.schema.dependsOn] ||
      model[props.schema.dependsOn] === false
     ) {
      await fetchOptions();
     } else {
      options.value = [];
     }
    }
   },
   { immediate: true },
  );
```

```js
const fetchOptions = async () => {
   route.meta.cache ||= new Map();
   const map = route.meta.cache;
   const url = `${props.schema.url}`;
   let postData = props.schema.data;
   if (props.schema.data instanceof Function) {
    postData = props.schema.data(model[props.schema.dependsOn]);
   }
   const key = JSON.stringify({
    url,
    postData,
   });
   options.value = map.get(key);
   if (!options.value) {
    const method = props.schema.method || "post";
    const data = (await request(method, url, postData)).data;
    if (!data.error) {
     options.value = (
      data.result.records ??
      data.result ??
      getProp(data, props.schema.path)
     ).map((o) => {
      if (Array.isArray(o)) {
       return {
        value: o[0],
        label: o[1],
       };
      } else if (o instanceof Object) {
       return {
        value: o[props.schema.value ?? "value"],
        label: o[props.schema.label ?? "label"],
       };
      } else {
       return {
        value: o,
        label: o,
       };
      }
     });
     map.set(url, options.value);
    } else {
     options.value = [];
    }
    if (props.schema.selected && options.value.length) {
     model[props.prop] = options.value[0].value;
    }
   }
  };
```
