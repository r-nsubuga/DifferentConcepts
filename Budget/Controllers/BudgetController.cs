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
    private readonly BudgetUpdateSubscriber _subscriber;

    public BudgetController(IBudgetService budgetService, BudgetUpdateSubscriber subscriber)
    {
        _budgetService = budgetService;
        _subscriber = subscriber;
    }
    
    //[Authorize]
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
    
    [HttpGet]
    [Route("subscribeToBudget")]
    public async Task<IActionResult> SubscribeToUpdate(int id)
    {
        _subscriber.SubscribeToBudgetUpdates("gjhfghdsyu00389", id, s => {var budgetId = id.ToString(); });
        return Ok();
    }
}