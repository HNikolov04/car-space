using CarSpace.Services.Core.Contracts.CarForum.CarForumArticle.Requests;
using CarSpace.Services.Core.Contracts.CarForum.CarForumArticle.Responses;

namespace CarSpace.Services.Core.Services.Interfaces;

public interface ICarForumArticleService
{
    Task<PaginatedCarForumArticlesResponse> GetCarForumArticlesAsync(GetCarForumArticlesRequest request, Guid? userId);
    Task<GetCarForumArticleByIdResponse?> GetCarForumArticleByIdAsync(Guid id, Guid? userId);
    Task<GetCarForumArticleByIdResponse> CreateCarForumArticleAsync(CreateCarForumArticleRequest request, Guid userId);
    Task<bool> UpdateCarForumArticleAsync(Guid id, UpdateCarForumArticleRequest request, Guid userId);
    Task<bool> DeleteCarForumArticleAsync(Guid id, Guid userId);
    Task<bool> SaveCarForumArticleAsync(Guid articleId, Guid userId);
    Task<bool> UnsaveCarForumArticleAsync(Guid articleId, Guid userId);
}
