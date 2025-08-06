using CarSpace.Services.Core.Contracts.CarShop.CarShopListing.Requests;
using CarSpace.Services.Core.Contracts.CarShop.CarShopListing.Responses;

namespace CarSpace.Services.Core.Interfaces;

public interface ICarShopListingService
{
    Task<PaginatedCarShopListingsResponse> GetCarShopListingsAsync(GetCarShopListingsRequest request, Guid? userId);
    Task<GetCarShopListingByIdResponse?> GetCarShopListingByIdAsync(Guid id, Guid? userId);
    Task<GetCarShopListingByIdResponse> CreateCarShopListingAsync(CreateCarShopListingRequest request, Guid userId);
    Task<bool> UpdateCarShopListingAsync(UpdateCarShopListingRequest request, Guid userId);
    Task DeleteCarShopListingAsync(Guid id, Guid userId);
    Task<bool> SaveCarShopListingAsync(Guid listingId, Guid userId);
    Task<bool> UnsaveCarShopListingAsync(Guid listingId, Guid userId);
}
