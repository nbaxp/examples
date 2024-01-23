<template>
  <el-container>
    <el-main style="display: flex; align-items: center; justify-content: center">
      <div class="login">
        <div style="display: flex; align-items: center; justify-content: center">
          <el-space><layout-logo /> <layout-locale /></el-space>
        </div>
        <el-card class="box-card">
          <el-row :gutter="40" style="width: 400px">
            <el-col>
              <el-tabs>
                <el-tab-pane :label="$t('emailRegister')">
                  <app-form v-model="emailModel" :schema="schema.properties.emailRegister" @success="success" />
                </el-tab-pane>
                <el-tab-pane :label="$t('smsRegister')">
                  <app-form v-model="smsModel" :schema="schema.properties.smsRegister" @success="success" />
                </el-tab-pane>
              </el-tabs>
              <div style="display: flex; align-items: center; justify-content: space-between; height: 50px">
                <router-link style to="/login">{{ $t('login') }}</router-link>
                <router-link style to="/forgot-password">{{ $t('forgotPassword') }}</router-link>
              </div>
            </el-col>
          </el-row>
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
  import useSchema from '@/models/register.js';
  import { schemaToModel } from '@/utils/index.js';

  const schema = useSchema();
  const emailModel = ref(schemaToModel(schema.properties.emailRegister));
  const smsModel = ref(schemaToModel(schema.properties.smsRegister));
  const router = useRouter();
  const success = (result) => {
    router.push('/login');
  };
</script>
