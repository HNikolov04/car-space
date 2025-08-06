using CarSpace.Services.Core.Contracts.About.Request;
using CarSpace.Services.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpace.WebApi.Controllers;

[Route("api/[controller]")]
[Authorize]
public class AboutUsController : BaseWebApiController
{
    private readonly IAboutUsService _service;

    public AboutUsController(IAboutUsService service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAboutUs()
    {
        var result = await _service.GetAsync();
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateAboutUs([FromBody] UpdateAboutUsRequest request)
    {
        await _service.UpdateAsync(request);
        return NoContent();
    }
}
