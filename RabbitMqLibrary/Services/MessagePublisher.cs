using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMqLibrary.Services.Interfaces;

namespace RabbitMqLibrary.Services;

public class MessagePublisher<T> : IMessagePublisher<T> where T : class
{
    private readonly IRabbitMqConnection _rabbitMqConnection;

    public string ExchangeName { get; set; } = "Exchange";
    public string QueueName { get; set; } = "Queue";
    public string RoutingKey { get; set; } = "RoutingKey";
    

    public MessagePublisher(IRabbitMqConnection rabbitMqConnection)
    {
        _rabbitMqConnection = rabbitMqConnection;
    }

    public void Publish(T message)
    {
        using var connection = _rabbitMqConnection.GetConnection();
        using var channel = connection.CreateModel();
        
        channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, true, false, null);
        channel.QueueDeclare(QueueName, false, false, false, null);
        channel.QueueBind(QueueName, ExchangeName, RoutingKey, null);
        
        var serializedMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(serializedMessage);
        
        channel.BasicPublish(exchange: ExchangeName,
            routingKey: RoutingKey,
            basicProperties: null,
            body: body);
    }
}