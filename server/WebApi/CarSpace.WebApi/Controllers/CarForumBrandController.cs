using CarSpace.Services.Core.Contracts.CarForum.CarForumBrand.Requests;
using CarSpace.Services.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpace.WebApi.Controllers;

[Route("api/[controller]")]
public class CarForumBrandController : BaseWebApiController
{
    private readonly ICarForumBrandService _brandService;

    public CarForumBrandController(ICarForumBrandService brandService)
    {
        _brandService = brandService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetBrands()
    {
        var result = await _brandService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var brand = await _brandService.GetByIdAsync(id);

        return brand is null ? NotFound() : Ok(brand);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Create([FromBody] CreateCarForumBrandRequest request)
    {
        await _brandService.CreateAsync(request);

        return Ok("Brand created.");
    }

    [HttpPut]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Update([FromBody] UpdateCarForumBrandRequest request)
    {
        var success = await _brandService.UpdateAsync(request);

        return success ? Ok("Updated successfully.") : NotFound();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _brandService.DeleteAsync(id);

        return success ? Ok("Deleted successfully.") : NotFound();
    }
}
