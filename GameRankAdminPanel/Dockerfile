FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
ENV ASPNETCORE_URLS=http://192.168.0.103:5002
EXPOSE 5002


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GameRankAdminPanel/GameRankAdminPanel.csproj", "GameRankAdminPanel/"]
RUN dotnet restore "GameRankAdminPanel/GameRankAdminPanel.csproj"
COPY . .
WORKDIR "/src/GameRankAdminPanel"
RUN dotnet build "./GameRankAdminPanel.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GameRankAdminPanel.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameRankAdminPanel.dll"]