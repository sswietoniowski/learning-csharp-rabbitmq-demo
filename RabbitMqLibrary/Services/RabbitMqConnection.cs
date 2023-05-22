using System.Reflection;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMqLibrary.Services.Interfaces;

namespace RabbitMqLibrary.Services;

public class RabbitMqConnection : IRabbitMqConnection
{
    private IConnection? _connection;
    private readonly ConnectionFactory _factory;

    public RabbitMqConnection(IConfiguration configuration)
    {
        var hostName = configuration["RabbitMq:HostName"];
        var port = configuration["RabbitMq:Port"];
        var userName = configuration["RabbitMq:UserName"];
        var password = configuration["RabbitMq:Password"];

        _factory = new ConnectionFactory();
        _factory.Uri = new Uri($"amqp://{userName}:{password}@{hostName}:{port}");
        _factory.ClientProvidedName = Assembly.GetEntryAssembly()?.GetName().Name ?? 
                                      Assembly.GetExecutingAssembly().GetName().Name;
    }

    public IConnection GetConnection()
    {
        if (_connection is IConnection { IsOpen: false })
        {
            _connection = _factory.CreateConnection();
        }

        return _connection!;
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}