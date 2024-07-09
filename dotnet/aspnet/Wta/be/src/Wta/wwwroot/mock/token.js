import Mock from '../../../lib/better-mock/mock.browser.esm.js';
import * as jose from '../../../lib/jose/index.js';

const publicJsonWebKey = {
  crv: 'P-256',
  ext: true,
  key_ops: ['verify'],
  kty: 'EC',
  x: '-JaTLMzlNj6a4DFNHbeXfEOimiqmB1SV-lVyW6CZ0Cs',
  y: 'H8FwD_z3eLs5X3l0_JZnuIgSFf1kNbWNAa1yuDfcvJ8',
};

const privateJsonWebKey = {
  crv: 'P-256',
  d: 'E40lOv52Czs0F3Y1TUPWVtZe2nWKuLOM2Igd86mpQSg',
  ext: true,
  key_ops: ['sign'],
  kty: 'EC',
  x: '-JaTLMzlNj6a4DFNHbeXfEOimiqmB1SV-lVyW6CZ0Cs',
  y: 'H8FwD_z3eLs5X3l0_JZnuIgSFf1kNbWNAa1yuDfcvJ8',
};

const publicKey = await window.crypto.subtle.importKey(
  'jwk',
  publicJsonWebKey,
  {
    name: 'ECDSA',
    namedCurve: 'P-256',
  },
  true,
  ['verify'],
);
const privateKey = await window.crypto.subtle.importKey(
  'jwk',
  privateJsonWebKey,
  {
    name: 'ECDSA',
    namedCurve: 'P-256',
  },
  true,
  ['sign'],
);

const issuer = 'urn:example:issuer'; //发行方
const audience = 'urn:example:audience'; //接收方
const accessTokenTimeout = '1m';
const refreshTokenTimeout = '24m';

async function createToken(claims, timeout) {
  const jwt = await new jose.SignJWT(claims)
    .setProtectedHeader({ alg: 'ES256' })
    .setIssuedAt()
    .setIssuer(issuer)
    .setAudience(audience)
    .setExpirationTime(timeout)
    .sign(privateKey);
  return jwt;
}

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
        Promise.all([createToken(claims, accessTokenTimeout), createToken(claims, refreshTokenTimeout)]).then(
          (results) => {
            const [access_token, refresh_token] = results;
            resolve({
              access_token,
              refresh_token,
            });
          },
        );
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
    return new Promise((resolve) => {
      jose
        .jwtVerify(jwt, publicKey, { issuer, audience })
        .then((result) => {
          const claims = { user: 'admin' };
          Promise.all([createToken(claims, accessTokenTimeout), createToken(claims, refreshTokenTimeout)]).then(
            (results) => {
              const [access_token, refresh_token] = results;
              resolve({
                access_token,
                refresh_token,
              });
            },
          );
        })
        .catch((error) => {
          resolve({ _status: 400, code: 400, message: 'refresh_token 已过期' });
        });
    });
  });
}
