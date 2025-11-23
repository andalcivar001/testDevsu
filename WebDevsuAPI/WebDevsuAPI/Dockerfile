# Dockerfile multi-stage para WebDevsuAPI (.NET 8)
# 1) Build con SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar solo los csproj relevantes primero para aprovechar la cache
# Este Dockerfile asume que se ejecuta con el contexto del repo raíz.
COPY ["WebDevsuAPI/WebDevsuAPI.csproj", "WebDevsuAPI/"]
COPY ["WebDevsuLogic/WebDevsuLogic.csproj", "WebDevsuLogic/"]
COPY ["WebDevsuDatabase/WebDevsuDatabase.csproj", "WebDevsuDatabase/"]

# Restaurar usando el proyecto principal
RUN dotnet restore "WebDevsuAPI/WebDevsuAPI.csproj"

# Copiar todo el contexto (incluye los proyectos referenciados)
COPY . .

# Publicar desde el subdirectorio del proyecto Web API
WORKDIR /src/WebDevsuAPI
RUN dotnet publish "WebDevsuAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2) Runtime image más ligera
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Hacer que la app escuche en el puerto 7103 dentro del contenedor
ENV ASPNETCORE_URLS=http://+:7103

# Sobrescribir la cadena de conexión para Docker
ENV ConnectionStrings__DefaultConnection="Host=host.docker.internal;Port=5432;Database=devsu;Username=postgres;Password=123;"

# Copiar artefactos publicados desde la etapa de build
COPY --from=build /app/publish .

EXPOSE 7103

ENTRYPOINT ["dotnet", "WebDevsuAPI.dll"]
