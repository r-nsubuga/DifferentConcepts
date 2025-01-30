using Budget.Entities;

namespace Budget.Domain.Tests;

public class BudgetTests
{
    [Fact]
    public void Should_Create_Budget()
    {
        var budget = new BudgetEntity();
        
        Assert.Single(budget.GetDomainEvents());
    }
}