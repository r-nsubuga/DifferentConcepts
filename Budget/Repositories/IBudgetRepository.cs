using Budget.Entities;

namespace Budget.Repositories;

public interface IBudgetRepository
{
    Task<BudgetEntity> CreateBudget(BudgetEntity budget);
}