using RabbitMQ.Client;

namespace RabbitMqLibrary.Services.Interfaces;

public interface IRabbitMqConnection : IDisposable
{
    IConnection GetConnection();
}