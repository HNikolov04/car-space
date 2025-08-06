using CarSpace.Data.Models.Entities.CarService;
using CarSpace.Data.Repositories.Interfaces;
using CarSpace.Services.Common.Messages;
using CarSpace.Services.Core.Contracts.CarService.CarServiceCategory.Requests;
using CarSpace.Services.Core.Contracts.CarService.CarServiceCategory.Response;
using CarSpace.Services.Core.Exceptions;
using CarSpace.Services.Core.Services.Interfaces;

namespace CarSpace.Services.Core.Services;

public class CarServiceCategoryService : ICarServiceCategoryService
{
    private readonly ICarServiceCategoryRepository _carServiceCategoryRepository;

    public CarServiceCategoryService(ICarServiceCategoryRepository carServiceCategoryRepository)
    {
        _carServiceCategoryRepository = carServiceCategoryRepository;
    }

    public async Task<List<CarServiceCategoryResponse>> GetAllAsync()
    {
        var categories = await _carServiceCategoryRepository.GetAllAsync();

        return categories
            .Select(c => new CarServiceCategoryResponse(c.Id, c.Name))
            .ToList();
    }

    public async Task<CarServiceCategoryResponse> GetByIdAsync(int id)
    {
        var category = await _carServiceCategoryRepository.GetByIdAsync(id);

        if (category is null)
        {
            throw new NotFoundException(ExceptionMessages.CategoryNotFound);
        }

        return new CarServiceCategoryResponse(category.Id, category.Name);
    }

    public async Task CreateCarServiceCategoryAsync(CreateCarServiceCategoryRequest request)
    {
        ValidateName(request.Name);

        var category = new CarServiceCategory
        {
            Name = request.Name
        };

        await _carServiceCategoryRepository.AddAsync(category);
    }

    public async Task<bool> UpdateCarServiceCategoryAsync(UpdateCarServiceCategoryRequest request)
    {
        ValidateName(request.Name);

        var category = await _carServiceCategoryRepository.GetByIdAsync(request.Id);

        if (category is null)
        {
            throw new NotFoundException(ExceptionMessages.CategoryNotFound);
        }

        category.Name = request.Name;

        await _carServiceCategoryRepository.UpdateAsync(category);

        return true;
    }

    public async Task<bool> DeleteCarServiceCategoryAsync(int id)
    {
        var category = await _carServiceCategoryRepository.GetByIdAsync(id);

        if (category is null)
        {
            throw new NotFoundException(ExceptionMessages.CategoryNotFound);
        }

        await _carServiceCategoryRepository.DeleteAsync(category);

        return true;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ValidationAppException(ExceptionMessages.InvalidCategoryName);
        }
    }
}
