INSTALL PLUGIN rpl_semi_sync_master SONAME 'semisync_master.so';  
INSTALL PLUGIN rpl_semi_sync_slave SONAME 'semisync_slave.so';  
SET GLOBAL rpl_semi_sync_master_enabled = 1; 
SET GLOBAL rpl_semi_sync_slave_enabled = 1;  

-- START SLAVE;

CHANGE MASTER TO MASTER_HOST='mysql',
MASTER_USER='replicator',
MASTER_PORT=3306,
MASTER_PASSWORD='password',
MASTER_AUTO_POSITION=1 FOR CHANNEL 'master1';