using Budget.Helpers;
using MediatR;

namespace Budget.Events;

public class BudgetCreatedEventHandler: INotificationHandler<BudgetCreatedEvent>
{
    private readonly ISearchService _searchService;

    public BudgetCreatedEventHandler(ISearchService searchService)
    {
        _searchService = searchService;
    }
    
    public async Task Handle(BudgetCreatedEvent notification, CancellationToken cancellationToken)
    {
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