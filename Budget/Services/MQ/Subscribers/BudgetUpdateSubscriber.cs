namespace Budget.Services.MQ.Subscribers;

public class BudgetUpdateSubscriber
{
    private readonly IRabbitMqService _rabbitMqService;
    private const string ExchangeName = "budgets";

    public BudgetUpdateSubscriber(IRabbitMqService rabbitMqService)
    {
        _rabbitMqService = rabbitMqService;
    }

    public void SubscribeToBudgetUpdates(string userId, int budgetId, Action<string> onMessageReceived)
    {
        var queueName = $"{userId}_queue";
        var routingKey = $"budget.updates.{budgetId}";
        Console.WriteLine($"Subscribing to {queueName}");
        _rabbitMqService.Subscribe(queueName,ExchangeName,routingKey, onMessageReceived);
    }
}