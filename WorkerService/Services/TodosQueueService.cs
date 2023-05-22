using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using WorkerService.DTOs;
using WorkerService.Services.Interfaces;

namespace WorkerService.Services;

public class TodosQueueService : ITodosQueueService, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    
    public TodosQueueService()
    {
        ConnectionFactory connectionFactory = new();
        connectionFactory.Uri = new Uri("amqp://guest:guest@rmq:5672");
        connectionFactory.ClientProvidedName = "Todos WorkerService";
     
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
    }
    
    public Task<TodoDto?> ReceiveAsync()
    {
        string exchangeName = "UncompletedTodosExchange";
        string routingKey = "uncompleted-todos-routing-key";
        string queueName = "TodosQueue";

        _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true, false, null);
        _channel.QueueDeclare(queueName, false, false, false, null);
        _channel.QueueBind(queueName, exchangeName, routingKey, null);
        
        BasicGetResult result = _channel.BasicGet(queueName, true);
        
        if (result == null)
        {
            return Task.FromResult<TodoDto?>(null);
        }
        
        string message = Encoding.UTF8.GetString(result.Body.ToArray());
        TodoDto todoDto = JsonSerializer.Deserialize<TodoDto>(message);
        
        return Task.FromResult<TodoDto?>(todoDto);
    }

    public void Dispose()
    {
        _connection.Dispose();
        _channel.Dispose();
    }
}