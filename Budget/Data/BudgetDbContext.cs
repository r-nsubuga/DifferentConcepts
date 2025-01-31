using Budget.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Budget.Data;

public class BudgetDbContext: DbContext
{
    private readonly IMediator _mediator;
    
    public DbSet<BudgetCategory> Categories { get; set; }
    public DbSet<BudgetEntity> Budgets { get; set; }
    public DbSet<BudgetExpense> Expenses { get; set; }
    
    public BudgetDbContext(DbContextOptions options, IMediator mediator): base(options)
    {
        _mediator = mediator;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        var domainEvents = ChangeTracker
            .Entries<BaseEntity>() // Ensure BaseEntity contains domain event logic
            .Where(entry => entry.Entity.GetDomainEvents().Any()) // Filter entities with events
            .SelectMany(entry => entry.Entity.GetDomainEvents())
            .ToList();

         foreach (var domainEvent in domainEvents)
        {
           await _mediator.Publish(domainEvent, cancellationToken);
        }

        
        return result;
    }
}