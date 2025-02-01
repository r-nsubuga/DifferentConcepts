using Budget.Dtos;
using Budget.Entities;
using Budget.Services;
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
    
    //[Authorize]
    [HttpPost]
    [Route("createBudget")]
    public async Task<IActionResult> CreateBudget([FromBody] CreateBudgetDto budgetDto)
    {
        var budget = new BudgetEntity(budgetDto.Name, budgetDto.EstimatedAmount);
        await _budgetService.CreateBudget(budget);
        return Ok();
    }
}