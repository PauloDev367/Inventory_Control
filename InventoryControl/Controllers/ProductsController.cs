using InventoryControl.Requests;
using InventoryControl.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers;

[ApiController]
[Route("api/v1/products")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly ProductService _service;

    public ProductsController(ProductService productService)
    {
        _service = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] int page = 1,
        [FromQuery] int perPage = 10
    )
    {
        var data = await _service.GetProductsAsync(page, perPage);
        return Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateProductRequest request)
    {
        var data = await _service.CreateProductAsync(request);
        return Ok(data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateProductRequest request)
    {
        try
        {
            var data = await _service.UpdateProductAsync(request, id);
            return Ok(data);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = "Error when try to update product" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid id
    )
    {
        var data = await _service.GetOneById(id);
        if (data == null)
            return NotFound();
        return Ok(data);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        try
        {
            await _service.DeleteProductAsync(id);
            return Ok(new { message = "Deleted successfully" });
        }
        catch (Exception e)
        {
            return BadRequest(new { error = "Error when try to delete product" });
        }
    }
}