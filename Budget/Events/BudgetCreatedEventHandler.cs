using Budget.Helpers;
using Budget.Services.MQ;
using Budget.Services.MQ.Publishers;
using MediatR;

namespace Budget.Events;

public class BudgetCreatedEventHandler: INotificationHandler<BudgetCreatedEvent>
{
    private readonly ISearchService _searchService;
    private readonly BudgetUpdatePublisher _publisher;

    public BudgetCreatedEventHandler(ISearchService searchService, BudgetUpdatePublisher publisher)
    {
        _searchService = searchService;
        _publisher = publisher;
    }
    
    public async Task Handle(BudgetCreatedEvent notification, CancellationToken cancellationToken)
    {
        _publisher.PublishBudgetUpdate(notification.Id, notification.Name);
        // await _searchService.IndexAsync(new
        // {
        //     Id = notification.Id,
        //     Name = notification.Name,
        // }, "budgets_index");
        // if (!response.IsValidResponse)
        // {
        //     Console.WriteLine($"Failed to index product: {response.DebugInformation}");
        // }
    }
}