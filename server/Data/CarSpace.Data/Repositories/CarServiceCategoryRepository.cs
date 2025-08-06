using CarSpace.Data.Models.Entities.CarService;
using CarSpace.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarSpace.Data.Repositories;

public class CarServiceCategoryRepository : ICarServiceCategoryRepository
{
    private readonly CarSpaceDbContext _context;

    public CarServiceCategoryRepository(CarSpaceDbContext context)
    {
        _context = context;
    }

    public async Task<List<CarServiceCategory>> GetAllAsync()
    {
        return await _context.CarServiceCategories.AsNoTracking().ToListAsync();
    }

    public async Task<CarServiceCategory?> GetByIdAsync(int id)
    {
        return await _context.CarServiceCategories.FindAsync(id);
    }

    public async Task AddAsync(CarServiceCategory category)
    {
        _context.CarServiceCategories.Add(category);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CarServiceCategory category)
    {
        _context.CarServiceCategories.Update(category);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(CarServiceCategory category)
    {
        _context.CarServiceCategories.Remove(category);

        await _context.SaveChangesAsync();
    }
}
