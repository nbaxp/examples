import { getUrl } from '@/utils/request.js';
import useMarkdown from '@/views/md.js';
import { log } from 'utils';

const convert = (list) => {
  list.forEach((o) => {
    if (o.component) {
      if (o.isMarkdown) {
        o.component = useMarkdown(o.component);
      } else {
        const file = `../views/${o.component}.js`;
        o.component = () => import(file);
      }
    }
    if (o.children?.length) {
      convert(o.children);
    }
  });
};

async function getMenuInfo() {
  log('fetch menus');
  const response = await fetch(getUrl('menu'), { method: 'POST' });
  const result = await response.json();
  //转换格式开始
  convert(result);
  //转换格式结束
  return result;
}

export { getMenuInfo };
