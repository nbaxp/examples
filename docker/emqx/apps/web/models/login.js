export default function () {
  return {
    title: "登录",
    url: "token/create",
    method: "POST",
    labelWidth: 0,
    submitStyle: "width:100%",
    properties: {
      userName: {
        title: "用户名",
        icon: "ep-user",
        rules: [
          {
            required: true,
          },
        ],
      },
      password: {
        title: "密码",
        input: "password",
        icon: "ep-lock",
        rules: [
          {
            required: true,
          },
        ],
      },
    },
  };
}
