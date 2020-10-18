FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM node:12.19.0-alpine as front
WORKDIR /web
COPY ./Frontend/AngularProject/package.json ./package.json
COPY ./Frontend/AngularProject/package-lock.json ./package-lock.json
RUN export NODE_OPTIONS="--max-old-space-size=2048" && npx npm-force-resolutions && npm install 

COPY ./Frontend/AngularProject/ /web
RUN export NODE_OPTIONS="--max-old-space-size=2048" && npm run build-prod

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src/Server

COPY ./Shared/LabApp.Shared.csproj  ../Shared/LabApp.Shared.csproj
COPY ./Server/LabApp.Server.csproj ./LabApp.Server.csproj
RUN dotnet restore LabApp.Server.csproj

COPY ./Server .
COPY ./Shared ../Shared
RUN dotnet publish LabApp.Server.csproj --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY --from=front /web/dist ./wwwroot/dist
ENTRYPOINT ["dotnet", "LabApp.Server.dll"]

# TODO: check build