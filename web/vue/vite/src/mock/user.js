import { jwtDecode } from 'jwt-decode';

import Mock from '@/lib/better-mock/mock.browser.esm.js';
import { log } from '@/utils/index.js';

export default function () {
  Mock.mock('/api/user/info', 'POST', (request) => {
    log(`mock:${request.url}`);
    const token = request.headers.authorization.split(' ')[1];
    const { exp, user } = jwtDecode(token);
    if (new Date(exp * 1000) < new Date()) {
      return { _status: 401 };
    }
    const result = {
      userName: user,
      avatar: '/assets/icons/avatar.svg',
      permissions: ['user'],
    };
    return JSON.parse(JSON.stringify(result));
  });
}
