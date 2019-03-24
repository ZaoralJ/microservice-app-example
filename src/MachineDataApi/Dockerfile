FROM microsoft/dotnet:2.2-aspnetcore-runtime-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk-stretch AS build
WORKDIR /src
COPY ["MachineDataApi/MachineDataApi.csproj", "MachineDataApi/"]
COPY ["Data.Sql/Data.Sql.csproj", "Data.Sql/"]
COPY ["Interfaces/Interfaces.csproj", "Interfaces/"]
COPY ["Models/Models.csproj", "Models/"]
COPY ["Framework/EventBus/EventBus.csproj", "Framework/EventBus/"]
COPY ["Framework/Logging.NLog.Impl.Castle/Logging.NLog.Impl.Castle.csproj", "Framework/Logging.NLog.Impl.Castle/"]
COPY ["Framework/Logging/Logging.csproj", "Framework/Logging/"]
COPY ["Data.Influx/Data.Influx.csproj", "Data.Influx/"]
RUN dotnet restore "MachineDataApi/MachineDataApi.csproj"
COPY . .
WORKDIR "/src/MachineDataApi"
RUN dotnet build "MachineDataApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MachineDataApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MachineDataApi.dll"]