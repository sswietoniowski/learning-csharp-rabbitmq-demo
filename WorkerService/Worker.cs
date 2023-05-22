using WorkerService.DTOs;
using WorkerService.Services.Interfaces;

namespace WorkerService;

public class Worker : BackgroundService
{
    const int DELAY_IN_MILLISECONDS = 1000 * 2; // 2 seconds
    
    private readonly ILogger<Worker> _logger;
    private readonly ITodosQueueService _todosQueueService;

    public Worker(ILogger<Worker> logger, ITodosQueueService todosQueueService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _todosQueueService = todosQueueService ?? throw new ArgumentNullException(nameof(todosQueueService));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            
            TodoDto? todoDto = await _todosQueueService.ReceiveAsync();
            
            if (todoDto is null) 
            {
                _logger.LogInformation("No todo received");
                continue;
            }
            
            _logger.LogInformation("Received todo: {todoDto}", todoDto?.Title);
            
            await Task.Delay(DELAY_IN_MILLISECONDS, stoppingToken);
        }
    }
}