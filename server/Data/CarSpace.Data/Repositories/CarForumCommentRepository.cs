using CarSpace.Data.Models.Entities.CarForum;
using CarSpace.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarSpace.Data.Repositories;

public class CarForumCommentRepository : ICarForumCommentRepository
{
    private readonly CarSpaceDbContext _context;

    public CarForumCommentRepository(CarSpaceDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<CarForumComment> Comments, int TotalCount)> GetCommentsForArticleAsync(Guid articleId, int page, int pageSize)
    {
        var query = _context.CarForumArticleComments
            .Include(c => c.User)
            .Where(c => c.CarForumArticleId == articleId)
            .OrderByDescending(c => c.CreatedAt);

        var totalCount = await query.CountAsync();

        var comments = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (comments, totalCount);
    }

    public async Task<CarForumComment?> GetCommentByIdAsync(Guid commentId)
    {
        return await _context.CarForumArticleComments
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == commentId);
    }

    public async Task AddCommentAsync(CarForumComment comment)
    {
        _context.CarForumArticleComments.Add(comment);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(Guid id)
    {
        var comment = await _context.CarForumArticleComments.FindAsync(id);

        if (comment is not null)
        {
            _context.CarForumArticleComments.Remove(comment);

            await _context.SaveChangesAsync();
        }
    }
}
