CREATE DATABASE demo;
USE demo;
CREATE TABLE users (
    id INT PRIMARY KEY,
    name VARCHAR(100),
    age INT
);
INSERT INTO users VALUES(1,'alice',14);