setlocal

cd /d "%~dp0"

rmdir /s /q dist
mkdir dist

xcopy /e/q/f/y /exclude:ignore.txt "./src/" "./dist/" 

dotnet publish ../be/src/Wta/Wta.csproj -c Release -o ./dist/apps/wta
xcopy /e/q/f/y "./dist/apps/wta/wwwroot/" "./dist/apps/nginx/html/" 

exit /b %ERRORLEVEL%