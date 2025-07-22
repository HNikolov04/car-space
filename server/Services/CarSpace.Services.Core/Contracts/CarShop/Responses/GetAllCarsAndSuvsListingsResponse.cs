namespace CarSpace.Services.Core.Contracts.CarShop.Responses;

public sealed record GetAllCarsAndSuvsListingsResponse(
    Guid Id,
    string Title,
    string Brand,
    string Model,
    int Year,
    decimal Price,
    string City,
    string ImageUrl
);
