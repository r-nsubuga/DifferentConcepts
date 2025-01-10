using Budget.Data;
using Budget.Entities;

namespace Budget.Repositories;

public class BudgetRepository: IBudgetRepository
{
    private readonly BudgetDbContext _context;

    public BudgetRepository(BudgetDbContext context)
    {
        _context = context;
    }
    public async Task<BudgetEntity> CreateBudget(BudgetEntity budget)
    {
        await _context.Budgets.AddAsync(budget);
        await _context.SaveChangesAsync();
        return budget;
    }
}