using CarSpace.Data.Models.Entities.CarService;
using CarSpace.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarSpace.Data.Repositories;

public class CarServiceListingRepository : ICarServiceListingRepository
{
    private readonly CarSpaceDbContext _context;

    public CarServiceListingRepository(CarSpaceDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<CarServiceListing> Listings, int TotalCount)> GetFilteredListingsAsync(
        string? searchTerm,
        int? categoryId,
        decimal? minPrice,
        decimal? maxPrice,
        bool savedOnly,
        bool myServicesOnly,
        Guid? userId,
        int page,
        int pageSize)
    {
        var query = _context.CarServiceListings
            .Include(l => l.Category)
            .Include(l => l.SavedByUsers)
            .Include(l => l.User)
            .AsQueryable();

        query = ApplyFilters(query, searchTerm, categoryId, minPrice, maxPrice, savedOnly, myServicesOnly, userId);

        var totalCount = await query.CountAsync();

        var listings = await query
            .OrderByDescending(l => l.UpdatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (listings, totalCount);
    }

    public async Task<CarServiceListing?> GetCarServiceListingByIdAsync(Guid id)
    {
        return await _context.CarServiceListings
            .AsNoTracking()
            .Include(l => l.Category)
            .Include(l => l.User)
            .Include(l => l.SavedByUsers)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task AddCarServiceListingAsync(CarServiceListing listing)
    {
        _context.CarServiceListings.Add(listing);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateCarServiceListingAsync(CarServiceListing listing)
    {
        _context.CarServiceListings.Update(listing);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> SaveListingAsync(Guid listingId, Guid userId)
    {
        var alreadyExists = await _context.UserSavedCarServicesListings
            .AnyAsync(s => s.UserId == userId && s.CarServiceListingId == listingId);

        if (alreadyExists)
        {
            return false;
        }

        var save = new UserSavedCarServiceListing
        {
            UserId = userId,
            CarServiceListingId = listingId
        };

        _context.UserSavedCarServicesListings.Add(save);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UnsaveListingAsync(Guid listingId, Guid userId)
    {
        var existing = await _context.UserSavedCarServicesListings
            .FirstOrDefaultAsync(s => s.UserId == userId && s.CarServiceListingId == listingId);

        if (existing is null)
        {
            return false;
        }

        _context.UserSavedCarServicesListings.Remove(existing);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task DeleteCarServiceListingAsync(Guid id)
    {
        var entity = await GetCarServiceListingByIdAsync(id);

        if (entity is not null)
        {
            _context.CarServiceListings.Remove(entity);

            await _context.SaveChangesAsync();
        }
    }

    private IQueryable<CarServiceListing> ApplyFilters(
        IQueryable<CarServiceListing> query,
        string? searchTerm,
        int? categoryId,
        decimal? minPrice,
        decimal? maxPrice,
        bool savedOnly,
        bool myServicesOnly,
        Guid? userId)
    {
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(l => l.Title.Contains(searchTerm));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(l => l.CategoryId == categoryId);
        }

        if (minPrice.HasValue)
        {
            query = query.Where(l => l.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(l => l.Price <= maxPrice.Value);
        }

        if (savedOnly && userId.HasValue)
        {
            query = query.Where(l => l.SavedByUsers.Any(s => s.UserId == userId));
        }

        if (myServicesOnly && userId.HasValue)
        {
            query = query.Where(l => l.UserId == userId);
        }

        return query;
    }
}
