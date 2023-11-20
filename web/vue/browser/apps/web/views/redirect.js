import { useRouter } from "vue-router";

export default {
  template: `redirect`,
  setup() {
    const router = useRouter();
    const redirect = router.currentRoute.value.query?.redirect ?? "/";
    router.push(redirect);
  },
};
