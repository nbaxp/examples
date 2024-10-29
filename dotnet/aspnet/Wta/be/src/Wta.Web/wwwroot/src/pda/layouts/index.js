import html from 'utils';

export default {
  template: html`
    <van-nav-bar title="标题" left-text="返回" right-text="按钮" left-arrow @click-left="back" @click-right="setting" />
    <router-view></router-view>
    <van-tabbar v-model="$route.matched[0].path">
      <van-tabbar-item icon="home-o" to="/">首页</van-tabbar-item>
      <van-tabbar-item icon="search">标签</van-tabbar-item>
      <van-tabbar-item icon="friends-o">标签</van-tabbar-item>
      <van-tabbar-item icon="setting-o">标签</van-tabbar-item>
    </van-tabbar>
  `,
  setup() {
    const back = () => history.back();
    const setting = () => history.back();
    return {
      back,
      setting,
    };
  },
};
