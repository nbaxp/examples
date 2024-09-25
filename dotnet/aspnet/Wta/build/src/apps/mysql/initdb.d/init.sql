CREATE USER 'readonly'@'%' IDENTIFIED WITH mysql_native_password BY 'password';
GRANT SELECT ON *.* TO 'readonly'@'%' WITH GRANT OPTION;

CREATE USER 'replicator'@'%' IDENTIFIED WITH mysql_native_password BY 'password';
GRANT SELECT, RELOAD, SHOW DATABASES, REPLICATION SLAVE, REPLICATION CLIENT ON *.* TO 'replicator'@'%';

FLUSH PRIVILEGES;
