namespace Budget.Services.MQ;

public interface IRabbitMqService
{
    Task Publish(string exchangeName, string routingKey, object message);
    Task Subscribe(string queueName, string exchangeName, string routingKey, Action<string> onMessageReceived);
}