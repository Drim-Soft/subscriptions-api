# Etapa 1: Compilar la aplicación
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar los archivos del proyecto y restaurar dependencias
COPY *.csproj .
RUN dotnet restore

# Copiar todo el código y compilar
COPY . .
RUN dotnet publish -c Release -o out

# Etapa 2: Imagen final para ejecución
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 5096
ENTRYPOINT ["dotnet", "SubscriptionsApi.dll"]

