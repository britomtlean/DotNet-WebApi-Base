# FAZ O BUILD E CRIA O DIRETÓRIO DO CONTAINER
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# INSTALAÇÃO DE DEPENDENCIAS
COPY WebApi2026.csproj ./
RUN dotnet restore

# Copia todo o restante
COPY . .

# Publica a aplicação
RUN dotnet publish -c Release -o /app/publish

# BUILDA PARA PRODUÇÃO
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "WebApi2026.dll"]
