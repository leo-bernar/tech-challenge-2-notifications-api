FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/FCG.Notifications.Api/FCG.Notifications.Api.csproj", "src/FCG.Notifications.Api/"]
RUN dotnet restore "src/FCG.Notifications.Api/FCG.Notifications.Api.csproj"

COPY . .
RUN dotnet publish "src/FCG.Notifications.Api/FCG.Notifications.Api.csproj" \
    --configuration Release \
    --no-restore \
    --output /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=build /app/publish .

USER $APP_UID
ENTRYPOINT ["dotnet", "FCG.Notifications.Api.dll"]
