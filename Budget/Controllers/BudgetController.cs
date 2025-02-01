using Budget.Dtos;
using Budget.Entities;
using Budget.RabbitMQ;
using Budget.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Controllers;

[ApiController]
[Route("[controller]")]
public class BudgetController: ControllerBase
{
    private readonly IBudgetService _budgetService;
    private readonly IRabbitMqProducer _rabbitMqProducer;

    public BudgetController(IBudgetService budgetService,IRabbitMqProducer rabbitMqProducer)
    {
        _budgetService = budgetService;
        _rabbitMqProducer = rabbitMqProducer;
    }
    
    //[Authorize]
    [HttpPost]
    [Route("createBudget")]
    public async Task<IActionResult> CreateBudget([FromBody] CreateBudgetDto budgetDto)
    {
        var budget = new BudgetEntity(budgetDto.Name, budgetDto.EstimatedAmount);
        await _budgetService.CreateBudget(budget);
        await _rabbitMqProducer.SendProductMessage(budget);
        return Ok();
    }
}