import { emailOrPhoneNumberRegex } from '@/utils/constants.js';

export default function () {
  return {
    url: 'user/forgot-password',
    labelWidth: 0,
    submitStyle: 'width:100%',
    properties: {
      emailOrPhoneNumber: {
        rules: [
          {
            required: true,
          },
          {
            pattern: emailOrPhoneNumberRegex,
          },
          {
            validator: 'remote',
            url: 'user/has-email-or-phone-number',
            message: '{0} not exist',
          },
        ],
      },
      authCode: {
        title: 'authCode',
        icon: 'auth',
        input: 'code-captcha',
        url: 'captcha/code',
        timeout: 120,
        query: 'emailOrPhoneNumber',
        regexp: emailOrPhoneNumberRegex,
        rules: [
          {
            required: true,
          },
        ],
      },
      password: {
        input: 'password',
        icon: 'password',
        rules: [
          {
            required: true,
          },
        ],
      },
      confirmPassword: {
        input: 'password',
        icon: 'password',
        rules: [
          {
            validator: 'compare',
            compare: 'password',
          },
        ],
      },
      codeHash: {
        hidden: true,
      },
    },
  };
}
