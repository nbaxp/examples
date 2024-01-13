<template>
  <slot></slot>
</template>

<script setup>
  import { provide } from 'vue';
  provide('pageData', new Map());
  console.log('page data init');
</script>
