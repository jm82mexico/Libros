#!/bin/bash
proyecto=$1
carpeta=$2
email=$3
usuario=$4
version=$5
certificado=$6
dotnet tool install --global dotnet-ef --version $version

dotnet new sln -n $proyecto
dotnet restore
dotnet new classlib -o $proyecto.Application
dotnet new classlib -o $proyecto.Domain
dotnet new classlib -o $proyecto.Infrastructure
dotnet new classlib -o $proyecto.Identity
dotnet new webapi -o $proyecto.API

dotnet sln add $proyecto.Application
dotnet sln add $proyecto.Domain
dotnet sln add $proyecto.Infrastructure
dotnet sln add $proyecto.Identity
dotnet sln add $proyecto.API

dotnet add $proyecto.API/$proyecto.API.csproj reference $proyecto.Identity/$proyecto.Identity.csproj
dotnet add $proyecto.API/$proyecto.API.csproj reference $proyecto.Infrastructure/$proyecto.Infrastructure.csproj

dotnet add $proyecto.Application/$proyecto.Application.csproj reference $proyecto.Domain/$proyecto.Domain.csproj

dotnet add $proyecto.Identity/$proyecto.Identity.csproj reference $proyecto.Application/$proyecto.Application.csproj

dotnet add $proyecto.Identity/$proyecto.Identity.csproj reference $proyecto.Application/$proyecto.Application.csproj

dotnet new gitignore
dotnet dev-certs https -ep /home/vscode/.aspnet/https/aspnetapp.pfx -p {$certificado}
git config --global init.defaultBranch main
git config --global --add safe.directory /workspaces/$carpeta
git config --global user.email "$email"
git config --global user.name "$usuario"
git init
git add .
git commit -m "Commit inicial"
