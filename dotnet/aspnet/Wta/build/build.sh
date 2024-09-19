#!/usr/bin/env bash

cd "$(dirname "$0")"

rm -rf dist
mkdir dist

dotnet publish ../be/src/Wta/Wta.csproj -c Release -o ./dist/publish/apps/wta