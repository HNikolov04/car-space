using CarSpace.Data.Models.Entities.CarShop;

namespace CarSpace.Data.Repositories.Interfaces;

public interface ICarShopBrandRepository
{
    Task<IEnumerable<CarShopBrand>> GetAllAsync();
    Task<CarShopBrand?> GetByIdAsync(int id);
    Task CreateAsync(CarShopBrand brand);
    Task UpdateAsync(CarShopBrand brand);
    Task DeleteAsync(CarShopBrand brand);
}