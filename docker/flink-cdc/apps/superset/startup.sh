#!/bin/sh

#pip install -i https://pypi.tuna.tsinghua.edu.cn/simple clickhouse-connect Flask-Limiter[redis]

# create admin account
superset fab create-admin \
  --username admin \
  --firstname Superset \
  --lastname Admin \
  --password admin \
  --email admin@superset.com
# upgrade database
superset db upgrade

# load examples
#superset load_examples​
# set up roles
superset init