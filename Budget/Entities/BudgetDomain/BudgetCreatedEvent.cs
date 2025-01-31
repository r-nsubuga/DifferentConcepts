using Budget.Entities.Interfaces;

namespace Budget.Entities.BudgetDomain;

public class BudgetCreatedEvent:IDomainEvent
{
    public int Id { get; set; } = 1;
    public string Name { get; set; } = "Test";
}