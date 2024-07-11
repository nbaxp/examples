import { createApp } from "vue";
import { createRouter, createWebHashHistory } from "vue-router";
import mixin from "./mixin.js";

const appComponent = { template: "<router-view></router-view>" };

const template = "<pre>{{JSON.stringify($router.getRoutes(),null,2)}}</pre>";

const layout1 = {
    template: `layout1:<router-view></router-view>`
};

const layout2 = {
    template: `layout2:<router-view></router-view>`
};

const routes = [
    {
        path:'',
        meta:{
            title:'root'
        },
        children:[
            {
                path:'/',
                component:{
                    template:"<router-view></router-view>"
                }
            }
        ]
    },
    {
        path:'/login',
        component:{
            template
        }
    }
    // {
    //     name: 'root1',
    //     path: "/",
    //     component: layout1,
    //     meta: {
    //         title: "门户"
    //     },
    //     children: [
    //         {
    //             path: "",
    //             component: {
    //                 template: `<h1>Home</h1><img class="loading" src="./src/loading.svg" />`
    //             }
    //         },
    //         {
    //             path: "about",
    //             component: {
    //                 template: "<h1>About</h1>"
    //             }
    //         }
    //     ]
    // },
    // {
    //     name: 'root2',
    //     path: "/doc",
    //     component: layout2,
    //     meta: {
    //         title: "文档"
    //     },
    //     children: [
    //         {
    //             path: "",
    //             component: {
    //                 template: "<h1>Doc Home</h1>"
    //             }
    //         },
    //         {
    //             path: "about",
    //             component: {
    //                 template: "<h1>Doc About</h1>"
    //             }
    //         }
    //     ]
    // },
];


const app = createApp(appComponent);

app.mixin(mixin);

const router = createRouter({
    history: createWebHashHistory(),
    routes,
});

app.use(router);

app.mount("#app");

