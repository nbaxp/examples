#!/usr/bin/env bash
cd "$(dirname "$0")"
docker compose -f docker-compose.base.yml up -d
docker compose -f docker-compose.yml up -d
