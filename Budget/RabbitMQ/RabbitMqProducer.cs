using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Budget.RabbitMQ;

public class RabbitMqProducer:IRabbitMqProducer
{
    public async Task SendProductMessage<T>(T message)
    {
        var factory = new ConnectionFactory
        {
           HostName = "23.88.107.221",
           UserName = "guest",
           Password = "guest",
        };
        
        var connection = await factory.CreateConnectionAsync();

        await using var channel = await connection.CreateChannelAsync();
        
        await channel
            .QueueDeclareAsync("testQueue", false, false, false, null);
        
        var messageBody = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(messageBody);

        // Publish the message
        await channel.BasicPublishAsync(
            exchange: "",
            routingKey: "testQueue",
            mandatory: false,
            body: body
        );
    }
    
    
}