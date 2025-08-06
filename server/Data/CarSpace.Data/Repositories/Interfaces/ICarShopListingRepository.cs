using CarSpace.Data.Models.Entities.CarShop;

namespace CarSpace.Data.Repositories.Interfaces;

public interface ICarShopListingRepository
{
    Task<(IEnumerable<CarShopListing> Listings, int TotalCount)> GetFilteredListingsAsync(
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
        int pageSize);

    Task<CarShopListing?> GetByIdAsync(Guid id);
    Task AddAsync(CarShopListing listing);
    Task UpdateAsync(CarShopListing listing);
    Task DeleteAsync(Guid id);

    Task<bool> SaveAsync(Guid listingId, Guid userId);
    Task<bool> UnsaveAsync(Guid listingId, Guid userId);
}
