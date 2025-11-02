# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar csproj e restaurar dependências
COPY ["petgo.api.csproj", "./"]
RUN dotnet restore "petgo.api.csproj"

# Copiar todo o código e compilar
COPY . .
RUN dotnet build "petgo.api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "petgo.api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Instalar curl para health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copiar arquivos publicados
COPY --from=publish /app/publish .

# Expor porta (Railway usa PORT env variable)
EXPOSE 8080

# Variáveis de ambiente padrão
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Comando de inicialização
ENTRYPOINT ["dotnet", "petgo.api.dll"]