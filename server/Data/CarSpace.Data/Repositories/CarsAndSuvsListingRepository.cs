using CarSpace.Data.Models.Entities.CarShop;
using CarSpace.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarSpace.Data.Repositories;

public class CarsAndSuvsListingRepository : ICarsAndSuvsListingRepository
{
    private readonly CarSpaceDbContext _context;

    public CarsAndSuvsListingRepository(CarSpaceDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CarsAndSuvsListing>> GetAllCarsAndSuvsListingsAsync()
    {
        return await _context.CarsAndSuvsListings.ToListAsync();
    }

    public async Task<CarsAndSuvsListing?> GetCarsAndSuvsListingByIdAsync(Guid id)
    {
        return await _context.CarsAndSuvsListings.FindAsync(id);
    }

    public async Task AddCarsAndSuvsListingAsync(CarsAndSuvsListing listing)
    {
        _context.CarsAndSuvsListings.Add(listing);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCarsAndSuvsListingAsync(CarsAndSuvsListing listing)
    {
        _context.CarsAndSuvsListings.Update(listing);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCarsAndSuvsListingAsync(Guid id)
    {
        var listing = await _context.CarsAndSuvsListings.FindAsync(id);
        if (listing is not null)
        {
            _context.CarsAndSuvsListings.Remove(listing);
            await _context.SaveChangesAsync();
        }
    }
}
