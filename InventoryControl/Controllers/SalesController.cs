using InventoryControl.Requests;
using InventoryControl.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers;

[ApiController]
[Route("api/v1/sales")]
[Authorize]
public class SalesController : ControllerBase
{
    private readonly SaleService _saleService;

    public SalesController(SaleService saleService)
    {
        _saleService = saleService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSaleRequest request)
    {
        var sale = await _saleService.CreateAsync(request);
        return Ok(sale);
    }

    [HttpGet("product/{id}")]
    public async Task<IActionResult> GetProductSales(
        Guid id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var data = await _saleService.GetProductSalesAsync(page, pageSize, id);
        return Ok(data);
    }
    [HttpGet]
    public async Task<IActionResult> GetAllSales(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var data = await _saleService.GetAllSalesAsync(page, pageSize);
        return Ok(data);
    }
}