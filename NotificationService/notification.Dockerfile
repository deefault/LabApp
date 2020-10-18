FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src/Notification

COPY ./Shared/LabApp.Shared.csproj  ../Shared/LabApp.Shared.csproj
COPY ./NotificationService/NotificationService.csproj ./NotificationService.csproj
RUN dotnet restore NotificationService.csproj

COPY ./NotificationService .
COPY ./Shared ../Shared
RUN dotnet publish NotificationService.csproj --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NotificationService.dll"]