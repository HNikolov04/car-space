using CarSpace.Data.Models.Entities.CarShop;
using CarSpace.Data.Repositories.Interfaces;
using CarSpace.Services.Common.Constants;
using CarSpace.Services.Common.Messages;
using CarSpace.Services.Core.Constants;
using CarSpace.Services.Core.Contracts.CarShop.CarShopListing.Requests;
using CarSpace.Services.Core.Contracts.CarShop.CarShopListing.Responses;
using CarSpace.Services.Core.Exceptions;
using CarSpace.Services.Core.Interfaces;
using CarSpace.Services.Core.Services.Interfaces;

namespace CarSpace.Services.Core.Services;

public class CarShopListingService : ICarShopListingService
{
    private readonly ICarShopListingRepository _carShopListingRepository;
    private readonly IImageService _imageService;

    public CarShopListingService(ICarShopListingRepository carShopListingRepository, IImageService imageService)
    {
        _carShopListingRepository = carShopListingRepository;
        _imageService = imageService;
    }

    public async Task<PaginatedCarShopListingsResponse> GetCarShopListingsAsync(GetCarShopListingsRequest request, Guid? userId)
    {
        var (listings, totalCount) = await _carShopListingRepository.GetFilteredListingsAsync(
            request.SearchTerm,
            request.BrandId,
            request.MinYear,
            request.MaxYear,
            request.MinMileage,
            request.MaxMileage,
            request.Transmission,
            request.MinHorsepower,
            request.MaxHorsepower,
            request.FuelType,
            request.Color,
            request.Doors,
            request.EuroStandard,
            request.MinPrice,
            request.MaxPrice,
            request.SavedOnly,
            request.MyListingsOnly,
            userId,
            request.Page,
            request.PageSize);

        var items = listings.Select(l => new GetAllCarShopListingsResponse(
            l.Id,
            l.Title,
            l.Description,
            l.Brand?.Name ?? ServiceDefaults.UnknownBrand,
            l.Model,
            l.Year,
            l.FuelType,
            l.Mileage,
            l.Price,
            l.City,
            l.ImageUrl,
            l.UpdatedAt,
            userId.HasValue && l.SavedByUsers.Any(s => s.UserId == userId),
            l.User?.UserName ?? ServiceDefaults.UnknownUser
        )).ToList();

        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        return new PaginatedCarShopListingsResponse(items, request.Page, totalPages, totalCount);
    }

    public async Task<GetCarShopListingByIdResponse?> GetCarShopListingByIdAsync(Guid id, Guid? userId)
    {
        var listing = await _carShopListingRepository.GetByIdAsync(id);

        if (listing is null)
        {
            return null;
        }

        var isSaved = userId.HasValue && listing.SavedByUsers.Any(s => s.UserId == userId);

        return new GetCarShopListingByIdResponse(
            listing.Id,
            listing.Title,
            listing.Description,
            listing.CarBrandId,
            listing.Brand?.Name ?? ServiceDefaults.UnknownBrand,
            listing.Model,
            listing.Year,
            listing.Mileage,
            listing.Horsepower,
            listing.Transmission,
            listing.FuelType,
            listing.Color,
            listing.EuroStandard,
            listing.Doors,
            listing.Price,
            listing.City,
            listing.Address,
            listing.ImageUrl,
            isSaved,
            listing.UpdatedAt,
            listing.UserId,
            listing.User?.UserName ?? ServiceDefaults.UnknownUser
        );
    }

    public async Task<GetCarShopListingByIdResponse> CreateCarShopListingAsync(CreateCarShopListingRequest request, Guid userId)
    {
        ValidateCreateRequest(request);

        var imageUrl = await _imageService.SaveImageOrDefaultAsync(
            request.ImageFile,
            userId,
            ImageServiceConstants.CARS_AND_SUVS_LISTINGS_SUB_FOLDER,
            ImageServiceConstants.CARS_AND_SUVS_LISTING_IMAGE);

        var entity = new CarShopListing
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            CarBrandId = request.BrandId,
            Model = request.Model,
            Year = request.Year,
            Mileage = request.Mileage,
            Horsepower = request.Horsepower,
            Transmission = request.Transmission,
            FuelType = request.FuelType,
            Color = request.Color,
            EuroStandard = request.EuroStandard,
            Address = request.Address,
            Doors = request.Doors,
            Price = request.Price,
            City = request.City,
            ImageUrl = imageUrl,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            UserId = userId
        };

