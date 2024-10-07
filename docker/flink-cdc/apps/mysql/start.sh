#!/bin/bash
set -e

echo "$(date +"%Y-%m-%d %H:%M:%S"):修改 my.cnf 权限"
chmod 644 /etc/mysql/conf.d/my.cnf

# {
#     echo 'init:'
#     until mysql -u root -p"$MYSQL_ROOT_PASSWORD" -e "SHOW DATABASES;"; do
#         echo "Waiting for MySQL to start..."
#         sleep 5
#     done
    
#     until mysql -u root -p"$MYSQL_ROOT_PASSWORD" -e "SHOW PLUGINS;" | grep -q "semisync_source"; do
#         echo "Waiting for semisync_source plugin to load..."
#         sleep 5
#     done

#     SQL_COMMANDS=$(cat <<EOF
# SET GLOBAL rpl_semi_sync_source_timeout=999999999;
# SET GLOBAL rpl_semi_sync_source_enabled = 1;
# EOF
#     )
    
#     mysql -u root -p"$MYSQL_ROOT_PASSWORD" -e "$SQL_COMMANDS"
# } &

echo "$(date +"%Y-%m-%d %H:%M:%S"):调用 Docker 的入口点脚本以启动 MySQL"
exec /usr/local/bin/docker-entrypoint.sh mysqld
