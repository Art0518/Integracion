# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy Gateway project files
COPY Gateway.CafeSanJuan/ /src/Gateway.CafeSanJuan/
COPY Gateway.CafeSanJuan/ocelot.json /src/Gateway.CafeSanJuan/ocelot.json

# Restore and publish
WORKDIR /src/Gateway.CafeSanJuan
RUN dotnet restore Gateway.CafeSanJuan.csproj
RUN dotnet publish Gateway.CafeSanJuan.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published files
COPY --from=build /app/publish .
COPY --from=build /src/Gateway.CafeSanJuan/ocelot.json .

ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "Gateway.CafeSanJuan.dll"]