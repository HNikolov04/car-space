using CarSpace.Data.Models.Entities.CarForum;

namespace CarSpace.Data.Repositories.Interfaces;

public interface ICarForumBrandRepository
{
    Task<IEnumerable<CarForumBrand>> GetAllAsync();
    Task<CarForumBrand?> GetByIdAsync(int id);
    Task CreateAsync(CarForumBrand brand);
    Task UpdateAsync(CarForumBrand brand);
    Task DeleteAsync(int id);
}
