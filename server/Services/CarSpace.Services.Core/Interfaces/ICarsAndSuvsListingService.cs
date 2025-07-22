using CarSpace.Services.Core.Contracts.CarShop.Requests;
using CarSpace.Services.Core.Contracts.CarShop.Responses;

namespace CarSpace.Services.Core.Interfaces;

public interface ICarsAndSuvsListingService
{
    Task<IEnumerable<GetAllCarsAndSuvsListingsResponse>> GetAllCarsAndSuvsListingsAsync();
    Task<GetCarsAndSuvsListingByIdResponse?> GetCarsAndSuvsListingByIdAsync(Guid id);
    Task<GetCarsAndSuvsListingByIdResponse> CreateCarsAndSuvsListingAsync(CreateCarsAndSuvsListingRequest request);
    Task<bool> UpdateCarsAndSuvsListingAsync(UpdateCarsAndSuvsListingRequest request);
    Task DeleteCarsAndSuvsListingAsync(Guid id);
}
