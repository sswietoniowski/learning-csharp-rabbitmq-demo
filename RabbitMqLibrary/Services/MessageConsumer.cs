using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMqLibrary.Services.Interfaces;

namespace RabbitMqLibrary.Services;

public class MessageConsumer<T> : IMessageConsumer<T> where T : class
{
    private readonly IRabbitMqConnection _rabbitMqConnection;

    public string ExchangeName { get; set; } = "Exchange";
    public string QueueName { get; set; } = "Queue";
    public string RoutingKey { get; set; } = "RoutingKey";
    

    public MessageConsumer(IRabbitMqConnection rabbitMqConnection)
    {
        _rabbitMqConnection = rabbitMqConnection;
    }

    public T? Consume()
    {
        using var connection = _rabbitMqConnection.GetConnection();
        using var channel = connection.CreateModel();
        
        channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, true, false, null);
        channel.QueueDeclare(QueueName, false, false, false, null);
        channel.QueueBind(QueueName, ExchangeName, RoutingKey, null);

        BasicGetResult result = channel.BasicGet(QueueName, true);
        
        if (result == null)
        {
            return null;
        }
        
        string serializedMessage = Encoding.UTF8.GetString(result.Body.ToArray());
        T? message = JsonSerializer.Deserialize<T>(serializedMessage);
        
        return message;
    }
}