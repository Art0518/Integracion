# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy libs folder with pre-compiled .NET Framework 4.8 DLLs
COPY libs/ /src/libs/

# Copy microservice project files
COPY Microservicio.Busqueda/ /src/Microservicio.Busqueda/

# Restore and publish
WORKDIR /src/Microservicio.Busqueda
RUN dotnet restore Microservicio.Busqueda.csproj
RUN dotnet publish Microservicio.Busqueda.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published files
COPY --from=build /app/publish .

# Set environment variables for Railway
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "Microservicio.Busqueda.dll"]
