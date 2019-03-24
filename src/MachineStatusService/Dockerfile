FROM microsoft/dotnet:2.2-aspnetcore-runtime-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk-stretch AS build
WORKDIR /src
COPY ["MachineStatusService/MachineStatusService.csproj", "MachineStatusService/"]
RUN dotnet restore "MachineStatusService/MachineStatusService.csproj"
COPY . .
WORKDIR "/src/MachineStatusService"
RUN dotnet build "MachineStatusService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MachineStatusService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MachineStatusService.dll"]