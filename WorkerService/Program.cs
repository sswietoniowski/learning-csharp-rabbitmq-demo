using RabbitMqLibrary.Services;
using RabbitMqLibrary.Services.Interfaces;
using WorkerService;
using WorkerService.DTOs;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
        services.AddSingleton<IMessageConsumer<TodoDto>, MessageConsumer<TodoDto>>();
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();