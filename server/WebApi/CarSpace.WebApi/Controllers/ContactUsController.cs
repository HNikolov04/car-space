using CarSpace.Services.Core.Contracts.Contact.Requests;
using CarSpace.Services.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpace.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContactUsController : BaseWebApiController
{
    private readonly IContactUsService _service;

    public ContactUsController(IContactUsService service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get()
    {
        var result = await _service.GetAsync();

        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Update([FromBody] UpdateContactUsRequest request)
    {
        await _service.UpdateAsync(request);

        return NoContent();
    }
}
