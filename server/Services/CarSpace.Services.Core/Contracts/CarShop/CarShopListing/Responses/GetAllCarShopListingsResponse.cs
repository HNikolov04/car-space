namespace CarSpace.Services.Core.Contracts.CarShop.CarShopListing.Responses;

public record GetAllCarShopListingsResponse(
    Guid Id,
    string Title,
    string Description,
    string BrandName,
    string Model,
    int Year,
    string FuelType,
    int Mileage,
    decimal Price,
    string City,
    string ImageUrl,
    DateTime UpdatedAt,
    bool IsSavedByCurrentUser,
    string UserNickname);
