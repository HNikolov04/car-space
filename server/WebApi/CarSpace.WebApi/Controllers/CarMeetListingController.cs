using CarSpace.Services.Core.Contracts.CarMeet.Requests;
using CarSpace.Services.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpace.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarMeetListingController : BaseWebApiController
{
    private readonly ICarMeetListingService _carMeetService;

    public CarMeetListingController(ICarMeetListingService carMeetService)
    {
        _carMeetService = carMeetService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetCarMeets([FromQuery] GetCarMeetListingsRequest request)
    {
        Guid? userId = IsAuthenticated() ? GetUserId() : null;

        var result = await _carMeetService.GetCarMeetsAsync(request, userId);

        return Ok(result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        Guid? userId = IsAuthenticated() ? GetUserId() : null;

        var result = await _carMeetService.GetCarMeetByIdAsync(id, userId);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateCarMeetListingRequest request)
    {
        var userId = GetUserId();

        var result = await _carMeetService.CreateCarMeetAsync(request, userId);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromForm] UpdateCarMeetListingRequest request)
    {
        var userId = GetUserId();

        var updated = await _carMeetService.UpdateCarMeetAsync(request, userId);

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

        await _carMeetService.DeleteCarMeetAsync(id, userId);

        return NoContent();
    }

    [HttpPost("{id}/save")]
    public async Task<IActionResult> Save(Guid id)
    {
        var userId = GetUserId();

        var success = await _carMeetService.SaveCarMeetAsync(id, userId);

        if (!success)
        {
            return BadRequest("Already saved or invalid.");
        }

        return Ok();
    }

    [HttpDelete("{id}/unsave")]
    public async Task<IActionResult> Unsave(Guid id)
    {
        var userId = GetUserId();

        var success = await _carMeetService.UnsaveCarMeetAsync(id, userId);

        if (!success)
        {
            return NotFound("Save not found.");
        }

        return Ok();
    }

    [HttpPost("{id}/join")]
    public async Task<IActionResult> Join(Guid id)
    {
        var userId = GetUserId();

        var success = await _carMeetService.JoinCarMeetAsync(id, userId);

        if (!success)
        {
            return BadRequest("Already joined or invalid.");
        }

        return Ok();
    }

    [HttpDelete("{id}/leave")]
    public async Task<IActionResult> Leave(Guid id)
    {
        var userId = GetUserId();

        var success = await _carMeetService.LeaveCarMeetAsync(id, userId);

        if (!success)
        {
            return NotFound("Join not found.");
        }

        return Ok();
    }

    [HttpGet("{id}/participants")]
    [AllowAnonymous]
    public async Task<IActionResult> GetParticipants(Guid id, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _carMeetService.GetCarMeetParticipantsAsync(id, page, pageSize);

        return Ok(result);
    }
}
