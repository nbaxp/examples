# 构建

## MySQL

在 Docker 下使用 MySQL 时：

1. 第一次启动时会先执行初始化，会跳过插件的加载，初始化完成后会再次启动会加载插件
1. 在 my.cnf 可以配置插件加载，但插件的相关配置会在初始化时报错，因为初始化时插件未加载
1. 初始化脚本执行是在初始化时加载，此时无法设置插件相关的全局变量

主库：

```sql
--alter user 'root'@'%' identified by 'root';
SET GLOBAL rpl_semi_sync_source_timeout=999999999;
SET GLOBAL rpl_semi_sync_source_enabled = 1; 
SHOW VARIABLES LIKE 'rpl_semi_sync%';
```
从库：

```sql
--alter user 'root'@'%' identified by 'root';
STOP REPLICA;
SHOW VARIABLES LIKE 'rpl_semi_sync%';
CHANGE REPLICATION SOURCE TO
    SOURCE_HOST='mysql',
    SOURCE_PORT=3306,
    SOURCE_USER='replicator',
    SOURCE_PASSWORD='password',
    SOURCE_AUTO_POSITION=1 FOR CHANNEL 'mysql';
START REPLICA;
SHOW VARIABLES LIKE 'rpl_semi_sync%';
SHOW REPLICA STATUS;
```

## 参考链接

1. windows cmd: <https://learn.microsoft.com/zh-cn/windows-server/administration/windows-commands/windows-commands>
1. docker compose doris: <https://github.com/apache/doris/blob/master/docker/runtime/docker-compose-demo/build-cluster/rum-command/3fe_3be.sh>
1. <https://doris.apache.org/zh-CN/docs/1.2/install/construct-docker/run-docker-cluster/>

## Doris

宿主机设置:

```shell
#windows:wsl --list
#windows:wsl -d docker-desktop
sysctl -w vm.max_map_count=2000000
echo "vm.swappiness = 0">> /etc/sysctl.conf
swapoff -a && swapon -a
sysctl -p
```

初始化：

```sql
--登录：
mysql -uroot -P9030 -h127.0.0.1
--修改密码：aA123456!
SET PASSWORD FOR 'root' = PASSWORD('doris-root-password');       
SET PASSWORD FOR 'admin' = PASSWORD('doris-admin-password');
--添加 BE 到集群：172.172.0.61:9050
--ALTER SYSTEM ADD BACKEND "be_host_ip:heartbeat_service_port";
```
