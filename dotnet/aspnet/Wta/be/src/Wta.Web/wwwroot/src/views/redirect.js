import { delay } from 'utils';
import { onMounted } from 'vue';
import { useRouter } from 'vue-router';

export default {
  template: `<div class="el-loading-mask">
  <div class="el-loading-spinner">
    <svg class="circular" viewBox="0 0 50 50">
      <circle class="path" cx="25" cy="25" r="20" fill="none"></circle>
    </svg>
    <p class="el-loading-text">Loading...</p>
  </div>
</div>`,
  setup() {
    const router = useRouter();
    const redirect = router.currentRoute.value.query?.redirect ?? '/';

    onMounted(async () => {
      if (!redirect.startsWith('/redirect')) {
        await delay(500);
        router.push(redirect);
      }
    });
  },
};
