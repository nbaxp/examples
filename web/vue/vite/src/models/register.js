export default function () {
  return {
    url: 'user/register',
    submitStyle: 'width:100%',
    properties: {
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
        rules: [
          {
            validator: 'compare',
            compare: 'password',
          },
        ],
      },
      email: {
        rules: [
          { required: true },
          {
            type: 'email',
          },
        ],
      },
      code: {
        rules: {
          required: true,
        },
      },
      codeHash: {
        hidden: true,
      },
    },
  };
}
