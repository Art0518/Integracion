# Build and publish stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de proyecto
COPY ["Microservicio.Reserva/Microservicio.Reserva.csproj", "Microservicio.Reserva/"]
COPY ["AccesoDatos/AccesoDatos.csproj", "AccesoDatos/"]
COPY ["GDatos/GDatos.csproj", "GDatos/"]
COPY ["Logica/Logica.csproj", "Logica/"]

# Restaurar dependencias
RUN dotnet restore "Microservicio.Reserva/Microservicio.Reserva.csproj"

# Copiar todo el código fuente
COPY . .

# Publicar
WORKDIR "/src/Microservicio.Reserva"
RUN dotnet publish "Microservicio.Reserva.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Variables de entorno para Railway
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT:-8080}

# Exponer puerto (Railway lo sobrescribe con PORT)
EXPOSE 8080

ENTRYPOINT ["dotnet", "Microservicio.Reserva.dll"]
