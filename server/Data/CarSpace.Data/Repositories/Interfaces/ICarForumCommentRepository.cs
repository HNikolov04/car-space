using CarSpace.Data.Models.Entities.CarForum;

namespace CarSpace.Data.Repositories.Interfaces;

public interface ICarForumCommentRepository
{
    Task<(IEnumerable<CarForumComment> Comments, int TotalCount)> GetCommentsForArticleAsync(Guid articleId, int page, int pageSize);
    Task<CarForumComment?> GetCommentByIdAsync(Guid commentId);
    Task AddCommentAsync(CarForumComment comment);
    Task DeleteCommentAsync(Guid id);
}