        await _carShopListingRepository.AddAsync(entity);

        var saved = await _carShopListingRepository.GetByIdAsync(entity.Id);

        if (saved is null)
        {
            throw new NotFoundException(ExceptionMessages.ListingNotFound);
        }

        return new GetCarShopListingByIdResponse(
            saved.Id,
            saved.Title,
            saved.Description,
            saved.CarBrandId,
            saved.Brand?.Name ?? ServiceDefaults.UnknownBrand,
            saved.Model,
            saved.Year,
            saved.Mileage,
            saved.Horsepower,
            saved.Transmission,
            saved.FuelType,
            saved.Color,
            saved.EuroStandard,
            saved.Doors,
            saved.Price,
            saved.City,
            saved.Address,
            saved.ImageUrl,
            false,
            saved.UpdatedAt,
            saved.UserId,
            saved.User?.UserName ?? ServiceDefaults.UnknownUser
        );
    }

    public async Task<bool> UpdateCarShopListingAsync(UpdateCarShopListingRequest request, Guid userId)
    {
        var existing = await _carShopListingRepository.GetByIdAsync(request.Id);

        if (existing is null)
        {
            throw new NotFoundException(ExceptionMessages.ListingNotFound);
        }

        if (existing.UserId != userId)
        {
            throw new UnauthorizedAccessAppException(ExceptionMessages.UnauthorizedListingAccess);
        }

        existing.Title = request.Title;
        existing.Description = request.Description;
        existing.CarBrandId = request.BrandId;
        existing.Model = request.Model;
        existing.Year = request.Year;
        existing.Mileage = request.Mileage;
        existing.Horsepower = request.Horsepower;
        existing.Transmission = request.Transmission;
        existing.FuelType = request.FuelType;
        existing.Color = request.Color;
        existing.EuroStandard = request.EuroStandard;
        existing.Doors = request.Doors;
        existing.Price = request.Price;
        existing.City = request.City;
        existing.UpdatedAt = DateTime.UtcNow;

        existing.ImageUrl = await _imageService.UpdateImageIfProvidedAsync(
            request.ImageFile,
            existing.ImageUrl,
            userId,
            ImageServiceConstants.CARS_AND_SUVS_LISTINGS_SUB_FOLDER);

        await _carShopListingRepository.UpdateAsync(existing);

        return true;
    }

    public async Task DeleteCarShopListingAsync(Guid id, Guid userId)
    {
        var listing = await _carShopListingRepository.GetByIdAsync(id);

        if (listing is null)
        {
            throw new NotFoundException(ExceptionMessages.ListingNotFound);
        }

        if (listing.UserId != userId)
        {
            throw new UnauthorizedAccessAppException(ExceptionMessages.UnauthorizedListingAccess);
        }

        await _imageService.DeleteImageAsync(listing.ImageUrl);
        await _carShopListingRepository.DeleteAsync(id);
    }

    public async Task<bool> SaveCarShopListingAsync(Guid listingId, Guid userId)
    {
        return await _carShopListingRepository.SaveAsync(listingId, userId);
    }

    public async Task<bool> UnsaveCarShopListingAsync(Guid listingId, Guid userId)
    {
        return await _carShopListingRepository.UnsaveAsync(listingId, userId);
    }

    private static void ValidateCreateRequest(CreateCarShopListingRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title) ||
            string.IsNullOrWhiteSpace(request.Description) ||
            string.IsNullOrWhiteSpace(request.Model) ||
            string.IsNullOrWhiteSpace(request.Transmission) ||
            string.IsNullOrWhiteSpace(request.FuelType) ||
            string.IsNullOrWhiteSpace(request.Color) ||
            string.IsNullOrWhiteSpace(request.EuroStandard) ||
            string.IsNullOrWhiteSpace(request.City) ||
            string.IsNullOrWhiteSpace(request.Address) ||
            request.BrandId <= 0 ||
            request.Year <= 1900 ||
            request.Mileage < 0 ||
            request.Horsepower < 0 ||
            request.Doors < 2 ||
            request.Price < 0)
        {
            throw new ValidationAppException(ExceptionMessages.InvalidListingData);
        }
    }
}
