# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy Gateway project files
COPY Gateway.CafeSanJuan/ /src/Gateway.CafeSanJuan/

# Restore and publish
WORKDIR /src/Gateway.CafeSanJuan
RUN dotnet restore Gateway.CafeSanJuan.csproj
RUN dotnet publish Gateway.CafeSanJuan.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published files
COPY --from=build /app/publish .

# Set environment variables for Railway
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "Gateway.CafeSanJuan.dll"]
