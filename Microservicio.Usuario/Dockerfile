# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy libs folder with pre-compiled .NET Framework 4.8 DLLs
COPY libs/ /src/libs/

# Copy microservice project files
COPY Microservicio.Usuario/ /src/Microservicio.Usuario/

# Restore and publish
WORKDIR /src/Microservicio.Usuario
RUN dotnet restore Microservicio.Usuario.csproj
RUN dotnet publish Microservicio.Usuario.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published files
COPY --from=build /app/publish .

# Set environment variables for Railway
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "Microservicio.Usuario.dll"]
