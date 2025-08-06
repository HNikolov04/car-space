using CarSpace.Data.Models.Entities.CarShop;
using CarSpace.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarSpace.Data.Repositories;

public class CarShopListingRepository : ICarShopListingRepository
{
    private readonly CarSpaceDbContext _context;

    public CarShopListingRepository(CarSpaceDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<CarShopListing> Listings, int TotalCount)> GetFilteredListingsAsync(
        string? searchTerm,
        int? brandId,
        int? minYear,
        int? maxYear,
        int? minMileage,
        int? maxMileage,
        string? transmission,
        int? minHorsepower,
        int? maxHorsepower,
        string? fuelType,
        string? color,
        int? doors,
        string? euroStandard,
        decimal? minPrice,
        decimal? maxPrice,
        bool savedOnly,
        bool myListingsOnly,
        Guid? userId,
        int page,
        int pageSize)
    {
        var query = _context.CarShopListings
            .Include(l => l.Brand)
            .Include(l => l.SavedByUsers)
            .Include(l => l.User)
            .AsQueryable();

        query = ApplyFilters(
            query,
            searchTerm,
            brandId,
            minYear,
            maxYear,
            minMileage,
            maxMileage,
            transmission,
            minHorsepower,
            maxHorsepower,
            fuelType,
            color,
            doors,
            euroStandard,
            minPrice,
            maxPrice,
            savedOnly,
            myListingsOnly,
            userId
        );

        var totalCount = await query.CountAsync();

        var listings = await query
            .OrderByDescending(l => l.UpdatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (listings, totalCount);
    }

    public async Task<CarShopListing?> GetByIdAsync(Guid id)
    {
        return await _context.CarShopListings
            .Include(l => l.Brand)
            .Include(l => l.SavedByUsers)
            .Include(l => l.User)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task AddAsync(CarShopListing listing)
    {
        _context.CarShopListings.Add(listing);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CarShopListing listing)
    {
        _context.CarShopListings.Update(listing);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);

        if (entity is not null)
        {
            _context.CarShopListings.Remove(entity);

            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> SaveAsync(Guid listingId, Guid userId)
    {
        var alreadyExists = await _context.UserSavedCarShopListings
            .AnyAsync(x => x.UserId == userId && x.CarShopListingId == listingId);

        if (alreadyExists)
        {
            return false;
        }

        _context.UserSavedCarShopListings.Add(new UserSavedCarShopListing
        {
            UserId = userId,
            CarShopListingId = listingId
        });

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UnsaveAsync(Guid listingId, Guid userId)
    {
        var entity = await _context.UserSavedCarShopListings
            .FirstOrDefaultAsync(x => x.UserId == userId && x.CarShopListingId == listingId);

        if (entity is null)
        {
            return false;
        }

        _context.UserSavedCarShopListings.Remove(entity);

        await _context.SaveChangesAsync();

        return true;
    }

    private IQueryable<CarShopListing> ApplyFilters(
        IQueryable<CarShopListing> query,
        string? searchTerm,
        int? brandId,
        int? minYear,
        int? maxYear,
        int? minMileage,
        int? maxMileage,
        string? transmission,
        int? minHorsepower,
        int? maxHorsepower,
        string? fuelType,
        string? color,
        int? doors,
        string? euroStandard,
        decimal? minPrice,
        decimal? maxPrice,
        bool savedOnly,
        bool myListingsOnly,
        Guid? userId)
    {
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(l => l.Title.Contains(searchTerm));
        }

        if (brandId.HasValue)
        {
            query = query.Where(l => l.CarBrandId == brandId);
        }

        if (minYear.HasValue)
        {
            query = query.Where(l => l.Year >= minYear.Value);
        }

        if (maxYear.HasValue)
        {
            query = query.Where(l => l.Year <= maxYear.Value);
        }

        if (minMileage.HasValue)
        {
            query = query.Where(l => l.Mileage >= minMileage.Value);
        }

        if (maxMileage.HasValue)
        {
            query = query.Where(l => l.Mileage <= maxMileage.Value);
        }

        if (!string.IsNullOrWhiteSpace(transmission))
        {
            query = query.Where(l => l.Transmission == transmission);
        }

        if (minHorsepower.HasValue)
        {
            query = query.Where(l => l.Horsepower >= minHorsepower.Value);
        }

        if (maxHorsepower.HasValue)
        {
            query = query.Where(l => l.Horsepower <= maxHorsepower.Value);
        }

        if (!string.IsNullOrWhiteSpace(fuelType))
        {
            query = query.Where(l => l.FuelType == fuelType);
        }

        if (!string.IsNullOrWhiteSpace(color))
        {
            query = query.Where(l => l.Color == color);
        }

        if (doors.HasValue)
        {
            query = query.Where(l => l.Doors == doors.Value);
        }

        if (!string.IsNullOrWhiteSpace(euroStandard))
        {
            query = query.Where(l => l.EuroStandard == euroStandard);
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

        if (myListingsOnly && userId.HasValue)
        {
            query = query.Where(l => l.UserId == userId);
        }

        return query;
    }
}
