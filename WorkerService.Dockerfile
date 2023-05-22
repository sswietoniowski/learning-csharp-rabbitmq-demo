FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app
ENV RabbitMQ__HostName="rmq"
ENV RabbitMQ__Port="5672"
ENV RabbitMQ__UserName="guest"
ENV RabbitMQ__Password="guest"

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RabbitMqLibrary/RabbitMqLibrary.csproj", "RabbitMqLibrary/"]
COPY ["WorkerService/WorkerService.csproj", "WorkerService/"]
RUN dotnet restore "WorkerService/WorkerService.csproj"
COPY . .
WORKDIR "/src/WorkerService"
RUN dotnet build "WorkerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorkerService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkerService.dll"]
