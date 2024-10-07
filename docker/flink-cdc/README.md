# FLink 1.20 FLink CDC 3.2.0 MySQL 8.0 to Doris 3.0

自动建库建表，目前最高只支持 MySQL 8.0.x

```bash
# container: Flink JobManager
pwd
# pwd: /opt/flink/cdc/conf
../bin/flink-cdc.sh mysql2doris.yaml
```
