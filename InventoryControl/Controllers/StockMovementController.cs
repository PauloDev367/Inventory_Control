using InventoryControl.Enums;
using InventoryControl.Models;
using InventoryControl.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers;

[ApiController]
[Route("api/v1/controller-movements")]
public class StockMovementController : ControllerBase
{
    private readonly StockMovementService _service;

    public StockMovementController(StockMovementService service)
    {
        _service = service;
    }

    [HttpGet("product/{id}")]
    public async Task<IActionResult> GetProductStockMovementByIdAsync(
        [FromRoute] Guid id,
        [FromQuery] MovementType? type,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
    )
    {
        var data = await _service.GetAllProductStockMovement(page, pageSize, id, type);
        return Ok(data);
    }
}