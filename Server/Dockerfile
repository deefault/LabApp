FROM mcr.microsoft.com/dotnet/core/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM node:14.3 as front
COPY ../Frontend/AngularProject/package.json ./package.json
COPY ../Frontend/AngularProject/package-lock.json ./package-lock.json
RUN export NODE_OPTIONS="--max-old-space-size=4096" && npm install && npm install -g @angular/cli

COPY ../Frontend/AngularProject/ ./web
WORKDIR /web
RUN export NODE_OPTIONS="--max-old-space-size=4096" && npm run build-prod

FROM mcr.microsoft.com/dotnet/core/sdk:5.0 AS build
WORKDIR /src

COPY ./LabApp.Server.csproj ./LabApp.Server.csproj
RUN dotnet restore "eShopOnContainers-ServicesAndWebApps.sln"

COPY . .
WORKDIR /src/Services/Ordering/Ordering.API
RUN dotnet publish --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY --from=front /web/dist ./wwwroot/dist
ENTRYPOINT ["dotnet", "LabApp.Server.dll"]

# TODO: check build