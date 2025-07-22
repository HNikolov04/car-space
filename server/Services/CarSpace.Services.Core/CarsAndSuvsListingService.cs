using CarSpace.Data.Models.Entities.CarShop;
using CarSpace.Data.Repositories.Interfaces;
using CarSpace.Services.Core.Contracts.CarShop.Requests;
using CarSpace.Services.Core.Contracts.CarShop.Responses;
using CarSpace.Services.Core.Interfaces;

namespace CarSpace.Services.Core;

public class CarsAndSuvsListingService : ICarsAndSuvsListingService
{
    private readonly ICarsAndSuvsListingRepository _repository;

    public CarsAndSuvsListingService(ICarsAndSuvsListingRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GetAllCarsAndSuvsListingsResponse>> GetAllCarsAndSuvsListingsAsync()
    {
        var listings = await _repository.GetAllCarsAndSuvsListingsAsync();
        return listings.Select(l => new GetAllCarsAndSuvsListingsResponse(
            l.Id, l.Title, l.Brand, l.Model, l.Year, l.Price, l.City, l.ImageUrl
        ));
    }

    public async Task<GetCarsAndSuvsListingByIdResponse?> GetCarsAndSuvsListingByIdAsync(Guid id)
    {
        var l = await _repository.GetCarsAndSuvsListingByIdAsync(id);
        return l is null ? null : new GetCarsAndSuvsListingByIdResponse(
            l.Id, l.Title, l.Description, l.Brand, l.Model, l.Year, l.Mileage, l.Horsepower,
            l.Transmission, l.FuelType, l.Color, l.EuroStandard, l.Doors,
            l.Price, l.City, l.ImageUrl, l.CreatedAt, l.UserId
        );
    }

    public async Task<GetCarsAndSuvsListingByIdResponse> CreateCarsAndSuvsListingAsync(CreateCarsAndSuvsListingRequest request)
    {
        var entity = new CarsAndSuvsListing
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Brand = request.Brand,
            Model = request.Model,
            Year = request.Year,
            Mileage = request.Mileage,
            Horsepower = request.Horsepower,
            Transmission = request.Transmission,
            FuelType = request.FuelType,
            Color = request.Color,
            EuroStandard = request.EuroStandard,
            Doors = request.Doors,
            Price = request.Price,
            City = request.City,
            ImageUrl = request.ImageUrl,
            CreatedAt = DateTime.UtcNow,
            UserId = request.UserId
        };

        await _repository.AddCarsAndSuvsListingAsync(entity);

        return new GetCarsAndSuvsListingByIdResponse(
            entity.Id, entity.Title, entity.Description, entity.Brand, entity.Model,
            entity.Year, entity.Mileage, entity.Horsepower, entity.Transmission,
            entity.FuelType, entity.Color, entity.EuroStandard, entity.Doors,
            entity.Price, entity.City, entity.ImageUrl, entity.CreatedAt, entity.UserId
        );
    }

    public async Task<bool> UpdateCarsAndSuvsListingAsync(UpdateCarsAndSuvsListingRequest request)
    {
        var l = await _repository.GetCarsAndSuvsListingByIdAsync(request.Id);
        if (l is null) return false;

        l.Title = request.Title;
        l.Description = request.Description;
        l.Brand = request.Brand;
        l.Model = request.Model;
        l.Year = request.Year;
        l.Mileage = request.Mileage;
        l.Horsepower = request.Horsepower;
        l.Transmission = request.Transmission;
        l.FuelType = request.FuelType;
        l.Color = request.Color;
        l.EuroStandard = request.EuroStandard;
        l.Doors = request.Doors;
        l.Price = request.Price;
        l.City = request.City;
        l.ImageUrl = request.ImageUrl;

        await _repository.UpdateCarsAndSuvsListingAsync(l);
        return true;
    }

    public async Task DeleteCarsAndSuvsListingAsync(Guid id)
    {
        await _repository.DeleteCarsAndSuvsListingAsync(id);
    }
}
