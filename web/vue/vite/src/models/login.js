export default function () {
  return {
    properties: {
      passwordLogin: {
        title: 'login',
        url: 'token/create',
        labelWidth: 0,
        submitStyle: 'width:100%',
        properties: {
          tenantNumber: {
            //hidden: true,
            input: 'select',
            url: 'tenant/search',
            value: 'number',
          },
          userName: {
            icon: 'user',
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
          authCode: {
            input: 'image-captcha',
            url: 'captcha/image',
            rules: [
              {
                required: true,
              },
            ],
          },
          codeHash: {
            hidden: true,
          },
          rememberMe: {
            type: 'boolean',
            showLabel: true,
          },
        },
      },
      smsLogin: {
        title: 'login',
        url: 'token/create',
        method: 'POST',
        labelWidth: 0,
        submitStyle: 'width:100%',
        properties: {
          phoneNumber: {
            icon: 'user',
            rules: [
              {
                required: true,
              },
            ],
          },
          verifyCode: {
            input: 'password',
            icon: 'password',
            rules: [
              {
                required: true,
              },
            ],
          },
        },
      },
      externalLogin: [
        {
          name: 'qq',
          icon: 'qq',
        },
        {
          name: 'weixin',
          icon: 'weixin',
        },
        {
          name: 'weibo',
          icon: 'weibo',
        },
      ],
    },
  };
}
