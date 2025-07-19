using CarSpace.Services.Core.Contracts.CarServiceListing.Requests;
using CarSpace.Services.Core.Contracts.CarServiceListing.Responses;

namespace CarSpace.Services.Core.Interfaces;

public interface ICarServiceListingService
{
    Task<IEnumerable<GetAllCarServiceListingsResponse>> GetAllCarServiceListingsAsync();
    Task<GetCarServiceListingByIdResponse?> GetCarServiceListingByIdAsync(Guid id);
    Task<GetCarServiceListingByIdResponse> CreateCarServiceListingAsync(CreateCarServiceListingRequest request);
    Task<bool> UpdateCarServiceListingAsync(UpdateCarServiceListingRequest request);
    Task DeleteCarServiceListingAsync(Guid id);
}