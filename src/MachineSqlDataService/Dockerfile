FROM microsoft/dotnet:2.2-runtime-stretch-slim AS base
WORKDIR /app


FROM microsoft/dotnet:2.2-sdk-stretch AS build
WORKDIR /src
COPY ["MachineSqlDataService/MachineSqlDataService.csproj", "MachineSqlDataService/"]
COPY ["Data.Sql/Data.Sql.csproj", "Data.Sql/"]
COPY ["Interfaces/Interfaces.csproj", "Interfaces/"]
COPY ["Models/Models.csproj", "Models/"]
COPY ["Framework/EventBus/EventBus.csproj", "Framework/EventBus/"]
COPY ["Framework/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj", "Framework/EventBus.RabbitMQ/"]
COPY ["Framework/Extensions/Extensions.csproj", "Framework/Extensions/"]
COPY ["Framework/Logging/Logging.csproj", "Framework/Logging/"]
COPY ["Framework/Logging.NLog.Impl.Castle/Logging.NLog.Impl.Castle.csproj", "Framework/Logging.NLog.Impl.Castle/"]
COPY ["EventLogger.NLog/EventLogger.NLog.csproj", "EventLogger.NLog/"]
RUN dotnet restore "MachineSqlDataService/MachineSqlDataService.csproj"
COPY . .
WORKDIR "/src/MachineSqlDataService"
RUN dotnet build "MachineSqlDataService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MachineSqlDataService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MachineSqlDataService.dll"]