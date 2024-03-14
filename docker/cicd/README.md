# portainer + minio + gitea + drone

## Portainer-CE

通过UI手动设置密码

## MinIO

通过UI手动创建 bucket

## Gitea

1. 通过UI手动安装,基础URL设置为 `http://[ip/domain]:[port]` 格式，与 Drone 搭配时不可使用 localhost 作为 hostname，测试时可使用 `host.docker.internal`
1. 第一个注册的账户为管理员
1. 修改 `data/gitea/gitea/conf/app.ini`  [webhook] 节点下添加配置 `ALLOWED_HOST_LIST = *`，否则 webhook 无法正常工作

## Drone

1. 在 Gitea 中创建名为 drone 的 OAuth2 应用，重定向为 `http://[ip/domain]:[drone_port]/login`,记录下客户端ID和密钥并更新 .env

## Nexus3

1. admin 密码文件位置 `/nexus-data/admin.password`
1. Repository -> Repositories: 创建一个 docker hosted repository 用于私有镜像，选中 Allow anonymous docker pull,设置 HTTP 端口 8082，重启容器
1. Security -> Realms 添加 Docker Bearer Token Realm
1. 启动特别耗时

## 常见问题

### server error - Ports are not available

使用 `netstat -ano|findstr "[port]"` 查看端口被哪个进程占用，如果端口未被占用，执行 `net stop winnat` 和 `net start winnat` ,重新启动容器
