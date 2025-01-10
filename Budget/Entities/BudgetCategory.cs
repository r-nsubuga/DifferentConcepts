namespace Budget.Entities;

public class BudgetCategory: BaseEntity
{
    public string? EventType { get; set; }
    public ICollection<BudgetExpense> Expenses { get; set; }
    public double? PricePercentage { get; set; }
}