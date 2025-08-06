using CarSpace.Data.Models.CarForum;
using CarSpace.Data.Models.Entities.CarForum;

namespace CarSpace.Data.Repositories.Interfaces;

public interface ICarForumArticleRepository
{
    Task<(IEnumerable<CarForumArticle> Articles, int TotalCount)> GetFilteredArticlesAsync(
        string? searchTerm,
        int? brandId,
        bool savedOnly,
        bool myArticlesOnly,
        Guid? userId,
        int page,
        int pageSize);

    Task<CarForumArticle?> GetArticleByIdAsync(Guid id);
    Task AddArticleAsync(CarForumArticle article);
    Task UpdateArticleAsync(CarForumArticle article);
    Task DeleteArticleAsync(Guid id);
    Task<bool> SaveArticleAsync(Guid articleId, Guid userId);
    Task<bool> UnsaveArticleAsync(Guid articleId, Guid userId);
}
