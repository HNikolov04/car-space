using CarSpace.Services.Core.Contracts.CarShop.CarShopListing.Requests;
using CarSpace.Services.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpace.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarShopListingController : BaseWebApiController
{
    private readonly ICarShopListingService _service;

    public CarShopListingController(
        ICarShopListingService service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetShopListings([FromQuery] GetCarShopListingsRequest request)
    {
        Guid? userId = IsAuthenticated() ? GetUserId() : null;

        var result = await _service.GetCarShopListingsAsync(request, userId);

        return Ok(result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        Guid? userId = IsAuthenticated() ? GetUserId() : null;

        var result = await _service.GetCarShopListingByIdAsync(id, userId);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateCarShopListingRequest request)
    {
        var userId = GetUserId();

        var result = await _service.CreateCarShopListingAsync(request, userId);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromForm] UpdateCarShopListingRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest("ID mismatch.");
        }

        var userId = GetUserId();

        var updated = await _service.UpdateCarShopListingAsync(request, userId);

        if (!updated)
        {
            return Forbid();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = GetUserId();

        await _service.DeleteCarShopListingAsync(id, userId);

        return NoContent();
    }

    [HttpPost("{id}/save")]
    public async Task<IActionResult> Save(Guid id)
    {
        var userId = GetUserId();

        var success = await _service.SaveCarShopListingAsync(id, userId);

        if (!success)
        {
            return BadRequest("Already saved or invalid.");
        }

        return Ok("Saved successfully.");
    }

    [HttpDelete("{id}/unsave")]
    public async Task<IActionResult> Unsave(Guid id)
    {
        var userId = GetUserId();

        var success = await _service.UnsaveCarShopListingAsync(id, userId);

        if (!success)
        {
            return NotFound("Save not found.");
        }

        return Ok("Unsave successful.");
    }
}
