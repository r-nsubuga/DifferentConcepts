using MediatR;

namespace Budget.Events;

public class BudgetCreatedEvent(int id, string name): INotification
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
}