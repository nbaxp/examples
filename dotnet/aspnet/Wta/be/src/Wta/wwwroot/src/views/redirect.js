import { delay } from 'utils';
import { onMounted } from 'vue';
import { useRouter } from 'vue-router';

export default {
  template: `<img class="loading" src="./src/assets/icons/loading.svg" />`,
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
