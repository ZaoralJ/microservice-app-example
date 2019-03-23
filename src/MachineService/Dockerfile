FROM microsoft/dotnet:2.2-aspnetcore-runtime-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk-stretch AS build
WORKDIR /src
COPY ["MachineService/MachineService.csproj", "MachineService/"]
COPY ["Models/Models.csproj", "Models/"]
COPY ["Framework/EventBus/EventBus.csproj", "Framework/EventBus/"]
COPY ["Framework/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj", "Framework/EventBus.RabbitMQ/"]
COPY ["Framework/Extensions/Extensions.csproj", "Framework/Extensions/"]
COPY ["Framework/Logging/Logging.csproj", "Framework/Logging/"]
COPY ["Framework/Logging.NLog.Impl.Castle/Logging.NLog.Impl.Castle.csproj", "Framework/Logging.NLog.Impl.Castle/"]
RUN dotnet restore "MachineService/MachineService.csproj"
COPY . .
WORKDIR "/src/MachineService"
RUN dotnet build "MachineService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MachineService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MachineService.dll"]