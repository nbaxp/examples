import { useTabsStore } from '@/store/index.js';
import html from 'utils';
import { getCurrentInstance, nextTick, ref, watchEffect } from 'vue';
import { onBeforeRouteUpdate, useRoute, useRouter } from 'vue-router';

export default {
  template: html`<pre>{{$router.getRoutes()}}</pre>`,
  setup() {
    const tabsStore = useTabsStore();
    const itemRefs = ref([]);
    const currentRoute = useRoute();
    const router = useRouter();
    const model = ref('');

    watchEffect(() => {
      model.value = currentRoute.fullPath;
    });

    const setRef = (index, el) => {
      if (el) {
        itemRefs.value[index] = el;
      } else {
        itemRefs.value.splice(index, 1);
      }
    };
    const showContextMenu = (index, show) => {
      if (show) {
        itemRefs.value.forEach((item, i) => {
          if (i !== index) {
            item?.handleClose();
          }
        });
      }
    };

    const refresh = (index) => {
      const currentIndex = tabsStore.routes.findIndex((o) => o.fullPath === currentRoute.fullPath);
      const route = tabsStore.routes[index];
      if (index !== currentIndex) {
        router.push({ path: route.fullPath });
      }
      tabsStore.isRefreshing = true;
      nextTick(() => {
        tabsStore.isRefreshing = false;
      });
    };

    const deleteItem = (start, end) => {
      tabsStore.routes.splice(start, end);
    };

    const remove = (name) => {
      if (tabsStore.routes.length > 1) {
        const index = tabsStore.routes.findIndex((o) => o.fullPath === name);
        const currentIndex = tabsStore.routes.findIndex((o) => o.fullPath === currentRoute.fullPath);
        deleteItem(index, 1);
        if (index === currentIndex) {
          if (tabsStore.routes[index]) {
            router.push(tabsStore.routes[index]);
          } else {
            router.push(tabsStore.routes[index - 1]);
          }
        }
      }
    };

    const removeLeft = (index) => {
      const currentIndex = tabsStore.routes.findIndex((o) => o.fullPath === currentRoute.fullPath);
      const route = tabsStore.routes[index];
      deleteItem(0, index);
      if (currentIndex < index) {
        router.push(route);
      }
    };

    const removeRight = (index) => {
      const currentIndex = tabsStore.routes.findIndex((o) => o.fullPath === currentRoute.fullPath);
      deleteItem(index + 1, tabsStore.routes.length - index);
      if (currentIndex > index) {
        router.push(tabsStore.routes[index]);
      }
    };

    const removeOthers = (index) => {
      removeRight(index);
      removeLeft(index);
      if (tabsStore.routes[0].fullPath !== currentRoute.fullPath) {
        router.push(tabsStore.routes[0]);
      }
    };

    const onClick = (context) => {
      if (!context.active) {
        router.push(context.props.name);
      }
    };
    return {
      model,
      tabsStore,
      itemRefs,
      onBeforeRouteUpdate,
      setRef,
      showContextMenu,
      refresh,
      remove,
      removeLeft,
      removeRight,
      removeOthers,
      onClick,
    };
  },
};
