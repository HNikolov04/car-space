using Microsoft.AspNetCore.Http;

namespace CarSpace.Services.Core.Contracts.CarShop.CarShopListing.Requests;

public record UpdateCarShopListingRequest(
    Guid Id,
    string Title,
    string Description,
    int BrandId,
    string Model,
    int Year,
    int Mileage,
    int Horsepower,
    string Transmission,
    string FuelType,
    string Color,
    string EuroStandard,
    int Doors,
    decimal Price,
    string City,
    string Address,
    IFormFile? ImageFile);
