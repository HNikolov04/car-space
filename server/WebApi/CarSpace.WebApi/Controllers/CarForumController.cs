using CarSpace.Services.Core.Contracts.CarForum.Requests;
using CarSpace.Services.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarSpace.WebApi.Controllers;

[ApiController]
[Route("api/forum")]
public class CarForumController : ControllerBase
{
    private readonly ICarForumService _service;

    public CarForumController(ICarForumService service)
    {
        _service = service;
    }

    [HttpGet("articles")]
    public async Task<IActionResult> GetAllArticles()
    {
        return Ok(await _service.GetAllArticlesAsync());
    }

    [HttpGet("articles/{id}")]
    public async Task<IActionResult> GetArticle(Guid id)
    {
        var article = await _service.GetArticleByIdAsync(id);

        if (article is null)
        {
            return NotFound();
        }

        return Ok(article);
    }

    [HttpPost("articles")]
    public async Task<IActionResult> CreateArticle([FromBody] CreateCarForumArticleRequest request)
    {
        var response = await _service.AddArticleAsync(request);

        return CreatedAtAction(nameof(GetArticle), new { id = response.Id }, response);
    }

    [HttpDelete("articles/{id}")]
    public async Task<IActionResult> DeleteArticle(Guid id)
    {
        await _service.DeleteArticleAsync(id);

        return NoContent();
    }

    [HttpGet("articles/{articleId}/comments")]
    public async Task<IActionResult> GetComments(Guid articleId)
    {
        return Ok(await _service.GetCommentsForArticleAsync(articleId));
    }

    [HttpGet("comments/{id}")]
    public async Task<IActionResult> GetComment(Guid id)
    {
        var comment = await _service.GetCommentByIdAsync(id);

        if (comment is null)
        {
            return NotFound();
        }

        return Ok(comment);
    }

    [HttpPost("articles/{articleId}/comments")]
    public async Task<IActionResult> AddComment(Guid articleId, [FromBody] CreateCarForumCommentRequest request)
    {
        if (request.CarForumArticleId != articleId)
        {
            return BadRequest("Article ID mismatch.");
        }

        var response = await _service.AddCommentAsync(request);

        return CreatedAtAction(nameof(GetComment), new { id = response.Id }, response);
    }

    [HttpDelete("comments/{id}")]
    public async Task<IActionResult> DeleteComment(Guid id)
    {
        await _service.DeleteCommentAsync(id);

        return NoContent();
    }
}