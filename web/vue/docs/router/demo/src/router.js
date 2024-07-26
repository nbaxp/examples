import { createRouter, createWebHashHistory, useRouter } from "vue-router";
import RouterMenu from "./menu.js";

const layout = {
  components: { RouterMenu },
  template: /* html */ `<div class="container">
  <div>menu:</div>
  <router-menu :routes="routes" />
  <div>bread:<router-link v-for="item in $route.matched" :to=""></router-link></div>
  <router-view></router-view>
</div>`,
  setup() {
    const router = useRouter();
    const routes = router
      .getRoutes()
      .filter((o) => o.path.lastIndexOf("/") === 0 && o.redirect);
    console.log("menus:");
    console.log(routes);
    return {
      routes: routes,
    };
  },
};

const component = {
  template: /* html */ `<div>url:{{$route.fullPath}}</div>`,
};

const routes = [
  {
    path: "/",
    redirect: "/home",
    component: layout,
    children: [
      {
        path: "/home",
        component,
      },
    ],
  },
  {
    path: "/admin",
    component: layout,
    redirect: "/admin/home",
    children: [
      {
        path: "home",
        component,
      },
      {
        path: "system",
        redirect: "/admin/system/menu",
        children: [
          {
            path: "menu",
            component,
          },
        ],
      },
    ],
  },
];

const router = createRouter({
  history: createWebHashHistory(),
  routes,
});

export default router;
