# gitea 配置

1. 配置 .evn 中的 IP 为 IP 地址或域名，windows 下测试使用 ipconfig 查找  vEthernet (WSL (Hyper-V firewall)) 对应的 IP
1. 更新宿主机 daemon.json ,添加 [IP]:3000 私有仓库，windows 下测试可以通过 docker desktop 配置功能。如：

   ```config
   {
     "registry-mirrors": [
       "http://hub-mirror.c.163.com"
     ],
     "insecure-registries": [
       "172.21.176.1/:3000"
     ]
   }
   ```

1. 启动 gitea 服务
1. 访问 `http://[IP]:3000/` 安装 gitea，注册管理员账户并登陆（root@aA123456!）
1. 访问 <http://[IP]:3000/user/settings/actions/runners,复制> Registration Token ，更新 .env 文件的 REGISTRATION_TOKEN
1. 访问 `http://[IP]:3000/user/settings/applications` 添加名为 package 的令牌，drone 使用此令牌发布版本，需要添加 package 和 repository 的读写权限
1. 执行 docker compose up -d` ，待 act_runner 重启成功后刷新 <http://[IP]:3000/user/settings/actions/runners> 查看 runners 是否生效
1. 同步相关仓库到本地
   1. <https://github.com/actions/checkout/>
   1. <https://github.com/docker/setup-qemu-action>
   1. <https://github.com/docker/setup-buildx-action>
   1. <https://github.com/docker/login-action>
   1. <https://github.com/docker/build-push-action>
   1. <https://github.com/easingthemes/ssh-deploy>
1. 创建仓库 demo 并在设置中启用 actions
1. 推送当前文件所在目录内容到 demo
1. 访问 <http://localhost:3000/root/demo/actions> 查看运行状态
1. 建议修改 docker 宿主机的 /etc/ssh/sshd_config 的默认端口，将 22 端口用于 git
1. 配置 ssh key

   ```bash
   cd ~/.ssh
   ls # 查看是否已经生成过
   ssh-keygen
   ls # id_rsa  id_rsa.pub  known_hosts
   cat ~/.ssh/id_rsa.pub # 添加到 gitea ssh 密钥
   ```

1. 在 TortoiseGit 的配置中，找到 Network 下的 SSH client 配置为 Git 安装目录下的`usr\bin\ssh.exe`
