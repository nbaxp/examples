import Mock from '../lib/better-mock/mock.browser.esm.js';

export default function () {
  Mock.mock('/api/locale', 'POST', (request) => {
    return {};
  });
}
