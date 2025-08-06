using CarSpace.Services.Core.Contracts.CarService.CarServiceListing.Requests;
using CarSpace.Services.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpace.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarServiceListingController : BaseWebApiController
{
    private readonly ICarServiceListingService _carServiceListingService;

    public CarServiceListingController(ICarServiceListingService carServiceListingService)
    {
        _carServiceListingService = carServiceListingService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetServiceListings([FromQuery] GetCarServiceListingsRequest request)
    {
        Guid? userId = IsAuthenticated() ? GetUserId() : null;

        var result = await _carServiceListingService.GetCarServiceListingsAsync(request, userId);

        return Ok(result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        Guid? userId = IsAuthenticated() ? GetUserId() : null;

        var listing = await _carServiceListingService.GetCarServiceListingByIdAsync(id, userId);

        if (listing is null)
        {
            return NotFound();
        }

        return Ok(listing);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateCarServiceListingRequest request)
    {
        var userId = GetUserId();

        var response = await _carServiceListingService.CreateCarServiceListingAsync(request, userId);

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromForm] UpdateCarServiceListingRequest request)
    {
        var userId = GetUserId();

        var updated = await _carServiceListingService.UpdateCarServiceListingAsync(request, userId);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost("{id}/save")]
    public async Task<IActionResult> Save(Guid id)
    {
        var userId = GetUserId();

        var success = await _carServiceListingService.SaveCarServiceListingAsync(id, userId);

        if (!success)
        {
            return BadRequest("Listing already saved or invalid.");
        }

        return Ok("Saved successfully.");
    }

    [HttpDelete("{id}/unsave")]
    public async Task<IActionResult> Unsave(Guid id)
    {
        var userId = GetUserId();

        var success = await _carServiceListingService.UnsaveCarServiceListingAsync(id, userId);

        if (!success)
        {
            return NotFound("Save not found or already removed.");
        }

        return Ok("Unsave successful.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = GetUserId();

        await _carServiceListingService.DeleteCarServiceListingAsync(id, userId);

        return NoContent();
    }
}
