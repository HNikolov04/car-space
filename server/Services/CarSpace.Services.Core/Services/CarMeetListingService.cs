using CarSpace.Data.Models.Entities.CarMeet;
using CarSpace.Data.Repositories.Interfaces;
using CarSpace.Services.Common.Constants;
using CarSpace.Services.Common.Messages;
using CarSpace.Services.Core.Constants;
using CarSpace.Services.Core.Contracts.CarMeet.Requests;
using CarSpace.Services.Core.Contracts.CarMeet.Responses;
using CarSpace.Services.Core.Exceptions;
using CarSpace.Services.Core.Services.Interfaces;

namespace CarSpace.Services.Core.Services;

public class CarMeetListingService : ICarMeetListingService
{
    private readonly ICarMeetListingRepository _carMeetListingRepository;
    private readonly IImageService _imageService;

    public CarMeetListingService(ICarMeetListingRepository carMeetListingRepository, IImageService imageService)
    {
        _carMeetListingRepository = carMeetListingRepository;
        _imageService = imageService;
    }

    public async Task<PaginatedCarMeetListingsResponse> GetCarMeetsAsync(GetCarMeetListingsRequest request, Guid? userId)
    {
        var (meets, totalCount) = await _carMeetListingRepository.GetFilteredMeetsAsync(
            request.SearchTerm,
            request.FilterByDate,
            request.SavedOnly,
            request.MyMeetsOnly,
            request.JoinedOnly,
            userId,
            request.Page,
            request.PageSize);

        var items = meets.Select(m => new GetAllCarMeetListingsResponse(
            m.Id,
            m.Title,
            m.Description,
            m.City,
            m.ImageUrl,
            m.MeetDate,
            m.UpdatedAt,
            m.User?.UserName ?? ServiceDefaults.UnknownUser,
            userId.HasValue && m.SavedByUsers.Any(u => u.UserId == userId),
            userId.HasValue && m.Participants.Any(p => p.UserId == userId)
        )).ToList();

        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        return new PaginatedCarMeetListingsResponse(items, request.Page, totalPages, totalCount);
    }

    public async Task<GetCarMeetListingByIdResponse?> GetCarMeetByIdAsync(Guid id, Guid? userId)
    {
        var meet = await _carMeetListingRepository.GetCarMeetByIdAsync(id);

        if (meet is null)
        {
            return null;
        }

        var isSaved = userId.HasValue && meet.SavedByUsers.Any(u => u.UserId == userId);
        var isJoined = userId.HasValue && meet.Participants.Any(p => p.UserId == userId);

        return new GetCarMeetListingByIdResponse(
            meet.Id,
            meet.Title,
            meet.Description,
            meet.City,
            meet.Address,
            meet.ImageUrl,
            meet.MeetDate,
            meet.UpdatedAt,
            meet.UserId,
            meet.User?.UserName ?? ServiceDefaults.UnknownUser,
            isSaved,
            isJoined
        );
    }

    public async Task<GetCarMeetListingByIdResponse> CreateCarMeetAsync(CreateCarMeetListingRequest request, Guid userId)
    {
        ValidateCreateRequest(request);

        var imageUrl = await _imageService.SaveImageOrDefaultAsync(
            request.ImageFile,
            userId,
            ImageServiceConstants.CAR_MEETS_SUB_FOLDER,
            ImageServiceConstants.CAR_MEET_IMAGE);

        var entity = new CarMeetListing
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            MeetDate = request.MeetDate,
            City = request.City,
            Address = request.Address,
            ImageUrl = imageUrl,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            UserId = userId
        };

        await _carMeetListingRepository.AddCarMeetAsync(entity);
        await _carMeetListingRepository.JoinCarMeetAsync(entity.Id, userId);

        return new GetCarMeetListingByIdResponse(
            entity.Id,
            entity.Title,
            entity.Description,
            entity.City,
            entity.Address,
            entity.ImageUrl,
            entity.MeetDate,
            entity.UpdatedAt,
            entity.UserId,
            ServiceDefaults.UnknownUser,
            IsSavedByCurrentUser: false,
            IsJoinedByCurrentUser: true
        );
    }

    public async Task<bool> UpdateCarMeetAsync(UpdateCarMeetListingRequest request, Guid userId)
    {
        var existing = await _carMeetListingRepository.GetCarMeetByIdAsync(request.Id);

        if (existing is null)
        {
            throw new NotFoundException(ExceptionMessages.MeetNotFound);
        }

        if (existing.UserId != userId)
        {
            throw new UnauthorizedAccessAppException(ExceptionMessages.UnauthorizedMeetAccess);
        }

        existing.Title = request.Title;
        existing.Description = request.Description;
        existing.MeetDate = request.MeetDate;
        existing.City = request.City;
        existing.Address = request.Address;
        existing.UpdatedAt = DateTime.UtcNow;

        existing.ImageUrl = await _imageService.UpdateImageIfProvidedAsync(
            request.ImageFile,
            existing.ImageUrl,
            userId,
            ImageServiceConstants.CAR_MEETS_SUB_FOLDER);

        await _carMeetListingRepository.UpdateCarMeetAsync(existing);

        return true;
    }

    public async Task DeleteCarMeetAsync(Guid id, Guid userId)
    {
        var meet = await _carMeetListingRepository.GetCarMeetByIdAsync(id);

        if (meet is null)
        {
            throw new NotFoundException(ExceptionMessages.MeetNotFound);
        }

        if (meet.UserId != userId)
        {
            throw new UnauthorizedAccessAppException(ExceptionMessages.UnauthorizedMeetAccess);
        }

        await _imageService.DeleteImageAsync(meet.ImageUrl);
        await _carMeetListingRepository.DeleteCarMeetAsync(id);
    }

    public async Task<bool> SaveCarMeetAsync(Guid meetId, Guid userId)
    {
        return await _carMeetListingRepository.SaveMeetAsync(meetId, userId);
    }

    public async Task<bool> UnsaveCarMeetAsync(Guid meetId, Guid userId)
    {
        return await _carMeetListingRepository.UnsaveMeetAsync(meetId, userId);
    }

    public async Task<bool> JoinCarMeetAsync(Guid meetId, Guid userId)
    {
        return await _carMeetListingRepository.JoinCarMeetAsync(meetId, userId);
    }

    public async Task<bool> LeaveCarMeetAsync(Guid meetId, Guid userId)
    {
        return await _carMeetListingRepository.LeaveCarMeetAsync(meetId, userId);
    }

    public async Task<PaginatedCarMeetListingParticipantsResponse> GetCarMeetParticipantsAsync(Guid meetId, int page, int pageSize)
    {
        var (participants, totalCount) = await _carMeetListingRepository.GetParticipantsAsync(meetId, page, pageSize);

        var items = participants.Select(p => new GetCarMeetListingParticipantResponse(p.Id, p.UserName)).ToList();
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        return new PaginatedCarMeetListingParticipantsResponse(items, page, totalPages, totalCount);
    }

    private static void ValidateCreateRequest(CreateCarMeetListingRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title) ||
            string.IsNullOrWhiteSpace(request.Description) ||
            string.IsNullOrWhiteSpace(request.City) ||
            string.IsNullOrWhiteSpace(request.Address) ||
            request.MeetDate <= DateTime.UtcNow)
        {
            throw new ValidationAppException(ExceptionMessages.InvalidMeetData);
        }
    }
}
