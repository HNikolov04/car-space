using CarSpace.Data.Models.Entities.CarService;
using CarSpace.Data.Repositories.Interfaces;
using CarSpace.Services.Common.Constants;
using CarSpace.Services.Common.Messages;
using CarSpace.Services.Core.Constants;
using CarSpace.Services.Core.Contracts.CarService.CarServiceListing.Requests;
using CarSpace.Services.Core.Contracts.CarService.CarServiceListing.Responses;
using CarSpace.Services.Core.Exceptions;
using CarSpace.Services.Core.Services.Interfaces;

namespace CarSpace.Services.Core.Services;

public class CarServiceListingService : ICarServiceListingService
{
    private readonly ICarServiceListingRepository _carServiceListingRepository;
    private readonly IImageService _imageService;

    public CarServiceListingService(
        ICarServiceListingRepository carServiceListingRepository,
        IImageService imageService)
    {
        _carServiceListingRepository = carServiceListingRepository;
        _imageService = imageService;
    }

    public async Task<PaginatedCarServiceListingsResponse> GetCarServiceListingsAsync(GetCarServiceListingsRequest request, Guid? userId)
    {
        var (listings, totalCount) = await _carServiceListingRepository.GetFilteredListingsAsync(
            request.SearchTerm,
            request.CategoryId,
            request.MinPrice,
            request.MaxPrice,
            request.SavedOnly,
            request.MyServicesOnly,
            userId,
            request.Page,
            request.PageSize
        );

        var items = listings.Select(l => new GetAllCarServiceListingsResponse(
            l.Id,
            l.Title,
            l.Description,
            l.City,
            l.Category?.Name ?? ServiceDefaults.UnknownCategory,
            l.Price,
            l.ImageUrl,
            l.UpdatedAt,
            userId.HasValue && l.SavedByUsers.Any(s => s.UserId == userId),
            l.User?.UserName ?? ServiceDefaults.UnknownUser
        )).ToList();

        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        return new PaginatedCarServiceListingsResponse(
            Items: items,
            CurrentPage: request.Page,
            TotalPages: totalPages,
            TotalCount: totalCount
        );
    }

    public async Task<GetCarServiceListingByIdResponse?> GetCarServiceListingByIdAsync(Guid id, Guid? userId)
    {
        var listing = await _carServiceListingRepository.GetCarServiceListingByIdAsync(id);

        if (listing is null)
        {
            return null;
        }

        var isSaved = userId.HasValue && listing.SavedByUsers.Any(s => s.UserId == userId);

        return new GetCarServiceListingByIdResponse(
            listing.Id,
            listing.Title,
            listing.Description,
            listing.Category?.Name ?? ServiceDefaults.UnknownCategory,
            listing.CategoryId,
            listing.PhoneNumber,
            listing.City,
            listing.Address,
            listing.Price,
            listing.ImageUrl,
            isSaved,
            listing.UpdatedAt,
            listing.User?.UserName ?? ServiceDefaults.UnknownUser,
            listing.UserId
        );
    }

    public async Task<GetCarServiceListingByIdResponse> CreateCarServiceListingAsync(CreateCarServiceListingRequest request, Guid userId)
    {
        ValidateCreateRequest(request);

        var imageUrl = await _imageService.SaveImageOrDefaultAsync(
            request.ImageFile,
            userId,
            ImageServiceConstants.CAR_SERVICES_SUB_FOLDER,
            ImageServiceConstants.CAR_SERVICE_IMAGE
        );

        var newListing = new CarServiceListing
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            PhoneNumber = request.PhoneNumber,
            City = request.City,
            Address = request.Address,
            Price = request.Price,
            ImageUrl = imageUrl,
            UpdatedAt = DateTime.UtcNow,
            UserId = userId,
            CategoryId = request.CategoryId
        };

        await _carServiceListingRepository.AddCarServiceListingAsync(newListing);

        var savedListing = await _carServiceListingRepository.GetCarServiceListingByIdAsync(newListing.Id);

        if (savedListing is null)
        {
            throw new NotFoundException(ExceptionMessages.ListingNotFound);
        }

        var isSaved = savedListing.SavedByUsers.Any(s => s.UserId == userId);

        return new GetCarServiceListingByIdResponse(
            savedListing.Id,
            savedListing.Title,
            savedListing.Description,
            savedListing.Category?.Name ?? ServiceDefaults.UnknownCategory,
            savedListing.CategoryId,
            savedListing.PhoneNumber,
            savedListing.City,
            savedListing.Address,
            savedListing.Price,
            savedListing.ImageUrl,
            isSaved,
            savedListing.UpdatedAt,
            savedListing.User?.UserName ?? ServiceDefaults.UnknownUser,
            savedListing.UserId
        );
    }

    public async Task<bool> UpdateCarServiceListingAsync(UpdateCarServiceListingRequest request, Guid userId)
    {
        var listing = await _carServiceListingRepository.GetCarServiceListingByIdAsync(request.Id);

        if (listing is null)
        {
            throw new NotFoundException(ExceptionMessages.ListingNotFound);
        }

        if (listing.UserId != userId)
        {
            throw new UnauthorizedAccessAppException(ExceptionMessages.UnauthorizedListingAccess);
        }

        listing.Title = request.Title;
        listing.Description = request.Description;
        listing.PhoneNumber = request.PhoneNumber;
        listing.City = request.City;
        listing.Address = request.Address;
        listing.Price = request.Price;
        listing.UpdatedAt = DateTime.UtcNow;
        listing.CategoryId = request.CategoryId;

        listing.ImageUrl = await _imageService.UpdateImageIfProvidedAsync(
            request.ImageFile,
            listing.ImageUrl,
            userId,
            ImageServiceConstants.CAR_SERVICES_SUB_FOLDER
        );

        await _carServiceListingRepository.UpdateCarServiceListingAsync(listing);

        return true;
    }

    public async Task<bool> SaveCarServiceListingAsync(Guid listingId, Guid userId)
    {
        return await _carServiceListingRepository.SaveListingAsync(listingId, userId);
    }

    public async Task<bool> UnsaveCarServiceListingAsync(Guid listingId, Guid userId)
    {
        return await _carServiceListingRepository.UnsaveListingAsync(listingId, userId);
    }

    public async Task DeleteCarServiceListingAsync(Guid id, Guid userId)
    {
        var listing = await _carServiceListingRepository.GetCarServiceListingByIdAsync(id);

        if (listing is null)
        {
            throw new NotFoundException(ExceptionMessages.ListingNotFound);
        }

        if (listing.UserId != userId)
        {
            throw new UnauthorizedAccessAppException(ExceptionMessages.UnauthorizedListingAccess);
        }

        await _imageService.DeleteImageAsync(listing.ImageUrl);

        await _carServiceListingRepository.DeleteCarServiceListingAsync(id);
    }

    private static void ValidateCreateRequest(CreateCarServiceListingRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title) ||
            string.IsNullOrWhiteSpace(request.Description) ||
            string.IsNullOrWhiteSpace(request.City) ||
            request.Price < 0 ||
            request.CategoryId <= 0)
        {
            throw new ValidationAppException(ExceptionMessages.InvalidListingData);
        }
    }
}
