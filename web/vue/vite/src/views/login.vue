<template>
  <el-container>
    <el-main class="flex">
      <div>
        <div class="flex items-center justify-center">
          <layout-logo />
          <layout-locale />
        </div>
        <el-card class="box-card">
          <app-form v-model="model" :schema="schema" @success="success" />
        </el-card>
        <layout-footer />
      </div>
    </el-main>
  </el-container>
</template>

<script setup>
  import { ref } from 'vue';
  import { useRouter } from 'vue-router';

  import AppForm from '@/components/form/index.vue';
  import LayoutFooter from '@/layout/footer.vue';
  import LayoutLocale from '@/layout/locale.vue';
  import LayoutLogo from '@/layout/logo.vue';
  import useLoginModel from '@/models/login.js';
  import { useTokenStore, useUserStore } from '@/store/index.js';
  import { schemaToModel } from '@/utils/index.js';

  const schema = ref(useLoginModel());
  const model = ref(schemaToModel(schema.value));
  model.value.userName = 'admin';
  model.value.password = '123456';
  const router = useRouter();
  const success = async (data) => {
    const tokenStore = useTokenStore();
    tokenStore.setToken(data.access_token, data.refresh_token);
    const userStore = useUserStore();
    await userStore.getUserInfo();
    const redirect = router.currentRoute.value.query?.redirect ?? '/';
    router.push(redirect);
  };
</script>

<style scoped>
  .el-main {
    display: flex;
    align-items: center;
    justify-content: center;
  }
</style>
