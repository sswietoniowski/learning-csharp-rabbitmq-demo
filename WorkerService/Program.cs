using WorkerService;
using WorkerService.Services;
using WorkerService.Services.Interfaces;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<ITodosQueueService, TodosQueueService>();
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();