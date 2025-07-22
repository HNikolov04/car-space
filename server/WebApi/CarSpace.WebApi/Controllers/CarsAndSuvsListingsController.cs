using CarSpace.Services.Core.Contracts.CarShop.Requests;
using CarSpace.Services.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpace.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CarsAndSuvsListingsController : ControllerBase
{
    private readonly ICarsAndSuvsListingService _service;

    public CarsAndSuvsListingsController(ICarsAndSuvsListingService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllCarsAndSuvsListingsAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetCarsAndSuvsListingByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCarsAndSuvsListingRequest request)
    {
        var result = await _service.CreateCarsAndSuvsListingAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateCarsAndSuvsListingRequest request)
    {
        var updated = await _service.UpdateCarsAndSuvsListingAsync(request);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteCarsAndSuvsListingAsync(id);
        return NoContent();
    }
}
