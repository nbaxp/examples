import html from 'utils';

export default {
  template: html`
    <el-row class="breadcrumb flex1">
      <el-breadcrumb>
        <template v-for="item in $route.matched">
          <el-breadcrumb-item v-if="item.meta.title" :to="{ path: item.path }">
            <span :title="item.path">{{ $t(item.meta?.title??'title')}}</span>
          </el-breadcrumb-item>
        </template>
      </el-breadcrumb>
    </el-row>
  `,
};
