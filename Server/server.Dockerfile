FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src/Server



COPY ./Server/LabApp.Server.csproj ./LabApp.Server.csproj

COPY ./Shared/LabApp.Shared.csproj  ../Shared/LabApp.Shared.csproj
COPY ./LabApp.Shared.EventBus/LabApp.Shared.EventBus.csproj  ../LabApp.Shared.EventBus/LabApp.Shared.EventBus.csproj
COPY ./LabApp.Shared.EventBus.Consistency/LabApp.Shared.EventBus.Consistency.csproj  ../LabApp.Shared.EventBus.Consistency/LabApp.Shared.EventBus.csproj
COPY ./LabApp.DbContext/LabApp.DbContext.csproj ../LabApp.DbContext/LabApp.DbContext.csproj
COPY ./LabApp.Shared.EventBus/LabApp.Shared.EventBus.csproj ../LabApp.Shared.EventBus/LabApp.Shared.EventBus.csproj
COPY LabApp.Shared.EventConsistency/LabApp.Shared.EventConsistency.csproj ../LabApp.Shared.EventConsistency/LabApp.Shared.EventConsistency.csproj
COPY LabApp.Shared.EventBus.Consistency/LabApp.Shared.EventBus.Consistency.csproj ../LabApp.Shared.EventBus.Consistency/LabApp.Shared.EventBus.Consistency.csproj
COPY LabApp.Shared.DbContext/LabApp.Shared.DbContext.csproj ../LabApp.Shared.DbContext/LabApp.Shared.DbContext.csproj ../LabApp.Shared.DbContext/LabApp.Shared.DbContext.csproj ../LabApp.Shared.DbContext/LabApp.Shared.DbContext.csproj
COPY LabApp.Shared.EventConsistency.Stores.EF/LabApp.Shared.EventConsistency.Stores.EF.csproj ../LabApp.Shared.EventConsistency.Stores.EF/LabApp.Shared.EventConsistency.Stores.EF.csproj ../LabApp.Shared.EventConsistency.Stores.EF/LabApp.Shared.EventConsistency.Stores.EF.csproj ../LabApp.Shared.EventConsistency.Stores.EF/LabApp.Shared.EventConsistency.Stores.EF.csproj
COPY LabApp.Shared.EventConsistency.Abstractions/LabApp.Shared.EventConsistency.Abstractions.csproj ../LabApp.Shared.EventConsistency.Abstractions/LabApp.Shared.EventConsistency.Abstractions.csproj
COPY LabApp.Shared.EventBus.Consistency/LabApp.Shared.EventBus.Consistency.csproj ../LabApp.Shared.EventBus.Consistency/LabApp.Shared.EventBus.Consistency.csproj
COPY LabApp.Shared.DbContext/LabApp.Shared.DbContext.csproj ../LabApp.Shared.DbContext/LabApp.Shared.DbContext.csproj
COPY LabApp.Shared.EventConsistency.Stores.EF/LabApp.Shared.EventConsistency.Stores.EF.csproj ../LabApp.Shared.EventConsistency.Stores.EF/LabApp.Shared.EventConsistency.Stores.EF.csproj
COPY LabApp.Shared.EventConsistency.Abstractions/LabApp.Shared.EventConsistency.Abstractions.csproj ../LabApp.Shared.EventConsistency.Abstractions/LabApp.Shared.EventConsistency.Abstractions.csproj
#RUN for file in $(ls *.csproj); do mkdir -p ../${file%.*}/ && mv $file ../${file%.*}/; done

RUN dotnet restore ./LabApp.Server.csproj

COPY ./Server .
COPY ./Shared ../Shared
COPY ./LabApp.DbContext ../LabApp.DbContext
COPY ./LabApp.Shared.EventBus ../LabApp.Shared.EventBus
COPY ./LabApp.Shared.EventBus.Consistency ../LabApp.Shared.EventBus.Consistency
COPY ./LabApp.Shared.EventConsistency ../LabApp.Shared.EventConsistency
COPY ./LabApp.Shared.EventConsistency.Abstractions ../LabApp.Shared.EventConsistency.Abstractions
COPY ./LabApp.Shared.EventConsistency.Stores.EF ../LabApp.Shared.EventConsistency.Stores.EF

RUN dotnet publish ./LabApp.Server.csproj --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "LabApp.Server.dll"]