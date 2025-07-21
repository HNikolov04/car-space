using CarSpace.Data.Models.Entities.Meet;
using CarSpace.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarSpace.Data.Repositories;

public class CarMeetRepository : ICarMeetRepository
{
    private readonly CarSpaceDbContext _context;

    public CarMeetRepository(CarSpaceDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CarMeet>> GetAllCarMeetsAsync()
    {
        return await _context.CarMeets.ToListAsync();
    }

    public async Task<CarMeet?> GetCarMeetByIdAsync(Guid id)
    {
        return await _context.CarMeets
            .Include(m => m.Participants)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task AddCarMeetAsync(CarMeet meet)
    {
        await _context.CarMeets.AddAsync(meet);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCarMeetAsync(CarMeet meet)
    {
        _context.CarMeets.Update(meet);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCarMeetAsync(Guid id)
    {
        var meet = await GetCarMeetByIdAsync(id);
        if (meet is not null)
        {
            _context.CarMeets.Remove(meet);
            await _context.SaveChangesAsync();
        }
    }

    public async Task JoinCarMeetAsync(CarMeet meet, Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user is not null && !meet.Participants.Contains(user))
        {
            meet.Participants.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
