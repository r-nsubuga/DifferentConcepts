using Budget.Entities;

namespace Budget.Services;

public interface IBudgetService
{
    Task IndexBudget(BudgetEntity budget);
    Task CreateBudget(BudgetEntity budget);
}