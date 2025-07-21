using CarSpace.Data.Models.Entities.Meet;

namespace CarSpace.Data.Repositories.Interfaces;

public interface ICarMeetRepository
{
    Task<IEnumerable<CarMeet>> GetAllCarMeetsAsync();
    Task<CarMeet?> GetCarMeetByIdAsync(Guid id);
    Task AddCarMeetAsync(CarMeet meet);
    Task UpdateCarMeetAsync(CarMeet meet);
    Task DeleteCarMeetAsync(Guid id);
    Task JoinCarMeetAsync(CarMeet meet, Guid userId);
}