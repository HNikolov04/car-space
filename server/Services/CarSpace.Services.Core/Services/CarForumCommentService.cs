using CarSpace.Data.Models.Entities.CarForum;
using CarSpace.Data.Repositories.Interfaces;
using CarSpace.Services.Common.Messages;
using CarSpace.Services.Core.Contracts.CarForum.CarForumComment.Requests;
using CarSpace.Services.Core.Contracts.CarForum.CarForumComment.Responses;
using CarSpace.Services.Core.Exceptions;
using CarSpace.Services.Core.Services.Interfaces;

namespace CarSpace.Services.Core.Services;

public class CarForumCommentService : ICarForumCommentService
{
    private readonly ICarForumCommentRepository _carForumCommentRepository;

    public CarForumCommentService(ICarForumCommentRepository carForumCommentRepository)
    {
        _carForumCommentRepository = carForumCommentRepository;
    }

    public async Task<PaginatedCarForumCommentsResponse> GetCommentsForArticleAsync(GetCarForumCommentsRequest request)
    {
        var (comments, totalCount) = await _carForumCommentRepository.GetCommentsForArticleAsync(request.ArticleId, request.Page, request.PageSize);

        var items = comments.Select(c => new GetAllCarForumCommentsForArticlesResponse(
            c.Id,
            c.Content,
            c.CreatedAt,
            c.UserId,
            c.User?.UserName ?? "Unknown",
            c.CarForumArticleId
        )).ToList();

        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        return new PaginatedCarForumCommentsResponse(items, request.Page, totalPages, totalCount);
    }

    public async Task<CreateCarForumCommentResponse> AddCommentAsync(CreateCarForumCommentRequest request, Guid userId)
    {
        ValidateCreateRequest(request);

        var entity = new CarForumComment
        {
            Id = Guid.NewGuid(),
            Content = request.Content,
            CreatedAt = DateTime.UtcNow,
            UserId = userId,
            CarForumArticleId = request.ArticleId
        };

        await _carForumCommentRepository.AddCommentAsync(entity);

        return new CreateCarForumCommentResponse(
            entity.Id,
            entity.Content,
            entity.CreatedAt,
            entity.UserId,
            "Unknown",
            entity.CarForumArticleId
        );
    }

    public async Task<bool> DeleteCommentAsync(Guid commentId, Guid userId)
    {
        var comment = await _carForumCommentRepository.GetCommentByIdAsync(commentId);

        if (comment is null)
        {
            throw new NotFoundException(ExceptionMessages.CommentNotFound);
        }

        if (comment.UserId != userId)
        {
            throw new UnauthorizedAccessAppException(ExceptionMessages.UnauthorizedCommentAccess);
        }

        await _carForumCommentRepository.DeleteCommentAsync(commentId);

        return true;
    }

    private static void ValidateCreateRequest(CreateCarForumCommentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Content))
        {
            throw new ValidationAppException(ExceptionMessages.InvalidCommentData);
        }
    }
}
