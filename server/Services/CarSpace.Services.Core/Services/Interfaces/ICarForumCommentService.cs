using CarSpace.Services.Core.Contracts.CarForum.CarForumComment.Requests;
using CarSpace.Services.Core.Contracts.CarForum.CarForumComment.Responses;

namespace CarSpace.Services.Core.Services.Interfaces;

public interface ICarForumCommentService
{
    Task<PaginatedCarForumCommentsResponse> GetCommentsForArticleAsync(GetCarForumCommentsRequest request);
    Task<CreateCarForumCommentResponse> AddCommentAsync(CreateCarForumCommentRequest request, Guid userId);
    Task<bool> DeleteCommentAsync(Guid commentId, Guid userId);
}
