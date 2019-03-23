FROM microsoft/dotnet:2.2-runtime-stretch-slim AS base
WORKDIR /app


FROM microsoft/dotnet:2.2-sdk-stretch AS build
WORKDIR /src
COPY ["MachineDataService/MachineDataService.csproj", "MachineDataService/"]
COPY ["Framework/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj", "Framework/EventBus.RabbitMQ/"]
COPY ["Framework/Extensions/Extensions.csproj", "Framework/Extensions/"]
COPY ["Framework/EventBus/EventBus.csproj", "Framework/EventBus/"]
COPY ["Framework/Logging/Logging.csproj", "Framework/Logging/"]
COPY ["Framework/Logging.NLog.Impl.Castle/Logging.NLog.Impl.Castle.csproj", "Framework/Logging.NLog.Impl.Castle/"]
RUN dotnet restore "MachineDataService/MachineDataService.csproj"
COPY . .
WORKDIR "/src/MachineDataService"
RUN dotnet build "MachineDataService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MachineDataService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MachineDataService.dll"]