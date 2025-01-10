namespace Budget.Dtos;

public class CreateBudgetDto
{
    public string? Name { get; set; }
    public decimal? EstimatedAmount { get; set; }
}