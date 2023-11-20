export default function () {
  return {
    title: "登录",
    //type: "object",
    api: {
      url: "token/create",
    },
    properties: {
      userName: {
        title: "用户名",
        //type: "string",
        rules: [
          {
            required: true,
          },
        ],
      },
      password: {
        title: "密码",
        //type: "string",
        input: "password",
        rules: [
          {
            required: true,
          },
        ],
      },
    },
  };
}
