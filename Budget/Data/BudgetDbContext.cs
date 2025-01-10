using Budget.Entities;
using Microsoft.EntityFrameworkCore;

namespace Budget.Data;

public class BudgetDbContext: DbContext
{
    public DbSet<BudgetCategory> Categories { get; set; }
    public DbSet<BudgetEntity> Budgets { get; set; }
    public DbSet<BudgetExpense> Expenses { get; set; }
    
    public BudgetDbContext(DbContextOptions<BudgetDbContext> options): base(options)
    {
        
    }
}