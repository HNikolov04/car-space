using CarSpace.Services.Core.Contracts.CarServiceListing.Requests;
using CarSpace.Services.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarSpace.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarServiceListingController : ControllerBase
    {
        private readonly ICarServiceListingService _carServiceListingService;

        public CarServiceListingController(ICarServiceListingService carServiceListingService)
        {
            _carServiceListingService = carServiceListingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _carServiceListingService.GetAllCarServiceListingsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var listing = await _carServiceListingService.GetCarServiceListingByIdAsync(id);

            if (listing is null)
            {
                return NotFound();
            }

            return Ok(listing);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCarServiceListingRequest request)
        {
            var response = await _carServiceListingService.CreateCarServiceListingAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCarServiceListingRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest("ID mismatch");
            }

            var updated = await _carServiceListingService.UpdateCarServiceListingAsync(request);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _carServiceListingService.DeleteCarServiceListingAsync(id);

            return NoContent();
        }
    }
}
