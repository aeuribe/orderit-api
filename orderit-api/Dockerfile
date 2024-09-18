# Etapa base: Imagen final con solo el runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
ENV DB_BUSINESS_CONNECTION_STRING="Host=192.168.1.3;Port=5432;Database=OrderItDB;User Id=postgres;Password=aeua2000"
ENV DB_IDENTITY_CONNECTION_STRING="Host=192.168.1.3;Port=5432;Database=IdentityOrderItDB;User Id=postgres;Password=aeua2000"

# Etapa de construcción: Imagen con el SDK para compilar y publicar la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia el archivo .csproj y restaura las dependencias
COPY orderit-api.csproj ./
RUN dotnet restore

# Copia el resto de los archivos y compila la aplicación
COPY . .
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

# Etapa de publicación: Publica la aplicación
FROM build AS publish
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final: Imagen que solo contiene los archivos necesarios para ejecutar la aplicación
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "orderit-api.dll"]
