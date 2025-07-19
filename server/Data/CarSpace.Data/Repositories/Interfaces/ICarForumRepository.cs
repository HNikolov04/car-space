using CarSpace.Data.Models.CarForum;
using CarSpace.Data.Models.Entities.CarForum;

namespace CarSpace.Data.Repositories.Interfaces;

public interface ICarForumRepository
{
    Task<IEnumerable<CarForumArticle>> GetAllArticlesAsync();
    Task<CarForumArticle?> GetArticleByIdAsync(Guid id);
    Task AddArticleAsync(CarForumArticle article);
    Task DeleteArticleAsync(Guid id);

    Task<IEnumerable<CarForumComment>> GetCommentsForArticleAsync(Guid articleId);
    Task<CarForumComment?> GetCommentByIdAsync(Guid commentId);
    Task AddCommentAsync(CarForumComment comment);
    Task DeleteCommentAsync(Guid id);
}