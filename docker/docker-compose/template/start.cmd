setlocal
cd /d "%~dp0"
docker compose -f docker-compose.base.yml up -d
docker compose -f docker-compose.yml up -d
