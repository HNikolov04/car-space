using CarSpace.Services.Core.Contracts.CarMeet.Requests;
using CarSpace.Services.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpace.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarMeetController : ControllerBase
{
    private readonly ICarMeetService _carMeetService;

    public CarMeetController(ICarMeetService carMeetService)
    {
        _carMeetService = carMeetService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _carMeetService.GetAllCarMeetsAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _carMeetService.GetCarMeetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCarMeetRequest request)
    {
        var result = await _carMeetService.CreateCarMeetAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateCarMeetRequest request)
    {
        var success = await _carMeetService.UpdateCarMeetAsync(request);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _carMeetService.DeleteCarMeetAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/join")]
    public async Task<IActionResult> Join(Guid id, [FromQuery] Guid userId)
    {
        var success = await _carMeetService.JoinCarMeetAsync(id, userId);
        return success ? Ok() : NotFound();
    }
}
