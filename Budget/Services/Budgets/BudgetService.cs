using Budget.Entities;
using Budget.Events;
using Budget.Helpers;
using Budget.Repositories;
using MediatR;

namespace Budget.Services.Budgets;

public class BudgetService: IBudgetService
{
    private readonly ISearchService _searchService;
    private readonly IBudgetRepository _budgetRepository;
    private readonly IPublisher _publisher;

    public BudgetService(ISearchService searchService, IBudgetRepository budgetRepository,
        IPublisher publisher)
    {
        _searchService = searchService;
        _budgetRepository = budgetRepository;
        _publisher = publisher;
    }
    
    public async Task IndexBudget(BudgetEntity budget)
    {
        var indexData = new
        {
            Id = budget.Id,
            Name = budget.Name,
        };
        await _searchService.IndexAsync(indexData, "budgets_index");
    }

    public async Task CreateBudget(BudgetEntity budget)
    {
        var budgetResponse = await _budgetRepository.CreateBudget(budget);
        var @event = new BudgetCreatedEvent(budgetResponse.Id, budgetResponse.Name);
        await _publisher.Publish(@event);
    }
}