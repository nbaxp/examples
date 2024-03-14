import html from "html";
import { ElConfigProvider } from "element-plus";
import zh from "./lib/element-plus/locale/zh-cn.min.mjs";
import en from "./lib/element-plus/locale/en.min.mjs";
import { reactive, onMounted, onUnmounted } from "vue";
import { ElNotification } from "element-plus";
import { dayjs } from "element-plus";

export default {
  components: { ElConfigProvider },
  template: html`
    <el-config-provider :locale="localeMap.get($i18n.locale)" size="small">
      <router-view></router-view>
    </el-config-provider>
  `,
  setup() {
    const localeMap = reactive(
      new Map([
        ["zh", zh],
        ["en", en],
      ])
    );
    const event = "Refresh";
    onMounted(() => {
      document.querySelector("#loading.loading").classList.remove("loading");
      PubSub.subscribe(event, async () => {
        await ElNotification.closeAll();
        ElNotification({
          type: "warning",
          title: `${dayjs(new Date()).format("YYYY-MM-DD HH:mm:ss")}`,
          dangerouslyUseHTMLString: true,
          message: `<a href="javascript:location.reload(true)">站点已更新，点击刷新!</a>`,
        });
      });
    });
    onUnmounted(() => PubSub.unsubscribe(event));
    return {
      localeMap,
    };
  },
};
