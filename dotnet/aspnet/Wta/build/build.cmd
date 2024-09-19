setlocal

cd /d "%~dp0"

rmdir /s /q dist
mkdir dist

dotnet publish ../be/src/Wta/Wta.csproj -c Release -o ./dist/publish/apps/wta

exit /b %ERRORLEVEL%