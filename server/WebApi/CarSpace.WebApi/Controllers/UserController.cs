using CarSpace.Services.Core.Contracts.User.Requests;
using CarSpace.Services.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarSpace.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var result = await _userService.RegisterAsync(request.Email, request.Username, request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok("Registration successful.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        var result = await _userService.LoginAsync(request.Email, request.Password);

        if (!result.Succeeded)
        {
            return Unauthorized("Invalid credentials.");
        }

        return Ok("Login successful.");
    }
}
