using Budget.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchController: ControllerBase
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet]
    [Route("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string query,
        [FromQuery] string module = "budgets",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var indexName = module switch
        {
            "budgets" => "budgets_index",
            "services" => "services_index",
            "products" => "products_index",
            _ => throw new ArgumentException("Invalid module name")
        };
        var results = await _searchService.SearchAsync<dynamic>(query, indexName, page, pageSize);
        return Ok(results);
    }
}