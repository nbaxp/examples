import router from "@/router/index.js";
import { defineStore } from "pinia";

import settings from "../config/settings.js";

const routes = [
	{
		path: "/",
		component: () => import("../views/components/layout/portal-layout.js"),
		meta: {
			icon: "folder",
		},
		children: [
			{
				path: "",
				component: () => import("../views/home.js"),
				meta: {
					title: "home",
					icon: "file",
				},
			},
		],
	},
	{
		path: "admin",
		component: () => import("../views/components/layout/admin-layout.js"),
		meta: {
			icon: "folder",
		},
		children: [
			{
				path: "",
				component: () => import("../views/admin/home.js"),
				meta: {
					title: "admin home",
					icon: "file",
				},
			},
		],
	},
];

const getRoutes = async () => {
	return routes;
};

export default defineStore("app", {
	state: () => ({
		settings: { ...settings },
		menu: null,
	}),
	actions: {
		async refreshMenu() {
			this.menu = await getRoutes();
			const key = "root";
			const root = router.getRoutes().find((o) => o.name === key);
			if (root) {
				router.removeRoute(root);
			}
			router.addRoute("/", { name: key, path: "/", children: this.menu });
		},
	},
});
