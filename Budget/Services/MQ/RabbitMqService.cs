using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Budget.Services.MQ;

public class RabbitMqService: IAsyncDisposable, IRabbitMqService
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    
    public RabbitMqService(string hostName, string exchangeName, string exchangeType = ExchangeType.Topic)
    {
        var factory = new ConnectionFactory{ HostName = hostName, UserName = "guest", Password = "guest" };
        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
        _channel.ExchangeDeclareAsync(exchange: exchangeName, type: exchangeType, durable:true);
    }
    
    public async Task Publish(string exchangeName, string routingKey, object message)
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        await _channel.BasicPublishAsync(exchange: exchangeName, routingKey: routingKey, body: body);
        Console.WriteLine($"Published message to {routingKey}: {message}");
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.DisposeAsync();
        await _connection.DisposeAsync();
    }
}