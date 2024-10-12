setlocal
cd /d "%~dp0"
set PLATFORM_NAME=WINDOWS
docker compose -f docker-compose.yml -f docker-compose.production.yml up -d
