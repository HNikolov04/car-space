using CarSpace.Data.Models.Entities.CarService;

namespace CarSpace.Data.Repositories.Interfaces;

public interface ICarServiceListingRepository
{
    Task<(IEnumerable<CarServiceListing> Listings, int TotalCount)> GetFilteredListingsAsync(
    string? searchTerm,
    int? categoryId,
    decimal? minPrice,
    decimal? maxPrice,
    bool savedOnly,
    bool myServicesOnly,
    Guid? userId,
    int page,
    int pageSize);
    Task<CarServiceListing?> GetCarServiceListingByIdAsync(Guid id);
    Task AddCarServiceListingAsync(CarServiceListing listing);
    Task UpdateCarServiceListingAsync(CarServiceListing listing);
    Task<bool> SaveListingAsync(Guid listingId, Guid userId);
    Task<bool> UnsaveListingAsync(Guid listingId, Guid userId);
    Task DeleteCarServiceListingAsync(Guid id);
}
