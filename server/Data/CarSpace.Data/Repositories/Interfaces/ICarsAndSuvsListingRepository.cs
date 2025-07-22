using CarSpace.Data.Models.Entities.CarShop;

namespace CarSpace.Data.Repositories.Interfaces;

public interface ICarsAndSuvsListingRepository
{
    Task<IEnumerable<CarsAndSuvsListing>> GetAllCarsAndSuvsListingsAsync();
    Task<CarsAndSuvsListing?> GetCarsAndSuvsListingByIdAsync(Guid id);
    Task AddCarsAndSuvsListingAsync(CarsAndSuvsListing listing);
    Task UpdateCarsAndSuvsListingAsync(CarsAndSuvsListing listing);
    Task DeleteCarsAndSuvsListingAsync(Guid id);
}
