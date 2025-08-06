using CarSpace.Services.Core.Contracts.CarForum.CarForumArticle.Requests;
using CarSpace.Services.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpace.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarForumArticleController : BaseWebApiController
{
    private readonly ICarForumArticleService _forumService;

    public CarForumArticleController(ICarForumArticleService forumService)
    {
        _forumService = forumService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetArticles([FromQuery] GetCarForumArticlesRequest request)
    {
        Guid? userId = IsAuthenticated() ? GetUserId() : null;

        var result = await _forumService.GetCarForumArticlesAsync(request, userId);

        return Ok(result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        Guid? userId = IsAuthenticated() ? GetUserId() : null;

        var article = await _forumService.GetCarForumArticleByIdAsync(id, userId);

        return article is null ? NotFound() : Ok(article);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCarForumArticleRequest request)
    {
        var userId = GetUserId();

        var response = await _forumService.CreateCarForumArticleAsync(request, userId);

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateCarForumArticleRequest request)
    {
        var userId = GetUserId();

        var updated = await _forumService.UpdateCarForumArticleAsync(request.Id, request, userId);

        return updated ? NoContent() : Forbid();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = GetUserId();

        var deleted = await _forumService.DeleteCarForumArticleAsync(id, userId);

        return deleted ? NoContent() : Forbid();
    }

    [HttpPost("{id}/save")]
    public async Task<IActionResult> Save(Guid id)
    {
        var userId = GetUserId();

        var success = await _forumService.SaveCarForumArticleAsync(id, userId);

        return success ? Ok() : BadRequest("Already saved or invalid.");
    }

    [HttpDelete("{id}/unsave")]
    public async Task<IActionResult> Unsave(Guid id)
    {
        var userId = GetUserId();

        var success = await _forumService.UnsaveCarForumArticleAsync(id, userId);

        return success ? Ok() : NotFound("Save not found.");
    }
}
