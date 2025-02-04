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

    public async Task Subscribe(string queueName, string exchangeName, string routingKey, Action<string> onMessageReceived)
    {
        await _channel.QueueDeclareAsync(queue:queueName, durable:true, exclusive:false, autoDelete: false);
        Console.WriteLine($"Subscribed to queue {queueName}");
        await _channel.QueueBindAsync(queue:queueName, exchange:exchangeName, routingKey:routingKey);
        
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [x] Received {message}");
            onMessageReceived.Invoke(message);
            return Task.CompletedTask;
        };
        await _channel.BasicConsumeAsync(queueName, autoAck:true, consumer: consumer);
        
    }
    
    // public void Dispose()
    // {
    //     // TODO release managed resources here
    // }

    public async ValueTask DisposeAsync()
    {
        await _channel.DisposeAsync();
        await _connection.DisposeAsync();
    }
}