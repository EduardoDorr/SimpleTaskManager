FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./DDS.SimpleTaskManager.API/DDS.SimpleTaskManager.API.csproj", "DDS.SimpleTaskManager.API/"]
COPY ["./DDS.SimpleTaskManager.Core/DDS.SimpleTaskManager.Core.csproj", "DDS.SimpleTaskManager.Core/"]
RUN dotnet restore "./DDS.SimpleTaskManager.API/DDS.SimpleTaskManager.API.csproj"
COPY . .
WORKDIR "/src/DDS.SimpleTaskManager.API"
RUN dotnet build "./DDS.SimpleTaskManager.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DDS.SimpleTaskManager.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DDS.SimpleTaskManager.API.dll"]
