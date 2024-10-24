# WTA

Web Template ASP.NET

```txt
/
├── .gitignore								# 提交过滤
├── .gitattributes							# 文件配置
├── .editorconfig							# 通用编辑器配置
├── Nuget.config							# 程序包配置
├── Directory.Packages.props				# 集中管理程序包版本
├── Wta.sln									# 解决方案文件
├── docker-compose.dcproj					# Docker Compose 项目文件
├── docker-compose.yml						# Docker Compose 配置
├── launchSettings.json						# Docker Compose 启动配置
├── .dockerignore							# 构建镜像的过滤
├── docs									# 文档
│     ├── architecture.md					# 架构设计
│     ├── framework.md						# 框架设计
│     ├── fe.md								# 前端说明
│     └── be.md								# 后端说明
├── build									# 构建
│     ├── build.cmd							# windows 构建脚本
│     ├── build.sh							# linux 构建脚本
│     ├── src								# 预发布文件
│	      ├── docker-compose.extend.yml		# 定义服务基础属性
│	      ├── docker-compose.yml			# 定义调试和生产环境通用服务
│	      ├── docker-compose.production.yml	# 定义生产环境服务
│	      ├── start.sh						# linux 生产启动脚本
│		  └── start.cmd						# windows 生产启动脚本
├── src										# 源码
│	  ├── Wta								# Web 项目
│ 	  ├── Wta.Application					# 应用类库
│     ├── Wta.Infrastructure				# 基础设施类库 
│     └── Wta.Migrations					# 数据迁移项目
└── README.md								# 说明
```
