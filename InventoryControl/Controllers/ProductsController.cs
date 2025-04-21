using InventoryControl.Enums;
using InventoryControl.Requests;
using InventoryControl.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers;

[ApiController]
[Route("api/v1/products")]
// [Authorize]
public class ProductsController : ControllerBase
{
    private readonly ProductService _service;
    private readonly StockMovementService _serviceStockMovement;

    public ProductsController(ProductService productService, StockMovementService serviceStockMovement)
    {
        _service = productService;
        _serviceStockMovement = serviceStockMovement;
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

    [HttpPost("{id}/categories")]
    public async Task<IActionResult> AddCategoryToProduct(
        [FromBody] AddCategoryToProductRequest request,
        [FromRoute] Guid id
    )
    {
        await _service.AddCategoryToProductAsync(id, request);
        return Ok(new { message = "Category added successfully" });
    }

    [HttpDelete("{id}/categories")]
    public async Task<IActionResult> RemoveCategoryFromProduct(
        [FromBody] RemoveCategoryToProductRequest request,
        [FromRoute] Guid id
    )
    {
        await _service.RemoveCategoryFromProductAsync(id, request);
        return Ok(new { message = "Categories removed successfully" });
    }

    [HttpGet("{id}/categories")]
    public async Task<IActionResult> GetAllProductCategories(
        [FromRoute] Guid id
    )
    {
        var data = await _service.GetAllProductCategoriesAsync(id);
        return Ok(data);
    }

    [HttpPatch("{id}/stock-movement/add")]
    public async Task<IActionResult> AddStockToProductAsync(
        [FromRoute] Guid id,
        [FromBody] AddOrRemoveStockMovementToProductRequest request
    )
    {
        var product = await _service.RemoverOrAddProductQuantityAsync(id, request.Quantity, MovementType.IN);
        return Ok(product);
    }
    
    [HttpPatch("{id}/stock-movement/remove")]
    public async Task<IActionResult> RemoveStockToProductAsync(
        [FromRoute] Guid id,
        [FromBody] AddOrRemoveStockMovementToProductRequest request
    )
    {
        var product = await _service.RemoverOrAddProductQuantityAsync(id, request.Quantity, MovementType.OUT);
        return Ok(product);
    }
}