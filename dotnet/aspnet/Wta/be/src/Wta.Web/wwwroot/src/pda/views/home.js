import html from 'utils';

export default {
  template: html`
    <van-grid square :gutter="10">
      <van-grid-item icon="photo-o" text="文字" to="/scan" />
      <van-grid-item icon="photo-o" text="文字" />
      <van-grid-item icon="photo-o" text="文字" />
      <van-grid-item icon="photo-o" text="文字" />
    </van-grid>
  `,
};
