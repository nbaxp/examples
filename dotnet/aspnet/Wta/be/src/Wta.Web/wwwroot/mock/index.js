import settings from '@/config/settings.js';
import Mock from '~/lib/better-mock/mock.browser.esm.js';
import useFile from './file.js';
import useLocale from './locale.js';
import useMenu from './menu.js';
import useToken from './token.js';
import useUser from './user.js';

Mock.setup({
  timeout: '200-600',
});

export default function () {
  if (!settings.useMock) {
    return;
  }
  console.debug('init mock');
  useLocale();
  useToken();
  useUser();
  useMenu();
  useFile();
}
