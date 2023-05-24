namespace RabbitMqLibrary.Services.Interfaces;

public interface IMessageConsumer<out T> where T : class
{
    public string ExchangeName { get; set; }
    public string QueueName { get; set; }
    public string RoutingKey { get; set; }

    T? Consume();
}