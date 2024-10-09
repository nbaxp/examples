setlocal
cd /d "%~dp0"
docker compose -f docker-compose.yml -f docker-compose.development.yml up -d
