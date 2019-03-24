FROM microsoft/dotnet:2.2-runtime-stretch-slim AS base
WORKDIR /app


FROM microsoft/dotnet:2.2-sdk-stretch AS build
WORKDIR /src
COPY ["MachineInfluxDataService/MachineInfluxDataService.csproj", "MachineInfluxDataService/"]
COPY ["Models/Models.csproj", "Models/"]
COPY ["Framework/EventBus/EventBus.csproj", "Framework/EventBus/"]
COPY ["Interfaces/Interfaces.csproj", "Interfaces/"]
COPY ["Framework/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj", "Framework/EventBus.RabbitMQ/"]
COPY ["Framework/Extensions/Extensions.csproj", "Framework/Extensions/"]
COPY ["Framework/Logging/Logging.csproj", "Framework/Logging/"]
COPY ["Framework/Logging.NLog.Impl.Castle/Logging.NLog.Impl.Castle.csproj", "Framework/Logging.NLog.Impl.Castle/"]
COPY ["Data.Influx/Data.Influx.csproj", "Data.Influx/"]
COPY ["EventLogger.NLog/EventLogger.NLog.csproj", "EventLogger.NLog/"]
RUN dotnet restore "MachineInfluxDataService/MachineInfluxDataService.csproj"
COPY . .
WORKDIR "/src/MachineInfluxDataService"
RUN dotnet build "MachineInfluxDataService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MachineInfluxDataService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MachineInfluxDataService.dll"]