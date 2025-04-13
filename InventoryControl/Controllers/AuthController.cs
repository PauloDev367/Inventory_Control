using InventoryControl.Requests;
using InventoryControl.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers;


[ApiController]
[Route("api/v1/")]
public class AuthController : ControllerBase
{
    private readonly IdentityService _identityService;

    public AuthController(IdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("users")]
    public async Task<IActionResult> Registrate([FromBody] CreateNewUserRequest request)
    {
        var user = await _identityService.CreateNewUser(request);
        if (user.Errors != null)
            return Ok(user);

        return BadRequest(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
        var token = await _identityService.Login(request);
        if (token.Errors != null)
        {
            return Ok(token);
        }

        return BadRequest(token);
    }
}
