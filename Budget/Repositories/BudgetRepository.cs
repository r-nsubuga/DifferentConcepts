using Budget.Data;
using Budget.Entities;
using Budget.Events;
using MediatR;

namespace Budget.Repositories;

public class BudgetRepository: IBudgetRepository
{
    private readonly BudgetDbContext _context;
    private readonly IMediator _mediator;

    public BudgetRepository(BudgetDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }
    public async Task<BudgetEntity> CreateBudget(BudgetEntity budget)
    {
        await _context.Budgets.AddAsync(budget);
        await _context.SaveChangesAsync();
        await _mediator.Publish(new BudgetCreatedEvent(budget.Id, budget.Name));
        return budget;
    }
}