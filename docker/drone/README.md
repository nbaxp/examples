# Drone 使用示例

1. 配置 .evn 中的 IP 为 IP 地址或域名,windows 本机测试使用 ipconfig 查看  vEthernet (Default Switch) 的 IP
1. 启动 gitea 服务
1. 修改 `data/gitea/gitea/conf/app.ini` 按照下面代码块中的配置更新并重启 gitea，否则 webhook 无法正常工作

    ```conf
    [webhook]
    ALLOWED_HOST_LIST = *
    ```

1. 访问 `http://[IP]:3000/` 安装 gitea，注册管理员账户并登陆（root@aA123456!）
1. 访问`http://[IP]:3000/user/settings/applications`，添加名为 drone 的应用，填写重定向地址,重定向地址格式为 `http://[IP]:[drone_port]/login`，如：`http://172.25.64.1:3800/login`
1. 复制客户端ID和密钥，保存并更新 .env 文件中的 DRONE_GITEA_CLIENT_ID 和 DRONE_GITEA_CLIENT_SECRET
1. 启动 drone 及其 runner 服务，访问 `http://[IP]:3800/` 跳转到 gite 进行登录
1. gite 中新建 demo 项目，提交当前目录文件到项目
1. 在 drone 中点击 SYNC 进行同步，激活 demo 仓库，点击 ACTIVATE REPOSITORY 按钮，接下来选中 Trusted 并保存
1. 访问 `http://[IP]:3800/root/demo` 查看自动构建进度
1. 访问 `http://[IP]:3000/root/-/packages` 查看发布的镜像，选择软件包在设置关联仓库
1. 访问 `http://[IP]:3000/root/demo/packages` 查看仓库的软件包
