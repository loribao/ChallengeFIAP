FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Appsec.Infra/Appsec.Infra.csproj", "Appsec.Infra/"]
RUN dotnet restore "Appsec.Infra/Appsec.Infra.csproj"
COPY . .
WORKDIR "/src/Appsec.Infra"
RUN dotnet build "Appsec.Infra.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Appsec.Infra.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Appsec.Infra.dll"]
