# gitea 配置

1. 启动 docker compose,访问 `http://localhost:3000/` 安装
1. 访问 `http://localhost:3000/user/settings/actions/runners`,复制 Registration Token ，更新 .env 文件的 REGISTRATION_TOKEN
1. 执行 `docker compose up -d` ，待 act_runner 重启成功后刷新 `http://localhost:3000/user/settings/actions/runners` 查看 runners 是否生效
1. 同步相关仓库到本地
    1. `https://github.com/actions/runner-images`
    1. `https://github.com/docker/setup-qemu-action`
    1. `https://github.com/docker/setup-buildx-action`
    1. `https://github.com/docker/build-push-action`
    1. `https://github.com/papodaca/install-docker-action`
1. 创建仓库 demo 并在设置中启用 actions
1. 推送当前文件所在目录内容到 demo
1. 访问 `http://localhost:3000/root/demo/actions` 查看运行状态
