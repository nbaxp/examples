import { useRoute, useRouter } from "vue-router";

export default {
    mounted() {
        const route = useRoute();
        const router = useRouter();
        // console.log('route:');
        // console.log(route);
        // console.log('matched:');
        // console.log(route.matched);
        // console.log('router:');
        // console.log(router);
        document.title = route.meta?.title ?? '';
    }
}