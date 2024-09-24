#!/usr/bin/env bash
cd "$(dirname "$0")"
docker compose -f docker-compose.base.yml up -d
docker compose -f docker-compose.minio.yml up -d
docker compose -f docker-compose.redis.yml up -d
docker compose -f docker-compose.mysql.yml up -d
docker compose -f docker-compose.yml -f docker-compose.development.yml up -d
