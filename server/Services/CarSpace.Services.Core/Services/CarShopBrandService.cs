using CarSpace.Data.Models.Entities.CarShop;
using CarSpace.Data.Repositories.Interfaces;
using CarSpace.Services.Common.Messages;
using CarSpace.Services.Core.Contracts.CarShop.CarShopBrand.Requests;
using CarSpace.Services.Core.Contracts.CarShop.CarShopBrand.Response;
using CarSpace.Services.Core.Exceptions;
using CarSpace.Services.Core.Services.Interfaces;

namespace CarSpace.Services.Core.Services;

public class CarShopBrandService : ICarShopBrandService
{
    private readonly ICarShopBrandRepository _carShopBrandRepository;

    public CarShopBrandService(ICarShopBrandRepository carShopBrandRepository)
    {
        _carShopBrandRepository = carShopBrandRepository;
    }

    public async Task<IEnumerable<CarShopBrandResponse>> GetAllCarShopBrandsAsync()
    {
        var brands = await _carShopBrandRepository.GetAllAsync();

        return brands.Select(b => new CarShopBrandResponse(b.Id, b.Name));
    }

    public async Task<CarShopBrandResponse?> GetCarShopBrandByIdAsync(int id)
    {
        var brand = await _carShopBrandRepository.GetByIdAsync(id);

        if (brand is null)
        {
            return null;
        }

        return new CarShopBrandResponse(brand.Id, brand.Name);
    }
    

    public async Task<CarShopBrand> CreateCarShopBrandAsync(CreateCarShopBrandRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ValidationAppException(ExceptionMessages.InvalidBrandData);
        }

        var brand = new CarShopBrand
        {
            Name = request.Name
        };

        await _carShopBrandRepository.CreateAsync(brand);

        return brand;
    }

    public async Task<bool> UpdateCarShopBrandAsync(UpdateCarShopBrandRequest request)
    {
        var existing = await _carShopBrandRepository.GetByIdAsync(request.Id);

        if (existing is null)
        {
            throw new NotFoundException(ExceptionMessages.BrandNotFound);
        }

        existing.Name = request.Name;

        await _carShopBrandRepository.UpdateAsync(existing);

        return true;
    }

    public async Task<bool> DeleteCarShopBrandAsync(int id)
    {
        var existing = await _carShopBrandRepository.GetByIdAsync(id);

        if (existing is null)
        {
            throw new NotFoundException(ExceptionMessages.BrandNotFound);
        }

        await _carShopBrandRepository.DeleteAsync(existing);

        return true;
    }
}
