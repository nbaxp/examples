CREATE USER 'readonly'@'%' IDENTIFIED WITH mysql_native_password BY 'aA123456!';
GRANT SELECT ON *.* TO 'readonly'@'%' WITH GRANT OPTION;

CREATE USER 'replicator'@'%' IDENTIFIED WITH mysql_native_password BY 'aA123456!';
GRANT SELECT, RELOAD, SHOW DATABASES, REPLICATION SLAVE, REPLICATION CLIENT ON *.* TO 'replicator'@'%';

FLUSH PRIVILEGES;

ALTER USER 'root'@'%' IDENTIFIED WITH mysql_native_password BY 'aA123456!';
