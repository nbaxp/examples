import { defineStore } from 'pinia';

export default defineStore('tabs', {
  state: () => ({
    routes: [],
    isRefreshing: false,
  }),
  actions: {
    addRoute(route) {
      if (route.fullPath !== '/home') {
        if (!this.routes.find((o) => o.name === route.name)) {
          this.routes.push(route);
        } else {
          const index = this.routes.findIndex((o) => o.name === route.name);
          this.routes[index] = route;
        }
      }
    },
    clear() {
      this.$reset();
    },
  },
});
