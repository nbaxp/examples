export default {
  name: "routerMenu",
  template: `<ul :class="{menu:!parent}">
      <li v-for="item in routes">
          <router-link :to="getFullPath(item.path)">{{item.path}}</router-link>
          <template v-if="item.children">
              <router-menu :routes="item.children" :parent="item" />
          </template>
      </li>
  </ul>`,
  props: ["routes", "parent"],
  setup(props) {
    const parent = props.parent;
    const getFullPath = (path) => {
      if (!parent) {
        return path;
      }
      if (parent.path.endsWith("/")) {
        return parent.path + path;
      }
      return parent.path + "/" + path;
    };
    return {
      getFullPath,
    };
  },
};
