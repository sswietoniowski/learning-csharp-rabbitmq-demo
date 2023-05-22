using RabbitMqLibrary.Services.Interfaces;
using WorkerService.DTOs;

namespace WorkerService;

public class Worker : BackgroundService
{
    const int DELAY_IN_MILLISECONDS = 1000 * 10; // 10 seconds
    
    private readonly ILogger<Worker> _logger;
    private readonly IMessageConsumer<TodoDto> _messageConsumer;

    public Worker(ILogger<Worker> logger, IMessageConsumer<TodoDto> messageConsumer)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _messageConsumer = messageConsumer ?? throw new ArgumentNullException(nameof(messageConsumer));
        
        _messageConsumer.QueueName = "Todos";
        _messageConsumer.ExchangeName = "Todos Exchange";
        _messageConsumer.RoutingKey = "Todos Routing Key";
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(DELAY_IN_MILLISECONDS, stoppingToken);

            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            
            TodoDto? todoDto = _messageConsumer.Consume();
            
            if (todoDto is null) 
            {
                _logger.LogInformation("No todo received");
                continue;
            }
            
            _logger.LogInformation("Received todo: {todoDto}", todoDto?.Title);
        }
    }
}