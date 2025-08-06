using CarSpace.Data.Models.Entities.CarForum;
using CarSpace.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarSpace.Data.Repositories;

public class CarForumBrandRepository : ICarForumBrandRepository
{
    private readonly CarSpaceDbContext _context;

    public CarForumBrandRepository(CarSpaceDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CarForumBrand>> GetAllAsync()
    {
        return await _context.CarForumBrands.ToListAsync();
    }

    public async Task<CarForumBrand?> GetByIdAsync(int id)
    {
        return await _context.CarForumBrands.FindAsync(id);
    }

    public async Task CreateAsync(CarForumBrand brand)
    {
        _context.CarForumBrands.Add(brand);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CarForumBrand brand)
    {
        _context.CarForumBrands.Update(brand);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var brand = await _context.CarForumBrands.FindAsync(id);

        if (brand != null)
        {
            _context.CarForumBrands.Remove(brand);

            await _context.SaveChangesAsync();
        }
    }
}
