using System.Text;
using Budget.Services.SignalR;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Budget.Services.MQ.Subscribers;

public class RabbitMqListener: BackgroundService, IAsyncDisposable
{
    private readonly IHubContext<MessageHub> _context;
    private IChannel _channel;
    private IConnection _connection;
    
    public RabbitMqListener(IHubContext<MessageHub> context)
    {
        _context = context;
        
        var factory = new ConnectionFactory{ HostName = "localhost", UserName = "guest", Password = "guest" };
        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
        
        var ExchangeName = "budgets";
        _channel.ExchangeDeclareAsync(exchange: ExchangeName, type: ExchangeType.Topic, durable: true);
        
        var queueName = "budget.queue";
        //var routingKeyPattern = "event.specific.#"; 
        
        _channel.QueueDeclareAsync(queue:queueName, durable:true, exclusive:false, autoDelete: false);
        Console.WriteLine($"Subscribed to queue {queueName}");
        _channel.QueueBindAsync(queue:queueName, exchange:ExchangeName, routingKey:"budget.#");
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var routingKey = ea.RoutingKey;
            
            _context.Clients.All.SendAsync("ReceiveMessage", message);
            Console.WriteLine(" [x] Received {0}", message);
            return Task.CompletedTask;
        };
        var queueName = "budget.queue";
        await _channel.BasicConsumeAsync(queueName, autoAck:true, consumer: consumer);
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.DisposeAsync();
    }
}