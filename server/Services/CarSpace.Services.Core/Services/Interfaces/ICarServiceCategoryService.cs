using CarSpace.Services.Core.Contracts.CarService.CarServiceCategory.Requests;
using CarSpace.Services.Core.Contracts.CarService.CarServiceCategory.Response;

namespace CarSpace.Services.Core.Services.Interfaces;

public interface ICarServiceCategoryService
{
    Task<List<CarServiceCategoryResponse>> GetAllAsync();
    Task<CarServiceCategoryResponse> GetByIdAsync(int id);
    Task CreateCarServiceCategoryAsync(CreateCarServiceCategoryRequest request);
    Task<bool> UpdateCarServiceCategoryAsync(UpdateCarServiceCategoryRequest request);
    Task<bool> DeleteCarServiceCategoryAsync(int id);
}
