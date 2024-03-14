import { defineAsyncComponent } from "vue";
import { createRouter, createWebHashHistory } from "vue-router";
import { useTitle } from "@vueuse/core";
import NProgress from "../lib/nprogress/nprogress.vite-esm.js";
import { isLogin, hasPermission } from "../api/user.js";
import { useAppStore } from "../store/index.js";
import { listToTree } from "../utils/index.js";
import { connection, connect, connectionId } from "../signalr/index.js";
import remoteRoutes from "./routes.js";

NProgress.configure({ showSpinner: false });

const routes = [
  {
    path: "/login",
    component: () => import("../views/login.js"),
    meta: {
      title: "登录",
      isHidden: true,
    },
  },
  {
    path: "/account",
    component: () => import("../views/account.js"),
    meta: {
      title: "用户中心",
      isHidden: true,
    },
  },
  {
    path: "/403",
    component: () => import("../views/403.js"),
    meta: {
      title: "权限不足",
      isHidden: true,
    },
  },
  {
    path: "/:pathMatch(.*)*",
    component: () => import("../views/404.js"),
    meta: {
      title: "无法找到",
      isHidden: true,
    },
  },
];

const router = createRouter({
  history: createWebHashHistory(),
  routes,
});

router.beforeEach(async (to, from, next) => {
  NProgress.start();
  try {
    if (to.path !== "/login") {
      if (!(await isLogin())) {
        next({ path: "/login", query: { redirect: to.fullPath } });
      } else {
        // 注释以下代码暂停权限验证
        // if (!to.meta.public && to.meta.hasPermission === false) {
        //   next({ path: "/403", query: { redirect: to.fullPath } });
        // } else {
        //   next();
        // }
        // 添加一下代码暂停权限验证
        next();
      }
    } else {
      next();
    }
  } catch (error) {
    NProgress.done();
  }
});

router.afterEach((to) => {
  try {
    if (!to.meta.isHidden) {
      const appStore = useAppStore();
      appStore.add(to);
    }
    if (to.meta.title) {
      useTitle().value = `${to.meta.title}`;
    }
    to.meta.cache = new Map();
  } finally {
    NProgress.done();
  }
});

const refreshRouter = async () => {
  await connect();
  const appStore = useAppStore();
  const permissions = appStore.user.permissions;
  const serverRoutes = Object.assign([], remoteRoutes);
  if (appStore.user.roles.some((o) => o === "物流")) {
    const baseDataRoutes = serverRoutes.find((o) => o.path === "base-data");
    baseDataRoutes.children = baseDataRoutes.children.filter((o) => o.path !== "bei-jian" && o.path !== "xiao-shou");
  }
  const setPermission = (list, parent = null) => {
    list.forEach((o) => {
      // full path
      o.meta.path = `${parent === null ? "/" : parent.meta.path + "/"}${o.path}`;
      // full name
      o.meta.fullName = `${parent === null ? "" : parent.meta.title + " > "}${o.meta.title}`;
      // permission
      if (o.meta.type === "page" || o.meta.type === "button") {
        if (!o.meta.public) {
          o.meta.hasPermission = !!permissions[o.meta.permission];
        }
      }
      // component
      if (o.meta?.type === "page") {
        if (!o.component) {
          o.component = o.meta.path ?? o.path;
        }
        if (o.component.constructor === String) {
          const name = o.component;
          o.component = async () => {
            const module = await import(`../views${name}.js`);
            module.default.name = name;
            return module.default;
          };
        }
      }
      // children
      if (o.children?.length) {
        setPermission(o.children, o);
        if (o.meta.type === "page") {
          o.meta.children = o.children;
          delete o.children;
        }
      }
    });
  };
  setPermission(serverRoutes);
  if (router.hasRoute("layout")) {
    router.removeRoute("layout");
  }
  const layout = {
    name: "layout",
    path: "/",
    redirect: "/home",
    component: () => import("../layouts/index.js"),
    children: serverRoutes,
  };
  router.addRoute("/", layout);
};
export default router;
export { refreshRouter };
