using CarSpace.Data.Models.CarForum;
using CarSpace.Data.Models.Entities.CarForum;
using CarSpace.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarSpace.Data.Repositories;

public class CarForumRepository : ICarForumRepository
{
    private readonly CarSpaceDbContext _context;

    public CarForumRepository(CarSpaceDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CarForumArticle>> GetAllArticlesAsync()
    {
        return await _context.CarForumArticles
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<CarForumArticle?> GetArticleByIdAsync(Guid id)
    {
        return await _context.CarForumArticles
            .Include(a => a.Comments)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddArticleAsync(CarForumArticle article)
    {
        _context.CarForumArticles.Add(article);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteArticleAsync(Guid id)
    {
        var article = await _context.CarForumArticles.FindAsync(id);
        if (article != null)
        {
            _context.CarForumArticles.Remove(article);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<CarForumComment>> GetCommentsForArticleAsync(Guid articleId)
    {
        return await _context.CarForumArticleComments
            .Where(c => c.CarForumArticleId == articleId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<CarForumComment?> GetCommentByIdAsync(Guid commentId)
    {
        return await _context.CarForumArticleComments
            .AsNoTracking()
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
        if (comment != null)
        {
            _context.CarForumArticleComments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
}
