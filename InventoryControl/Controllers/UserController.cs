using InventoryControl.Requests;
using InventoryControl.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers;

[ApiController]
[Route("/api/v1/users")]
// [Authorize]
public class UserController : ControllerBase
{
    private readonly IdentityService _identityService;

    public UserController(IdentityService identityService)
    {
        _identityService = identityService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPaginatedUsers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _identityService.GetPaginatedUsersAsync(pageNumber, pageSize);
        return Ok(result);
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserRequest request)
    {
        var result = await _identityService.UpdateUserAsync(userId, request);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok("User updated.");
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var result = await _identityService.DeleteUserAsync(userId);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok("User removed.");
    }
}