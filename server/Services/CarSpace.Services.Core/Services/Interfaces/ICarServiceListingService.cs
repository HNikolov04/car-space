using CarSpace.Services.Core.Contracts.CarService.CarServiceListing.Requests;
using CarSpace.Services.Core.Contracts.CarService.CarServiceListing.Responses;

namespace CarSpace.Services.Core.Services.Interfaces;

public interface ICarServiceListingService
{
    Task<PaginatedCarServiceListingsResponse> GetCarServiceListingsAsync(GetCarServiceListingsRequest request, Guid? userId);
    Task<GetCarServiceListingByIdResponse?> GetCarServiceListingByIdAsync(Guid id, Guid? userId);
    Task<GetCarServiceListingByIdResponse> CreateCarServiceListingAsync(CreateCarServiceListingRequest request, Guid userId);
    Task<bool> UpdateCarServiceListingAsync(UpdateCarServiceListingRequest request, Guid userId);
    Task<bool> SaveCarServiceListingAsync(Guid listingId, Guid userId);
    Task<bool> UnsaveCarServiceListingAsync(Guid listingId, Guid userId);
    Task DeleteCarServiceListingAsync(Guid id, Guid userId);
}