using CarSpace.Services.Core.Contracts.CarForum.Requests;
using CarSpace.Services.Core.Contracts.CarForum.Responses;

namespace CarSpace.Services.Core.Interfaces;

public interface ICarForumService
{
    Task<IEnumerable<GetCarForumArticleResponse>> GetAllArticlesAsync();
    Task<GetCarForumArticleResponse?> GetArticleByIdAsync(Guid id);
    Task<CreateCarForumArticleResponse> AddArticleAsync(CreateCarForumArticleRequest dto);
    Task DeleteArticleAsync(Guid id);

    Task<IEnumerable<GetAllCarForumCommentsForArticleResponse>> GetCommentsForArticleAsync(Guid articleId);
    Task<GetCarForumCommentResponse?> GetCommentByIdAsync(Guid commentId);
    Task<CreateCarForumCommentResponse> AddCommentAsync(CreateCarForumCommentRequest dto);
    Task DeleteCommentAsync(Guid id);
}
