FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app
ENV RabbitMQ__Host="rmq"
ENV RabbitMQ__Port="5672"
ENV RabbitMQ__Username="guest"
ENV RabbitMQ__Password="guest"

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src/WorkerService
COPY ["WorkerService.csproj", "./"]
RUN dotnet restore "WorkerService.csproj"

COPY . .

WORKDIR "/src/WorkerService"
RUN dotnet build "WorkerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorkerService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkerService.dll"]
