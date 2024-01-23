import { emailRegex, phoneNumberRegex } from '@/utils/constants.js';

export default function () {
  const properties = {
    userName: {
      icon: 'user',
      rules: [
        {
          required: true,
        },
        {
          validator: 'remote',
          url: 'user/valid-user-name',
          message: '{0} has already used',
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
  };
  return {
    properties: {
      emailRegister: {
        url: 'user/register',
        labelWidth: 0,
        submitStyle: 'width:100%',
        properties: {
          ...properties,
          email: {
            icon: 'mail',
            rules: [
              { required: true },
              {
                type: 'email',
              },
            ],
          },
          authCode: {
            title: 'emailCode',
            icon: 'auth',
            input: 'code-captcha',
            url: 'captcha/code',
            timeout: 120,
            query: 'email',
            regexp: emailRegex,
            rules: {
              required: true,
            },
          },
        },
      },
      smsRegister: {
        url: 'user/register',
        labelWidth: 0,
        submitStyle: 'width:100%',
        properties: {
          ...properties,
          phoneNumber: {
            icon: 'mail',
            rules: [
              { required: true },
              {
                pattern: phoneNumberRegex,
              },
            ],
          },
          authCode: {
            title: 'smsCode',
            icon: 'auth',
            input: 'code-captcha',
            url: 'captcha/code',
            timeout: 120,
            query: 'phoneNumber',
            regexp: phoneNumberRegex,
            rules: {
              required: true,
            },
          },
        },
      },
    },
  };
}
