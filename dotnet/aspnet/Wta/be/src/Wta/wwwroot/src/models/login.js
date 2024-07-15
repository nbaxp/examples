export default function () {
  return {
    title: "登录",
    url: "token/create",
    method: "POST",
    labelWidth: 0,
    submitStyle: "width:100%",
    properties: {
      tenantNumber: {
        input: "select",
        url: "tenant/search",
        value: "number",
      },
      userName: {
        icon: "ep-user",
        rules: [
          {
            required: true,
          },
        ],
      },
      password: {
        input: "password",
        icon: "ep-lock",
        rules: [
          {
            required: true,
          },
        ],
      },
      authCode: {
        input: "image-captcha",
        url: "captcha/image",
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
        title: "记住我",
        type: "boolean",
        showLabel: true,
      },
    },
  };
}
