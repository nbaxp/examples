# 架构设计

1. 基于容器化运行
1. 开发相关
   1. 源码管理：harness
   1. 自动构建：harness
   1. 镜像或其他软件包发布：harness
   1. 软件注册表：harness
1. 调试环境和单机部署
   1. Docker Compose
   1. Portainer CE
1. 应用架构
    1. 前后端分离
    1. 前端采用 ESM 模块化
    1. Caddy Web 服务器和网关
    1. MySQL 8.0 关系数据库
    1. Doris 3.0 数据仓库
    1. Flink CDC 数据集成框架
    1. Redis 缓存
    1. MinIO 文件存储