using CarSpace.Data.Models.Entities.CarServiceListing;

namespace CarSpace.Data.Repositories.Interfaces;

public interface ICarServiceListingRepository
{
    Task<IEnumerable<CarServiceListing>> GetAllCarServiceListingsAsync();
    Task<CarServiceListing?> GetCarServiceListingByIdAsync(Guid id);
    Task AddCarServiceListingAsync(CarServiceListing listing);
    Task UpdateCarServiceListingAsync(CarServiceListing listing);
    Task DeleteCarServiceListingAsync(Guid id);
}
