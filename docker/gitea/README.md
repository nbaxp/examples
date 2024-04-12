# gitea 配置

基于 WSL 的 Windows 环境下，无法成功登录 Docker Registry，建议使用 Drone

1. 配置 .evn 中的 IP 为 IP 地址或域名
1. 启动 gitea 服务
1. 访问 `http://[IP]:3000/` 安装 gitea，注册管理员账户并登陆（root@aA123456!）
1. 访问 <http://[IP]:3000/user/settings/actions/runners,复制> Registration Token ，更新 .env 文件的 REGISTRATION_TOKEN
1. 执行 docker compose up -d` ，待 act_runner 重启成功后刷新 <http://[IP]:3000/user/settings/actions/runners> 查看 runners 是否生效
1. 同步相关仓库到本地
    1. <https://github.com/actions/checkout/>
    1. <https://github.com/docker/setup-qemu-action>
    1. <https://github.com/docker/setup-buildx-action>
    1. <https://github.com/docker/login-action>
    1. <https://github.com/docker/build-push-action>
1. 创建仓库 demo 并在设置中启用 actions
1. 推送当前文件所在目录内容到 demo
1. 访问 <http://localhost:3000/root/demo/actions> 查看运行状态
