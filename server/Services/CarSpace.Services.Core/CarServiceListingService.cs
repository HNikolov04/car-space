using CarSpace.Data.Models.Entities.CarServices;
using CarSpace.Data.Repositories.Interfaces;
using CarSpace.Services.Core.Contracts.CarServiceListing.Requests;
using CarSpace.Services.Core.Contracts.CarServiceListing.Responses;
using CarSpace.Services.Core.Interfaces;

namespace CarSpace.Services.Core;

public class CarServiceListingService : ICarServiceListingService
{
    private readonly ICarServiceListingRepository _carServiceListingRepository;

    public CarServiceListingService(ICarServiceListingRepository carServiceListingRepository)
    {
        _carServiceListingRepository = carServiceListingRepository;
    }

    public async Task<IEnumerable<GetAllCarServiceListingsResponse>> GetAllCarServiceListingsAsync()
    {
        var listings = await _carServiceListingRepository.GetAllCarServiceListingsAsync();

        return listings.Select(l => new GetAllCarServiceListingsResponse(
            l.Id,
            l.Title,
            l.City,
            l.Category,
            l.Price,
            l.ImageUrl
        ));
    }

    public async Task<GetCarServiceListingByIdResponse?> GetCarServiceListingByIdAsync(Guid id)
    {
        var l = await _carServiceListingRepository.GetCarServiceListingByIdAsync(id);

        return l is null ? null : new GetCarServiceListingByIdResponse(
            l.Id,
            l.Title,
            l.Description,
            l.Category,
            l.PhoneNumber,
            l.City,
            l.Address,
            l.Price,
            l.ImageUrl,
            l.CreatedAt,
            l.UserId
        );
    }

    public async Task<GetCarServiceListingByIdResponse> CreateCarServiceListingAsync(CreateCarServiceListingRequest request)
    {
        var entity = new CarServiceListing
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Category = request.Category,
            PhoneNumber = request.PhoneNumber,
            City = request.City,
            Address = request.Address,
            Price = request.Price,
            ImageUrl = request.ImageUrl,
            CreatedAt = DateTime.UtcNow,
            UserId = request.UserId
        };

        await _carServiceListingRepository.AddCarServiceListingAsync(entity);

        return new GetCarServiceListingByIdResponse(
            entity.Id,
            entity.Title,
            entity.Description,
            entity.Category,
            entity.PhoneNumber,
            entity.City,
            entity.Address,
            entity.Price,
            entity.ImageUrl,
            entity.CreatedAt,
            entity.UserId
        );
    }

    public async Task<bool> UpdateCarServiceListingAsync(UpdateCarServiceListingRequest request)
    {
        var listing = await _carServiceListingRepository.GetCarServiceListingByIdAsync(request.Id);

        if (listing is null)
            return false;

        listing.Title = request.Title;
        listing.Description = request.Description;
        listing.Category = request.Category;
        listing.PhoneNumber = request.PhoneNumber;
        listing.City = request.City;
        listing.Address = request.Address;
        listing.Price = request.Price;
        listing.ImageUrl = request.ImageUrl;

        await _carServiceListingRepository.UpdateCarServiceListingAsync(listing);

        return true;
    }

    public async Task DeleteCarServiceListingAsync(Guid id)
    {
        await _carServiceListingRepository.DeleteCarServiceListingAsync(id);
    }
}