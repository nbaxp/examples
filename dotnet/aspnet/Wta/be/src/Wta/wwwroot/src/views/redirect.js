import { useRouter } from 'vue-router';

export default {
  template: `<img class="loading" src="./src/assets/icons/loading.svg" />`,
  setup() {
    const router = useRouter();
    const redirect = router.currentRoute.value.query?.redirect ?? '/';
    router.push(redirect);
  },
};
