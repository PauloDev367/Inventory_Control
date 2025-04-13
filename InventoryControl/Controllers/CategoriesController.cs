using InventoryControl.Requests;
using InventoryControl.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers;

[ApiController]
[Route("api/v1/categories")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly CategoryService _service;

    public CategoriesController(CategoryService categoryService)
        => _service = categoryService;

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] int page = 1,
        [FromQuery] int perPage = 10
    )
    {
        var data = await _service.GetCategoriesAsync(page, perPage);
        return Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCategoryRequest request)
    {
        var data = await _service.CreateCategoryAsync(request);
        return Ok(data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateCategoryRequest request)
    {
        try
        {
            var data = await _service.UpdateCategoryAsync(request, id);
            return Ok(data);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = "Error when try to update category" });
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
            await _service.DeleteCategoryAsync(id);
            return Ok(new { message = "Deleted successfully" });
        }
        catch (Exception e)
        {
            return BadRequest(new { error = "Error when try to delete category" });
        }
    }
}