using System.Collections.Concurrent;
using System.Text;
using Budget.Services.SignalR;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Budget.Services.MQ.Subscribers;

public class RabbitMqListener: BackgroundService, IAsyncDisposable
{
    private readonly IHubContext<MessageHub> _context;
    private readonly IChannel _channel;
    private readonly IConnection _connection;
    private readonly ConcurrentDictionary<string, string> _clientQueues = new();
    
    private const string ExchangeName = "budgets";
    
    public RabbitMqListener(IHubContext<MessageHub> context)
    {
        _context = context;
        
        var factory = new ConnectionFactory{ HostName = "localhost", UserName = "guest", Password = "guest" };
        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
        
        _channel.ExchangeDeclareAsync(exchange: ExchangeName, type: ExchangeType.Topic, durable: true);
    }
    
    public void SubscribeClient(string clientId, string routingKey)
    {
        if (_clientQueues.ContainsKey(clientId))
            return; // Client already subscribed

        // Create a unique queue for the client
        var queueName = $"queue_{clientId}_{Guid.NewGuid()}";
        _clientQueues[clientId] = queueName;

        _channel.QueueDeclareAsync(queue: queueName,
            durable: true,
            exclusive: false, // Queue will be deleted when client disconnects
            autoDelete: true,
            arguments: null);

        _channel.QueueBindAsync(queue: queueName, exchange: ExchangeName, routingKey: routingKey);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync +=  (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var receivedRoutingKey = ea.RoutingKey;

            // Send message only to the connected client
            _context.Clients.Client(clientId).SendAsync("ReceiveMessage", message);
            return Task.CompletedTask;
        };

        _channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);
    }

    public void UnsubscribeClient(string clientId)
    {
        if (_clientQueues.TryRemove(clientId, out var queueName))
        {
            _channel.QueueDeleteAsync(queueName);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.DisposeAsync();
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.CompletedTask;
}