import Mock from '~/lib/better-mock/mock.browser.esm.js';
import CryptoJS from '~/lib/crypto-js.js';
import * as jose from '~/lib/jose/index.js';

const issuer = 'urn:example:issuer'; //发行方
const audience = 'urn:example:audience'; //接收方
const accessTokenTimeout = 1;
const refreshTokenTimeout = 24;

const base64UrlEncode = (str) => {
  const encodedSource = CryptoJS.enc.Base64.stringify(str);
  const reg = /\//g;
  return encodedSource.replace(/=+$/, '').replace(/\+/g, '-').replace(reg, '_');
};

const createToken = (claims = { user: 'admin' }, minutes = 1, secretSalt = '123456') => {
  const header = JSON.stringify({
    alg: 'HS256',
    typ: 'JWT',
    issuer,
    audience,
  });

  const iat = new Date().getTime();
  const exp = iat + 2 * 60 * minutes;

  const payload = JSON.stringify({
    ...claims,
    iat,
    exp,
  });

  const before_sign = `${base64UrlEncode(CryptoJS.enc.Utf8.parse(header))}.${base64UrlEncode(CryptoJS.enc.Utf8.parse(payload))}`;
  const signature = CryptoJS.HmacSHA256(before_sign, secretSalt);
  const signatureBase64 = base64UrlEncode(signature);

  return `${before_sign}.${signatureBase64}`;
};

export default function () {
  Mock.mock('/api/token/create', 'POST', (request) => {
    const { userName, password } = JSON.parse(request.body ?? '{}');

    if (!userName) {
      return { code: 400, errors: { userName: ['用户名不能为空'] } };
    }
    if (!password) {
      return { code: 400, message: '密码不能为空' };
    }
    if (userName === 'admin' && password === '123456') {
      const claims = { user: userName };
      return new Promise((resolve) => {
        resolve({
          access_token: createToken(claims, accessTokenTimeout),
          refresh_token: createToken(claims, refreshTokenTimeout),
        });
      });
    }
    return {
      code: 400,
      message: '用户名或密码错误',
      data: { '': '用户名或密码错误' },
    };
  });

  Mock.mock('/api/token/refresh', 'POST', (request) => {
    const jwt = JSON.parse(request.body);
    const claims = { user: 'admin' };
    return new Promise((resolve) => {
      resolve({
        access_token: createToken(claims, accessTokenTimeout),
        refresh_token: createToken(claims, refreshTokenTimeout),
      });
    });
  });
}
