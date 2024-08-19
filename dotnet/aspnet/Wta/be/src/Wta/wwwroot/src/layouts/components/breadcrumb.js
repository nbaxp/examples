import html from 'utils';
import { useRouter } from 'vue-router';

export default {
  template: html`
    <el-row class="breadcrumb flex1">
      <el-breadcrumb>
        <template v-for="item in $route.matched">
          <el-breadcrumb-item v-if="!item.meta?.hideInMenu" :to="{ path: item.path }">
            <span :title="item.path">{{ getTitle(item) }}</span>
          </el-breadcrumb-item>
        </template>
      </el-breadcrumb>
    </el-row>
  `,
  setup() {
    const router = useRouter();
    const getTitle = (route) => {
      if (route.redirect) {
        return router.getRoutes().find((o) => o.path === route.redirect)?.meta?.title;
      }
      return route.meta?.title;
    };
    return {
      getTitle,
    };
  },
};
