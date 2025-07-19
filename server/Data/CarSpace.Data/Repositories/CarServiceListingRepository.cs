using CarSpace.Data.Models.Entities.CarServiceListing;
using CarSpace.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarSpace.Data.Repositories;

public class CarServiceListingRepository : ICarServiceListingRepository
{
    private readonly CarSpaceDbContext _context;

    public CarServiceListingRepository(CarSpaceDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CarServiceListing>> GetAllCarServiceListingsAsync()
    {
        return await _context.CarServiceListings
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<CarServiceListing?> GetCarServiceListingByIdAsync(Guid id)
    {
        return await _context.CarServiceListings
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task AddCarServiceListingAsync(CarServiceListing listing)
    {
        _context.CarServiceListings.Add(listing);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCarServiceListingAsync(CarServiceListing listing)
    {
        _context.CarServiceListings.Update(listing);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCarServiceListingAsync(Guid id)
    {
        var entity = await _context.CarServiceListings.FindAsync(id);
        if (entity != null)
        {
            _context.CarServiceListings.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
