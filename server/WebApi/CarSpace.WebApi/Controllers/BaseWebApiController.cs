using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarSpace.WebApi.Controllers;

[ApiController]
[Authorize]
public abstract class BaseWebApiController : ControllerBase
{
    protected Guid GetUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return string.IsNullOrWhiteSpace(userId) ? Guid.Empty : Guid.Parse(userId);
    }

    protected bool IsAdministrator()
    {
        return User.IsInRole("Administrator");
    }

    protected bool IsAuthenticated()
    {
        return User.Identity?.IsAuthenticated ?? false;
    }
}