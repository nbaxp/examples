#!/bin/sh

pip install pydoris -i https://pypi.tuna.tsinghua.edu.cn/simple
#pip install -i https://pypi.tuna.tsinghua.edu.cn/simple clickhouse-connect Flask-Limiter[redis]

sed -i "s/BABEL_DEFAULT_LOCALE = \"en/BABEL_DEFAULT_LOCALE = \"zh/g" superset/config.py
sed -i "s/LANGUAGES = {}/# LANGUAGES = {\"zh\": {\"flag\": \"cn\", \"name\": \"Chinese\"},}/g" superset/config.py

# create admin account
superset fab create-admin \
  --username admin \
  --firstname Superset \
  --lastname Admin \
  --password aA123456 \
  --email admin@superset.com
# upgrade database
superset db upgrade

# load examples
#superset load_examples​
# set up roles
superset init

/usr/bin/run-server.sh