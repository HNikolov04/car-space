using CarSpace.Data.Models.Entities.CarService;

namespace CarSpace.Data.Repositories.Interfaces;

public interface ICarServiceCategoryRepository
{
    Task<List<CarServiceCategory>> GetAllAsync();
    Task<CarServiceCategory?> GetByIdAsync(int id);
    Task AddAsync(CarServiceCategory category);
    Task UpdateAsync(CarServiceCategory category);
    Task DeleteAsync(CarServiceCategory category);
}
