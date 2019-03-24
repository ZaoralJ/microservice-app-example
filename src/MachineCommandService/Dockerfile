FROM microsoft/dotnet:2.2-aspnetcore-runtime-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk-stretch AS build
WORKDIR /src
COPY ["MachineStateService/MachineStateService.csproj", "MachineStateService/"]
RUN dotnet restore "MachineStateService/MachineStateService.csproj"
COPY . .
WORKDIR "/src/MachineStateService"
RUN dotnet build "MachineStateService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MachineStateService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MachineStateService.dll"]