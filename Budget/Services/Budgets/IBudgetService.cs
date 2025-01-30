using Budget.Entities;

namespace Budget.Services.Budgets;

public interface IBudgetService
{
    Task IndexBudget(BudgetEntity budget);
    Task CreateBudget(BudgetEntity budget);
}