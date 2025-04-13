﻿using InventoryControl.Requests;
using InventoryControl.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers;

[ApiController]
[Route("api/v1/suppliers")]
public class SuppliersController: ControllerBase
{
    private readonly SupplierService _service;

    public SuppliersController(SupplierService supplierService)
        => _service = supplierService;

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] int page = 1,
        [FromQuery] int perPage = 10
    )
    {
        var data = await _service.GetSupplierAsync(page, perPage);
        return Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateSupplierRequest request)
    {
        var data = await _service.CreateSupplierAsync(request);
        return Ok(data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateSupplierRequest request)
    {
        try
        {
            var data = await _service.UpdateSupplierAsync(request, id);
            return Ok(data);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = "Error when try to update supplier" });
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
            await _service.DeleteSupplierAsync(id);
            return Ok(new { message = "Deleted successfully" });
        }
        catch (Exception e)
        {
            return BadRequest(new { error = "Error when try to delete supplier" });
        }
    }
}