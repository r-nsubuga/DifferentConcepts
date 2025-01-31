using Budget.Entities.BudgetDomain;
using Budget.Enums;

namespace Budget.Entities;

public class BudgetEntity: BaseEntity
{
    public string Name { get; set; }
    public ICollection<BudgetCategory>? BudgetCategories { get; set; }
    public decimal? EstimatedTotalAmount { get; set; }
    
    public decimal? ActualTotalAmount { get; set; }

    public BudgetEntity(string name, decimal? estimatedTotalAmount)
    {
        Name = name;
        EstimatedTotalAmount = estimatedTotalAmount;
        AddDomainEvent(new BudgetCreatedEvent());
    }
} 