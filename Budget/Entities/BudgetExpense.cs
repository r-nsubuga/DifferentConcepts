namespace Budget.Entities;

public class BudgetExpense: BaseEntity
{
    public string Name { get; set; }
    public decimal? Amount { get; set; }
    public string? Note { get; set; }
    public int CategoryId { get; set; }
    public BudgetCategory Category { get; set; }
}