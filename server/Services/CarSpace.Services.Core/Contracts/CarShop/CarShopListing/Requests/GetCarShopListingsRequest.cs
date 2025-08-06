namespace CarSpace.Services.Core.Contracts.CarShop.CarShopListing.Requests;

public record GetCarShopListingsRequest(
    string? SearchTerm,
    int? BrandId,
    int? MinYear,
    int? MaxYear,
    int? MinMileage,
    int? MaxMileage,
    int? MinHorsepower,
    int? MaxHorsepower,
    string? Transmission,
    string? FuelType,
    string? Color,
    string? EuroStandard,
    int? Doors,
    decimal? MinPrice,
    decimal? MaxPrice,
    bool SavedOnly,
    bool MyListingsOnly,
    int Page = 1,
    int PageSize = 10);
