# Docker 环境下使用 Docker Compose 快速搭建自动构建平台

Windows 下安装最新版的 Docker Desktop：<https://www.docker.com/products/docker-desktop>

目前的几种方式：

1. gitea + drone + drone runner：至少3个项目组合实现
1. gitea/forgejo + gitea runner/forgejo runner：至少2个项目组合实现，兼容 github actions,forgejo 是 gitea 的新分支
1. gitness: drone 的下一代版本，一站式实现，兼容 github actions

## 目录结构说明

1. docker-compose.yml：应用配置文件
1. .env：docker-compose.yml 中使用的环境变量
1. apps：存放应用程序配置
1. data: Docker 挂载的应用目录
1. logs: Docker 挂在的日志目录

## 服务配置

命令行执行 `docker compose upd -d` 启动服务

### Portainer-CE

`http://host.docker.internal:19000/` 通过 UI 手动设置密码，如 admin 1111111111111111

### MinIO

`http://host.docker.internal:9001/login` 通过 UI 手动创建默认 bucket，如 default

### Gitea

1. 通过 UI 手动安装,基础 URL 设置为 `http://[ip/domain]:[port]` 格式，如：`http://host.docker.internal:3000/`与 Drone 搭配时不可使用 localhost 作为 hostname
1. 如不填写管理员信息，则第一个注册的账户为管理员,建议用户名设置为 root
1. 在 Gitea 中创建名为 drone 的 OAuth2 应用，重定向为 `http://[ip/domain]:[drone_port]/login`,记录下客户端 ID 和密钥并更新 .env，测试时可使用 `http://host.docker.internal:3800/login`,命令行执行 `docker compose up -d`
1. 修改 `data/gitea/gitea/conf/app.ini` 按照下面代码块中的配置更新，否则 webhook 无法正常工作

    ```conf
    [webhook]
    ALLOWED_HOST_LIST = *
    ```

1. 命令行执行 `docker compose restart gitea`

### Nexus3

1. admin 密码文件位置在服务命令行中执行 `cat /nexus-data/admin.password`，重设密码如：`aA123456!`
1. Repository -> Repositories: 创建一个 docker hosted repository 用于私有镜像，可命名为 `docker-hosted-repository`选中 Allow anonymous docker pull,设置 HTTP 端口 8082
1. Security -> Realms 添加 Docker Bearer Token Realm
1. `docker compose restart nexus3` 重启容器耗时较多，访问 `http://host.docker.internal:8081/` 等待仓库可用

## 测试

1. 登录 `http://host.docker.internal:3000` 创建测试仓库 demo
1. 访问 `http://host.docker.internal:3800/` 激活 demo 仓库，点击 ACTIVATE REPOSITORY 按钮，接下来选中 Trusted 并保存
1. 使用 git 克隆仓库到本地
1. 复制目录 demo 下的全部文件到本地仓库
1. 提交 demo 到远程
1. 访问 `http://host.docker.internal:3800/root/demo` 查看自动构建进度
1. 查看 `http://localhost:9001/browser/default` 查看自动发布的存档
1. 查看 `http://host.docker.internal:8081/#browse/browse:docker-hosted-repository` 查看自动发布的镜像
1. 查看 `http://host.docker.internal/` 查看自动发布的应用
1. 进入 docker-dind 容器命令行，打开 /app 目录，执行 `docker compose up -d`，访问 `http://host.docker.internal:8080` 验证发布到私有仓库的镜像被正确拉取并容器化运行
1. 访问 `http://localhost:3000/root/demo/_edit/main/src/App.vue` 在线编辑并提交，验证在没有开发环境状态下快速更新源码并发布程序，docker-dind 命令行执行 `docker compose pull` `docker compose up -d`
1. 可修改 demo 的 drone.yml 配置文件，使用可访问的 SMTP 服务器信息实现自动构建成功后发送 Email 通知到资源库所有者

## 常见问题

### server error - Ports are not available

使用 `netstat -ano|findstr "[port]"` 查看端口被哪个进程占用，如果端口未被占用，执行 `net stop winnat` 和 `net start winnat` ,重新启动容器
