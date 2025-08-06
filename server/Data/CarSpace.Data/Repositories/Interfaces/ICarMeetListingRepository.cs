using CarSpace.Data.Models.Entities.CarMeet;
using CarSpace.Data.Models.Entities.User;

namespace CarSpace.Data.Repositories.Interfaces;

public interface ICarMeetListingRepository
{
    Task<(IEnumerable<CarMeetListing> Meets, int TotalCount)> GetFilteredMeetsAsync(
        string? searchTerm,
        DateTime? filterByDate,
        bool savedOnly,
        bool myMeetsOnly,
        bool joinedOnly,
        Guid? userId,
        int page,
        int pageSize);

    Task<CarMeetListing?> GetCarMeetByIdAsync(Guid id);
    Task AddCarMeetAsync(CarMeetListing meet);
    Task UpdateCarMeetAsync(CarMeetListing meet);
    Task DeleteCarMeetAsync(Guid id);

    Task<bool> SaveMeetAsync(Guid meetId, Guid userId);
    Task<bool> UnsaveMeetAsync(Guid meetId, Guid userId);

    Task<bool> JoinCarMeetAsync(Guid meetId, Guid userId);
    Task<bool> LeaveCarMeetAsync(Guid meetId, Guid userId);

    Task<(IEnumerable<ApplicationUser> Participants, int TotalCount)> GetParticipantsAsync(Guid meetId, int page, int pageSize);
}
