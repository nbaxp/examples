export default function () {
  return {
    url: 'user/reset-password',
    properties: {
      currentPassword: {
        input: 'password',
        icon: 'password',
        rules: [
          {
            required: true,
          },
        ],
      },
      newPassword: {
        input: 'password',
        icon: 'password',
        rules: [
          {
            required: true,
          },
        ],
      },
      confirmNewPassword: {
        input: 'password',
        icon: 'password',
        rules: [
          {
            validator: 'compare',
            compare: 'newPassword',
          },
        ],
      },
    },
  };
}
