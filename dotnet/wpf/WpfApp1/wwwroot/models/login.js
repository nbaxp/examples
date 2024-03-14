export default function () {
  return {
    title: "登录",
    type: "object",
    properties: {
      username: {
        title: "用户名",
        type: "string",
        rules: [
          {
            required: true,
            message: "用户名不能为空",
          },
        ],
      },
      password: {
        title: "密码",
        type: "string",
        input: "password",
        rules: [
          {
            required: true,
            message: "密码不能为空",
          },
        ],
      },
      client_id: {
        default: "basic-web",
        hidden: true,
      },
      grant_type: {
        default: "password",
        hidden: true,
      },
      scope: {
        default: "WebAppGateway BaseService",
        hidden: true,
      },
    },
  };
}
