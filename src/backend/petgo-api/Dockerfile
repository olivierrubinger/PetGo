# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar csproj e restaurar
COPY ["petgo.api.csproj", "./"]
RUN dotnet restore "petgo.api.csproj"

# Copiar código e compilar
COPY . .
RUN dotnet build "petgo.api.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "petgo.api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Instalar curl para health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

# Porta do Railway (importante: usar variável $PORT)
ENV ASPNETCORE_URLS=http://+:$PORT
ENV ASPNETCORE_ENVIRONMENT=Production

# Forçar uso de IPv4
ENV DOTNET_SYSTEM_NET_SOCKETS_INLINE_COMPLETIONS=1
ENV DOTNET_USE_POLLING_FILE_WATCHER=1

ENTRYPOINT ["dotnet", "petgo.api.dll"]