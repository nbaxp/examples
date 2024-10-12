# 说明

## 开发

1. 本地安装 Docker Desktop
1. 克隆代码到本地
1. 执行 build/src/dev.cmd
1. 配置启动的相关服务，如 flink cdc
1. 打开 be/src/Wta.sln 解决方案，启动 wta 项目，访问 http://localhost:5000
1. 容器内调试：启动项设为 docker compose，启动项目，访问 https://localhost

## 测试

1. 提交代码到服务器
1. 服务器自动构建并生成镜像到仓库，如果是 tag，则自动发布
1. 不是 tag 的，需要手动在 Portainer 中进行容器管理

## 部署

1. 服务器安装 Docker ，配置本地镜像仓库
1. 上传代码到服务器
1. 执行 build/src/start.cmd
1. 配置启动的相关服务，如 flink cdc
1. 提交代码到 git 服务器：http://localhost:13000
1. 打 tag，服务器自动构建并生成镜像到仓库，自动发布