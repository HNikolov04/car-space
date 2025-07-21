using CarSpace.Data.Models.Entities.Meet;
using CarSpace.Data.Repositories.Interfaces;
using CarSpace.Services.Core.Contracts.CarMeet.Requests;
using CarSpace.Services.Core.Contracts.CarMeet.Responses;
using CarSpace.Services.Core.Interfaces;

namespace CarSpace.Services.Core;

public class CarMeetService : ICarMeetService
{
    private readonly ICarMeetRepository _carMeetRepository;

    public CarMeetService(ICarMeetRepository carMeetRepository)
    {
        _carMeetRepository = carMeetRepository;
    }

    public async Task<IEnumerable<GetAllCarMeetsResponse>> GetAllCarMeetsAsync()
    {
        var meets = await _carMeetRepository.GetAllCarMeetsAsync();
        return meets.Select(m => new GetAllCarMeetsResponse(
            m.Id, m.Title, m.MeetDate, m.City, m.ImageUrl));
    }

    public async Task<GetCarMeetByIdResponse?> GetCarMeetByIdAsync(Guid id)
    {
        var m = await _carMeetRepository.GetCarMeetByIdAsync(id);
        return m is null ? null : new GetCarMeetByIdResponse(
            m.Id, m.Title, m.Description, m.MeetDate,
            m.City, m.Address, m.ImageUrl, m.CreatedAt, m.CreatorId);
    }

    public async Task<GetCarMeetByIdResponse> CreateCarMeetAsync(CreateCarMeetRequest request)
    {
        var entity = new CarMeet
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            MeetDate = request.MeetDate,
            City = request.City,
            Address = request.Address,
            ImageUrl = request.ImageUrl,
            CreatedAt = DateTime.UtcNow,
            CreatorId = request.CreatorId
        };

        await _carMeetRepository.AddCarMeetAsync(entity);

        return new GetCarMeetByIdResponse(
            entity.Id, entity.Title, entity.Description, entity.MeetDate,
            entity.City, entity.Address, entity.ImageUrl, entity.CreatedAt, entity.CreatorId);
    }

    public async Task<bool> UpdateCarMeetAsync(UpdateCarMeetRequest request)
    {
        var existing = await _carMeetRepository.GetCarMeetByIdAsync(request.Id);
        if (existing is null) return false;

        existing.Title = request.Title;
        existing.Description = request.Description;
        existing.MeetDate = request.MeetDate;
        existing.City = request.City;
        existing.Address = request.Address;
        existing.ImageUrl = request.ImageUrl;

        await _carMeetRepository.UpdateCarMeetAsync(existing);
        return true;
    }

    public async Task DeleteCarMeetAsync(Guid id)
    {
        await _carMeetRepository.DeleteCarMeetAsync(id);
    }

    public async Task<bool> JoinCarMeetAsync(Guid meetId, Guid userId)
    {
        var meet = await _carMeetRepository.GetCarMeetByIdAsync(meetId);
        if (meet is null) return false;

        await _carMeetRepository.JoinCarMeetAsync(meet, userId);
        return true;
    }
}
