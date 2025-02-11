namespace Budget.Services.MQ.Publishers;

public class BudgetUpdatePublisher
{
    private readonly IRabbitMqService _rabbitMqService;
    private const string ExchangeName = "budgets";

    public BudgetUpdatePublisher(IRabbitMqService rabbitMqService)
    {
        _rabbitMqService = rabbitMqService;
    }

    public void PublishBudgetUpdate(int budgetId, object message)
    {
        var routingKey = $"budget.updates.{budgetId}";
        _rabbitMqService.Publish(ExchangeName,routingKey,message);
    }
}