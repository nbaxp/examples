//https://medium.com/@jackysee/simple-loading-svg-inline-in-vue-35994f5326f2
import html from 'utils';
import { onMounted, ref } from 'vue';

const cache = new Map();

export default {
  template: html`<template v-if="name.startsWith('data:image')">
      <img :src="name" style="max-height:18px;"/>
    </template>
    <template v-else-if="name.startsWith('ep-')">
      <component :is="name" />
    </template>
    <template v-else>
      <g v-html="svg" />
    </template> `,
  props: {
    name: {
      default: 'file',
    },
  },
  setup(props) {
    const svg = ref(null);
    onMounted(async () => {
      if (!props.name.startsWith('ep-') && !props.name.startsWith('data:image')) {
        const url = `./src/assets/icons/${props.name}.svg`;
        if (!cache.has(url)) {
          try {
            cache.set(
              url,
              fetch(url).then((o) => o.text()),
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
