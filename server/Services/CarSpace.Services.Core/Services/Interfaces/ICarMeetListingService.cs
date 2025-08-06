using CarSpace.Services.Core.Contracts.CarMeet.Requests;
using CarSpace.Services.Core.Contracts.CarMeet.Responses;

namespace CarSpace.Services.Core.Services.Interfaces;

public interface ICarMeetListingService
{
    Task<PaginatedCarMeetListingsResponse> GetCarMeetsAsync(GetCarMeetListingsRequest request, Guid? userId);
    Task<GetCarMeetListingByIdResponse?> GetCarMeetByIdAsync(Guid id, Guid? userId);
    Task<GetCarMeetListingByIdResponse> CreateCarMeetAsync(CreateCarMeetListingRequest request, Guid userId);
    Task<bool> UpdateCarMeetAsync(UpdateCarMeetListingRequest request, Guid userId);
    Task DeleteCarMeetAsync(Guid id, Guid userId);
    Task<bool> SaveCarMeetAsync(Guid meetId, Guid userId);
    Task<bool> UnsaveCarMeetAsync(Guid meetId, Guid userId);
    Task<bool> JoinCarMeetAsync(Guid meetId, Guid userId);
    Task<bool> LeaveCarMeetAsync(Guid meetId, Guid userId);
    Task<PaginatedCarMeetListingParticipantsResponse> GetCarMeetParticipantsAsync(Guid meetId, int page, int pageSize);
}