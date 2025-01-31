using Budget.Entities.Interfaces;

namespace Budget.Entities;

public class BaseEntity
{
    private List<IDomainEvent> _events= [];
    public int Id { get; set; }
    
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _events.AsReadOnly();
    
    public void ClearDomainEvents() => _events.Clear();
    
    public void AddDomainEvent(IDomainEvent @event) => _events.Add(@event);
}