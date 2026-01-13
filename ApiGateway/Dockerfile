# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy ApiGateway project files
COPY ApiGateway/ /src/ApiGateway/

# Restore and publish
WORKDIR /src/ApiGateway
RUN dotnet restore ApiGateway.csproj
RUN dotnet publish ApiGateway.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published files
COPY --from=build /app/publish .

# Set environment variables for Railway
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose port (Railway will set PORT dynamically)
EXPOSE 8080

ENTRYPOINT ["dotnet", "ApiGateway.dll"]
