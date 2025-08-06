using CarSpace.Services.Core.Contracts.CarService.CarServiceCategory.Requests;
using CarSpace.Services.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpace.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarServiceCategoryController : BaseWebApiController
{
    private readonly ICarServiceCategoryService _categoryService;

    public CarServiceCategoryController(ICarServiceCategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _categoryService.GetAllAsync();

        return Ok(categories);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _categoryService.GetByIdAsync(id);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Create([FromBody] CreateCarServiceCategoryRequest request)
    {
        await _categoryService.CreateCarServiceCategoryAsync(request);

        return Ok();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCarServiceCategoryRequest request)
    {
        var updated = await _categoryService.UpdateCarServiceCategoryAsync(request);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _categoryService.DeleteCarServiceCategoryAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
