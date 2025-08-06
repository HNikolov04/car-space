using CarSpace.Services.Core.Contracts.CarForum.CarForumComment.Requests;
using CarSpace.Services.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpace.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarForumCommentController : BaseWebApiController
{
    private readonly ICarForumCommentService _commentService;

    public CarForumCommentController(ICarForumCommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet("{articleId}/comments")]
    [AllowAnonymous]
    public async Task<IActionResult> GetComments([FromQuery] GetCarForumCommentsRequest request)
    {
        var result = await _commentService.GetCommentsForArticleAsync(request);

        return Ok(result);
    }

    [HttpPost("{articleId}/comments")]
    public async Task<IActionResult> AddComment([FromBody] CreateCarForumCommentRequest request)
    {
        var userId = GetUserId();

        var response = await _commentService.AddCommentAsync(request, userId);

        return CreatedAtAction(nameof(GetComments), new { request.ArticleId }, response);
    }

    [HttpDelete("comments/{commentId}")]
    public async Task<IActionResult> DeleteComment(Guid commentId)
    {
        var userId = GetUserId();

        var deleted = await _commentService.DeleteCommentAsync(commentId, userId);

        return deleted ? NoContent() : Forbid();
    }
}
