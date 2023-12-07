export default function () {
  return {
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
    },
  };
}
