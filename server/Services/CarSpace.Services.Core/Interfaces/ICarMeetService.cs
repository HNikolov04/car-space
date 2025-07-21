using CarSpace.Services.Core.Contracts.CarMeet.Requests;
using CarSpace.Services.Core.Contracts.CarMeet.Responses;

namespace CarSpace.Services.Core.Interfaces;

public interface ICarMeetService
{
    Task<IEnumerable<GetAllCarMeetsResponse>> GetAllCarMeetsAsync();
    Task<GetCarMeetByIdResponse?> GetCarMeetByIdAsync(Guid id);
    Task<GetCarMeetByIdResponse> CreateCarMeetAsync(CreateCarMeetRequest request);
    Task<bool> UpdateCarMeetAsync(UpdateCarMeetRequest request);
    Task DeleteCarMeetAsync(Guid id);
    Task<bool> JoinCarMeetAsync(Guid meetId, Guid userId);
}
