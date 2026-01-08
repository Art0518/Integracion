# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything
COPY . .

# Publish only the microservice project (skip restore errors from .NET Framework projects)
WORKDIR /src/Microservicio.Busqueda
RUN dotnet publish Microservicio.Busqueda.csproj -c Release -o /app/publish --no-restore || \
    dotnet publish Microservicio.Busqueda.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published files
COPY --from=build /app/publish .

# Set environment variables for Railway
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "Microservicio.Busqueda.dll"]
