using Budget.Dtos;
using Budget.Entities;
using Budget.Services.Budgets;
using Budget.Services.MQ.Subscribers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Controllers;

[ApiController]
[Route("[controller]")]
public class BudgetController: ControllerBase
{
    private readonly IBudgetService _budgetService;

    public BudgetController(IBudgetService budgetService)
    {
        _budgetService = budgetService;
    }
    
    [HttpPost]
    [Route("createBudget")]
    public async Task<IActionResult> CreateBudget([FromBody] CreateBudgetDto budgetDto)
    {
        var budget = new BudgetEntity
        {
            Name = budgetDto.Name,
            EstimatedTotalAmount = budgetDto.EstimatedAmount
        };
        await _budgetService.CreateBudget(budget);
        return Ok();
    }
}