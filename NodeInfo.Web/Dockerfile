# build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY NodeInfo.Web/NodeInfo.Web.csproj NodeInfo.Web/
RUN dotnet restore NodeInfo.Web/NodeInfo.Web.csproj
COPY . .
RUN dotnet publish NodeInfo.Web/NodeInfo.Web.csproj -c Release -o /app/publish --no-restore

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# для healthcheck
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "NodeInfo.Web.dll"]