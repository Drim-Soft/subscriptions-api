# Imagen base para compilar
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar archivo de proyecto
COPY ./*.csproj ./
RUN dotnet restore

# Copiar todo el código fuente
COPY . ./
RUN dotnet publish -c Release -o out

# Imagen final de runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copiar artefactos publicados
COPY --from=build /app/out .

# Copiar .env al contenedor
COPY .env /app/.env

# Exponer puerto
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENV PORT=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "SubscriptionsApi.dll"]
