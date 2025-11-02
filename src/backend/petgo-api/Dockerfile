# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Desabilitar IPv6 no build
ENV DOTNET_SYSTEM_NET_DISABLEIPV6=1

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

# DESABILITAR IPv6 NO SISTEMA
RUN echo "precedence ::ffff:0:0/96  100" >> /etc/gai.conf

COPY --from=publish /app/publish .

# Variáveis de ambiente - FORÇAR IPv4
ENV ASPNETCORE_URLS=http://+:$PORT
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_SYSTEM_NET_DISABLEIPV6=1
ENV DOTNET_SYSTEM_NET_HTTP_SOCKETSHTTPHANDLER_HTTP2SUPPORT=false
ENV COMPlus_EnableDiagnostics=0

ENTRYPOINT ["dotnet", "petgo.api.dll"]