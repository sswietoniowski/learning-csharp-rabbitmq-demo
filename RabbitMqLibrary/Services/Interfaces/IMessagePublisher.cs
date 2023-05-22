namespace RabbitMqLibrary.Services.Interfaces;

public interface IMessagePublisher<in T> where T : class
{
    public string ExchangeName { get; set; }
    public string QueueName { get; set; }
    public string RoutingKey { get; set; }

    void Publish(T message);
}