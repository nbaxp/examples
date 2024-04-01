# gitea 配置

1. 启动 docker compose,访问 `http://localhost:3000/` 安装
1. 访问 `http://localhost:3000/user/settings/actions/runners`,复制 Registration Token ，更新 .env 文件的 REGISTRATION_TOKEN
1. 执行 `docker compose up -d` ，待 act_runner 重启成功后刷新 `http://localhost:3000/user/settings/actions/runners` 查看 runners 是否生效
1. 创建仓库 demo 并在设置中启用 actions
1. git clonse demo 复制当前目录内容更新 demo 并推送
1. 访问 `http://localhost:3000/root/demo/actions` 查看运行状态
