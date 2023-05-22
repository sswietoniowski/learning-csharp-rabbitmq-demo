FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS="http://*:5000"
ENV RabbitMQ__Host="rmq"
ENV RabbitMQ__Port="5672"
ENV RabbitMQ__Username="guest"
ENV RabbitMQ__Password="guest"
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src/WebAPI
COPY ["WebAPI.csproj", "./"]
RUN dotnet restore "WebAPI.csproj"
COPY . .
WORKDIR "/src/WebAPI"
RUN dotnet build "WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAPI.dll"]
