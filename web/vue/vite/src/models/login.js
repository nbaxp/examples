export default function () {
  return {
    properties: {
      passwordLogin: {
        title: 'login',
        url: 'token/create',
        method: 'POST',
        labelWidth: 0,
        submitStyle: 'width:100%',
        properties: {
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
          rememberMe: {
            type: 'boolean',
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
