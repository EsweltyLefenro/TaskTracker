# ===== build stage =====
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY TaskTracker.Web/TaskTracker.Web.csproj TaskTracker.Web/
RUN dotnet restore TaskTracker.Web/TaskTracker.Web.csproj

COPY . .
WORKDIR /src/TaskTracker.Web
RUN dotnet publish -c Release -o /app/publish --no-restore

# ===== runtime stage =====
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Нужен curl для healthcheck
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "TaskTracker.Web.dll"]