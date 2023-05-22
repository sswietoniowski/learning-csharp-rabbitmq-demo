using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Web.UI.DTOs;
using Web.UI.Services.Interfaces;

namespace Web.UI.Services;

public class TodosQueueService : ITodosQueueService, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public TodosQueueService()
    {
        ConnectionFactory connectionFactory = new();
        connectionFactory.Uri = new Uri("amqp://guest:guest@rmq:5672");
        connectionFactory.ClientProvidedName = "Todos WebAPI";
        
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
    }
    
    public Task SendAsync(TodoDto todoDto)
    {
        string exchangeName = "UncompletedTodosExchange";
        string routingKey = "uncompleted-todos-routing-key";
        string queueName = "TodosQueue";

        _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true, false, null);
        _channel.QueueDeclare(queueName, false, false, false, null);
        _channel.QueueBind(queueName, exchangeName, routingKey, null);

        var message = JsonSerializer.Serialize(todoDto);
        var messageBodyBytes = Encoding.UTF8.GetBytes(message);
        
        _channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);
        
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}