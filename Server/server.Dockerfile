FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src/Server

COPY ./Shared/LabApp.Shared.csproj  ../Shared/LabApp.Shared.csproj
COPY ./Server/LabApp.Server.csproj ./LabApp.Server.csproj
COPY ./LabApp.DbContext/LabApp.DbContext.csproj ../LabApp.DbContext/LabApp.DbContext.csproj
COPY ./LabApp.Shared.EventBus/LabApp.Shared.EventBus.csproj ../LabApp.Shared.EventBus/LabApp.Shared.EventBus.csproj
RUN dotnet restore LabApp.Server.csproj

COPY ./Server .
COPY ./Shared ../Shared
COPY ./LabApp.DbContext ../LabApp.DbContext
COPY ./LabApp.Shared.EventBus ../LabApp.Shared.EventBus
RUN dotnet publish LabApp.Server.csproj --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "LabApp.Server.dll"]