using Budget.Helpers;
using Budget.Entities.BudgetDomain;
using Budget.RabbitMQ;
using MediatR;

namespace Budget.Events;

public class BudgetCreatedEventHandler: INotificationHandler<BudgetCreatedEvent>
{
    private readonly ISearchService _searchService;
    private readonly IRabbitMqProducer _rabbitMqProducer;

    public BudgetCreatedEventHandler(ISearchService searchService,IRabbitMqProducer rabbitMqProducer)
    {
        _searchService = searchService;
        _rabbitMqProducer = rabbitMqProducer;
    }
    
    public async Task Handle(BudgetCreatedEvent notification, CancellationToken cancellationToken)
    {
        
        //Publish to broker for queues and processing eg Rabbitmq
        await _rabbitMqProducer.SendProductMessage(notification);
        await _searchService.IndexAsync(new
        {
            Id = notification.Id,
            Name = notification.Name,
        }, "budgets_index");
        // if (!response.IsValidResponse)
        // {
        //     Console.WriteLine($"Failed to index product: {response.DebugInformation}");
        // }
    }
}