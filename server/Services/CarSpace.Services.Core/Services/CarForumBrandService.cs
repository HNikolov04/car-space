using CarSpace.Data.Models.Entities.CarForum;
using CarSpace.Data.Repositories.Interfaces;
using CarSpace.Services.Common.Messages;
using CarSpace.Services.Core.Contracts.CarForum.CarForumBrand.Requests;
using CarSpace.Services.Core.Contracts.CarForum.CarForumBrand.Response;
using CarSpace.Services.Core.Exceptions;
using CarSpace.Services.Core.Services.Interfaces;

namespace CarSpace.Services.Core.Services;

public class CarForumBrandService : ICarForumBrandService
{
    private readonly ICarForumBrandRepository _carForumBrandRepository;

    public CarForumBrandService(ICarForumBrandRepository carForumBrandRepository)
    {
        _carForumBrandRepository = carForumBrandRepository;
    }

    public async Task<IEnumerable<CarForumBrandResponse>> GetAllAsync()
    {
        var brands = await _carForumBrandRepository.GetAllAsync();

        return brands.Select(b => new CarForumBrandResponse(b.Id, b.Name));
    }

    public async Task<CarForumBrandResponse?> GetByIdAsync(int id)
    {
        var brand = await _carForumBrandRepository.GetByIdAsync(id);

        return brand is null
            ? null
            : new CarForumBrandResponse(brand.Id, brand.Name);
    }

    public async Task<CarForumBrandResponse> CreateAsync(CreateCarForumBrandRequest request)
    {
        ValidateCreateRequest(request);

        var entity = new CarForumBrand
        {
            Name = request.Name
        };

        await _carForumBrandRepository.CreateAsync(entity);

        return new CarForumBrandResponse(entity.Id, entity.Name);
    }

    public async Task<bool> UpdateAsync(UpdateCarForumBrandRequest request)
    {
        ValidateUpdateRequest(request);

        var existing = await _carForumBrandRepository.GetByIdAsync(request.Id);

        if (existing is null)
        {
            throw new NotFoundException(ExceptionMessages.BrandNotFound);
        }

        existing.Name = request.Name;

        await _carForumBrandRepository.UpdateAsync(existing);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _carForumBrandRepository.GetByIdAsync(id);

        if (existing is null)
        {
            throw new NotFoundException(ExceptionMessages.BrandNotFound);
        }

        await _carForumBrandRepository.DeleteAsync(id);

        return true;
    }

    private static void ValidateCreateRequest(CreateCarForumBrandRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ValidationAppException(ExceptionMessages.InvalidBrandData);
        }
    }

    private static void ValidateUpdateRequest(UpdateCarForumBrandRequest request)
    {
        if (request.Id <= 0 || string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ValidationAppException(ExceptionMessages.InvalidBrandData);
        }
    }
}
