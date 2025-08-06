using CarSpace.Data.Models.Entities.CarMeet;
using CarSpace.Data.Models.Entities.User;
using CarSpace.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarSpace.Data.Repositories;

public class CarMeetListingRepository : ICarMeetListingRepository
{
    private readonly CarSpaceDbContext _context;

    public CarMeetListingRepository(CarSpaceDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<CarMeetListing> Meets, int TotalCount)> GetFilteredMeetsAsync(
        string? searchTerm,
        DateTime? filterByDate,
        bool savedOnly,
        bool myMeetsOnly,
        bool joinedOnly,
        Guid? userId,
        int page,
        int pageSize)
    {
        var query = _context.CarMeetListings
            .Include(m => m.SavedByUsers)
            .Include(m => m.Participants)
            .Include(m => m.User)
            .AsQueryable();

        query = ApplyFilters(query, searchTerm, filterByDate, savedOnly, myMeetsOnly, joinedOnly, userId);

        var totalCount = await query.CountAsync();

        var meets = await query
            .OrderByDescending(m => m.MeetDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (meets, totalCount);
    }

    public async Task<CarMeetListing?> GetCarMeetByIdAsync(Guid id)
    {
        return await _context.CarMeetListings
            .Include(m => m.Participants)
            .Include(m => m.SavedByUsers)
            .Include(m => m.User)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task AddCarMeetAsync(CarMeetListing meet)
    {
        await _context.CarMeetListings.AddAsync(meet);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCarMeetAsync(CarMeetListing meet)
    {
        _context.CarMeetListings.Update(meet);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCarMeetAsync(Guid id)
    {
        var meet = await _context.CarMeetListings.FindAsync(id);

        if (meet != null)
        {
            _context.CarMeetListings.Remove(meet);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> SaveMeetAsync(Guid meetId, Guid userId)
    {
        var alreadySaved = await _context.UserSavedCarMeetListings
            .AnyAsync(x => x.UserId == userId && x.CarMeetId == meetId);

        if (alreadySaved)
        {
            return false;
        }

        _context.UserSavedCarMeetListings.Add(new UserSavedCarMeetListing
        {
            CarMeetId = meetId,
            UserId = userId
        });

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UnsaveMeetAsync(Guid meetId, Guid userId)
    {
        var entry = await _context.UserSavedCarMeetListings
            .FirstOrDefaultAsync(x => x.UserId == userId && x.CarMeetId == meetId);

        if (entry == null)
        {
            return false;
        }

        _context.UserSavedCarMeetListings.Remove(entry);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> JoinCarMeetAsync(Guid meetId, Guid userId)
    {
        var alreadyJoined = await _context.UserJoinedCarMeetListings
            .AnyAsync(x => x.UserId == userId && x.CarMeetId == meetId);

        if (alreadyJoined)
        {
            return false;
        }

        _context.UserJoinedCarMeetListings.Add(new UserJoinedCarMeetListing
        {
            CarMeetId = meetId,
            UserId = userId
        });

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> LeaveCarMeetAsync(Guid meetId, Guid userId)
    {
        var join = await _context.UserJoinedCarMeetListings
            .FirstOrDefaultAsync(x => x.UserId == userId && x.CarMeetId == meetId);

        if (join == null)
        {
            return false;
        }

        _context.UserJoinedCarMeetListings.Remove(join);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<(IEnumerable<ApplicationUser> Participants, int TotalCount)> GetParticipantsAsync(Guid meetId, int page, int pageSize)
    {
        var query = _context.UserJoinedCarMeetListings
            .Where(x => x.CarMeetId == meetId)
            .Select(x => x.User);

        var totalCount = await query.CountAsync();

        var users = await query
            .OrderBy(u => u.UserName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (users, totalCount);
    }

    private static IQueryable<CarMeetListing> ApplyFilters(
        IQueryable<CarMeetListing> query,
        string? searchTerm,
        DateTime? filterByDate,
        bool savedOnly,
        bool myMeetsOnly,
        bool joinedOnly,
        Guid? userId)
    {
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(m => m.Title.Contains(searchTerm));
        }

        if (filterByDate.HasValue)
        {
            query = query.Where(m => m.MeetDate.Date == filterByDate.Value.Date);
        }

        if (savedOnly && userId.HasValue)
        {
            query = query.Where(m => m.SavedByUsers.Any(u => u.UserId == userId.Value));
        }

        if (myMeetsOnly && userId.HasValue)
        {
            query = query.Where(m => m.UserId == userId.Value);
        }

        if (joinedOnly && userId.HasValue)
        {
            query = query.Where(m => m.Participants.Any(u => u.UserId == userId.Value));
        }

        return query;
    }
}
