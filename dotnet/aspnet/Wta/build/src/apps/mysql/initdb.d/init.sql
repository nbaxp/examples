INSTALL PLUGIN rpl_semi_sync_master SONAME 'semisync_master.so';  
INSTALL PLUGIN rpl_semi_sync_slave SONAME 'semisync_slave.so';  
SET GLOBAL rpl_semi_sync_master_enabled = 1; 
SET GLOBAL rpl_semi_sync_slave_enabled = 1;  

CREATE USER 'readonly'@'%' IDENTIFIED WITH mysql_native_password BY 'password';
GRANT SELECT ON *.* TO 'readonly'@'%' WITH GRANT OPTION;

CREATE USER 'replicator'@'%' IDENTIFIED WITH mysql_native_password BY 'password';
GRANT REPLICATION SLAVE ON *.* TO 'replicator'@'%';
