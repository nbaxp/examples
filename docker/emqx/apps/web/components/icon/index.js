//https://medium.com/@jackysee/simple-loading-svg-inline-in-vue-35994f5326f2
import html from "utils";
import { onMounted, ref } from "vue";

const cache = new Map();

export default {
  template: html`<template v-if="name.indexOf('ep-')===0">
      <component :is="name" />
    </template>
    <template v-else>
      <g v-html="svg" />
    </template> `,
  props: {
    name: {
      default: "file",
    },
  },
  setup(props) {
    const svg = ref(null);
    onMounted(async () => {
      if (!props.name.startsWith("ep-")) {
        const url = `./assets/icons/${props.name}.svg`;
        if (!cache.has(url)) {
          try {
            cache.set(
              url,
              fetch(url).then((o) => o.text())
            );
          } catch (error) {
            console.log(error);
            cache.delete(url);
          }
        }
        svg.value = await cache.get(url);
      }
    });
    return {
      svg,
    };
  },
};
