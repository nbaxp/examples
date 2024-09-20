#!/bin/bash

echo "$(date +"%Y-%m-%d %H:%M:%S"):修改 my.cnf 权限"
chmod 644 /etc/mysql/conf.d/my.cnf

echo "$(date +"%Y-%m-%d %H:%M:%S"):调用 Docker 的入口点脚本以启动 MySQL"
exec /usr/local/bin/docker-entrypoint.sh mysqld
