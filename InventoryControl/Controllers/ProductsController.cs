using InventoryControl.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers;

[ApiController]
[Route("api/v1/products")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _service;
    public ProductsController(ProductService productService)
    {
        _service = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
    [FromQuery] int page=1,
    [FromQuery] int perPage=10
    )
    {
        var data = await _service.GetProducts(page, perPage);
        return Ok(data);
    }

}
