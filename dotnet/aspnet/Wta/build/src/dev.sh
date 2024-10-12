#!/usr/bin/env bash
cd "$(dirname "$0")"
export PLATFORM_NAME=LINUX
docker compose up -d
