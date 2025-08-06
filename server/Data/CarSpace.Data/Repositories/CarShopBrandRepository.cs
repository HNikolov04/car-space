using CarSpace.Data.Models.Entities.CarShop;
using CarSpace.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarSpace.Data.Repositories;

public class CarShopBrandRepository : ICarShopBrandRepository
{
    private readonly CarSpaceDbContext _context;

    public CarShopBrandRepository(CarSpaceDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CarShopBrand>> GetAllAsync()
    {
        return await _context.CarShopBrands.AsNoTracking().ToListAsync();
    }

    public async Task<CarShopBrand?> GetByIdAsync(int id)
    {
        return await _context.CarShopBrands.FindAsync(id);
    }

    async Task ICarShopBrandRepository.CreateAsync(CarShopBrand brand)
    {
        _context.CarShopBrands.Add(brand);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CarShopBrand brand)
    {
        _context.CarShopBrands.Update(brand);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(CarShopBrand brand)
    {
        _context.CarShopBrands.Remove(brand);

        await _context.SaveChangesAsync();
    }
}
