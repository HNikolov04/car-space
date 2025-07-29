namespace CarSpace.Services.Core.Contracts.CarShop.Requests;

public sealed record CreateCarsAndSuvsListingRequest(
    string Title,
    string Description,
    string Brand,
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
    string ImageUrl,
    Guid UserId
);
