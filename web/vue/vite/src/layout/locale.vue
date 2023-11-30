<template>
  <el-dropdown class="cursor-pointer">
    <span class="el-dropdown-link flex">
      <el-icon :size="18">
        <i class="i-ion-language" />
      </el-icon>
    </span>
    <template #dropdown>
      <el-dropdown-menu>
        <el-dropdown-item v-for="locale in $i18n.availableLocales" :key="locale" @click="change(locale)">
          {{ options.find((o) => o.value === locale).label }}
          <el-icon v-if="locale === $i18n.locale" class="el-icon--right">
            <i class="i-ep-select" />
          </el-icon>
        </el-dropdown-item>
      </el-dropdown-menu>
    </template>
  </el-dropdown>
</template>

<script setup>
  import { useI18n } from 'vue-i18n';

  import SvgIcon from '@/components/icon/index.vue';
  import { options } from '@/locale/index.js';

  const i18n = useI18n();
  const change = (locale) => {
    i18n.locale.value = locale;
  };
</script>
