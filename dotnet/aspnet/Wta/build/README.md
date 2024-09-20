# build

## 批处理脚本

<https://learn.microsoft.com/zh-cn/windows-server/administration/windows-commands/windows-commands>

## MySQL 主从半同步

使用 my.cnf 配置加载插件，使用 SQL 命令配置

主库：

```sql
SET GLOBAL rpl_semi_sync_source_timeout=999999999;
SET GLOBAL rpl_semi_sync_source_enabled = 1; 
SHOW VARIABLES LIKE 'rpl_semi_sync%';
```
从库：

```sql
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