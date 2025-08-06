using CarSpace.Data.Models.CarForum;
using CarSpace.Data.Models.Entities.CarForum;
using CarSpace.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarSpace.Data.Repositories;

public class CarForumArticleRepository : ICarForumArticleRepository
{
    private readonly CarSpaceDbContext _context;

    public CarForumArticleRepository(CarSpaceDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<CarForumArticle> Articles, int TotalCount)> GetFilteredArticlesAsync(
        string? searchTerm,
        int? brandId,
        bool savedOnly,
        bool myArticlesOnly,
        Guid? userId,
        int page,
        int pageSize)
    {
        var query = _context.CarForumArticles
            .Include(a => a.SavedByUsers)
            .Include(a => a.CarBrand)
            .Include(a => a.User)
            .AsQueryable();

        query = ApplyFilters(query, searchTerm, brandId, savedOnly, myArticlesOnly, userId);

        var totalCount = await query.CountAsync();

        var results = await query
            .OrderByDescending(a => a.UpdatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (results, totalCount);
    }

    public async Task<CarForumArticle?> GetArticleByIdAsync(Guid id)
    {
        return await _context.CarForumArticles
            .AsNoTracking()
            .Include(a => a.CarForumComments)
            .Include(a => a.SavedByUsers)
            .Include(a => a.CarBrand)
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddArticleAsync(CarForumArticle article)
    {
        _context.CarForumArticles.Add(article);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateArticleAsync(CarForumArticle article)
    {
        _context.CarForumArticles.Update(article);
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

    public async Task<bool> SaveArticleAsync(Guid articleId, Guid userId)
    {
        var alreadySaved = await _context.UserSavedCarForumArticles
            .AnyAsync(x => x.UserId == userId && x.CarForumArticleId == articleId);

        if (alreadySaved)
        {
            return false;
        }

        var entity = new UserSavedCarForumArticle
        {
            CarForumArticleId = articleId,
            UserId = userId
        };

        _context.UserSavedCarForumArticles.Add(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UnsaveArticleAsync(Guid articleId, Guid userId)
    {
        var existing = await _context.UserSavedCarForumArticles
            .FirstOrDefaultAsync(x => x.UserId == userId && x.CarForumArticleId == articleId);

        if (existing == null)
        {
            return false;
        }

        _context.UserSavedCarForumArticles.Remove(existing);
        await _context.SaveChangesAsync();

        return true;
    }

    private static IQueryable<CarForumArticle> ApplyFilters(
        IQueryable<CarForumArticle> query,
        string? searchTerm,
        int? brandId,
        bool savedOnly,
        bool myArticlesOnly,
        Guid? userId)
    {
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(a => a.Title.Contains(searchTerm));
        }

        if (brandId.HasValue)
        {
            query = query.Where(a => a.CarBrandId == brandId.Value);
        }

        if (savedOnly && userId.HasValue)
        {
            query = query.Where(a => a.SavedByUsers.Any(s => s.UserId == userId.Value));
        }

        if (myArticlesOnly && userId.HasValue)
        {
            query = query.Where(a => a.UserId == userId.Value);
        }

        return query;
    }
}
