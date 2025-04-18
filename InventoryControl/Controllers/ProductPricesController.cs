using InventoryControl.Data;
using InventoryControl.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers;

[ApiController]
[Route("/api/v1/products-prices")]
[Authorize]
public class ProductPricesController : ControllerBase
{
    private readonly ProductPriceService _productPriceService;

    public ProductPricesController(ProductPriceService productPriceService)
    {
        _productPriceService = productPriceService;
    }

    [HttpGet("product/{id}")]
    public async Task<IActionResult> GetProductPricesAsync(
        [FromRoute] Guid id,
        [FromQuery] int page = 1,
        [FromQuery] int perPage = 10)
    {
        var data = await _productPriceService.GetAllProductPriceFromProductAsync(page, perPage, id);
        return Ok(data);
    }
}