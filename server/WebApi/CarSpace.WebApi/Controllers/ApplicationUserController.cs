using CarSpace.Services.Core.Contracts.User.Requests;
using CarSpace.Services.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpace.WebApi.Controllers;

[Route("api/[controller]")]
[Authorize]
public class ApplicationUserController : BaseWebApiController
{
    private readonly IApplicationUserService _userService;

    public ApplicationUserController(IApplicationUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterApplicationUserRequest request)
    {
        var result = await _userService.RegisterAsync(request);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(new { message = "Registration successful." });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginApplicationUserRequest request)
    {
        var response = await _userService.LoginAsync(request);

        return response is null
            ? Unauthorized("Invalid credentials.")
            : Ok(response);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = GetUserId();

        var user = await _userService.GetUserByIdAsync(userId);

        return user is null
            ? NotFound("User not found.")
            : Ok(user);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateProfile([FromForm] UpdateApplicationUserRequest request)
    {
        var userId = GetUserId();

        if (userId == Guid.Empty)
        {
            return Unauthorized();
        }

        var updated = await _userService.UpdateUserAsync(userId, request);

        return updated
            ? Ok("Profile updated successfully.")
            : BadRequest("Failed to update profile. Ensure current password is correct and new password meets the requirements.");
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAccount()
    {
        var userId = GetUserId();

        var deleted = await _userService.SoftDeleteUserAsync(userId);

        return deleted
            ? Ok("Account deleted successfully.")
            : BadRequest("Failed to delete account.");
    }
}
