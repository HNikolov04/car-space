using CarSpace.Services.Core.Contracts.CarShop.CarShopBrand.Requests;
using CarSpace.Services.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpace.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CarShopBrandController : BaseWebApiController
{
    private readonly ICarShopBrandService _brandService;

    public CarShopBrandController(ICarShopBrandService brandService)
    {
        _brandService = brandService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetBrands()
    {
        var brands = await _brandService.GetAllCarShopBrandsAsync();

        return Ok(brands);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var brand = await _brandService.GetCarShopBrandByIdAsync(id);

        if (brand == null)
        {
            return NotFound();
        }

        return Ok(brand);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Create([FromBody] CreateCarShopBrandRequest request)
    {
        var response = await _brandService.CreateCarShopBrandAsync(request);

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCarShopBrandRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest("ID mismatch.");
        }

        var success = await _brandService.UpdateCarShopBrandAsync(request);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _brandService.DeleteCarShopBrandAsync(id);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}
