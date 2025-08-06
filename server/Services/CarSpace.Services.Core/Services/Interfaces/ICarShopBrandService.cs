using CarSpace.Data.Models.Entities.CarShop;
using CarSpace.Services.Core.Contracts.CarShop.CarShopBrand.Requests;
using CarSpace.Services.Core.Contracts.CarShop.CarShopBrand.Response;

namespace CarSpace.Services.Core.Services.Interfaces;

public interface ICarShopBrandService
{
    Task<IEnumerable<CarShopBrandResponse>> GetAllCarShopBrandsAsync();
    Task<CarShopBrandResponse?> GetCarShopBrandByIdAsync(int id);
    Task<CarShopBrand> CreateCarShopBrandAsync(CreateCarShopBrandRequest request);
    Task<bool> UpdateCarShopBrandAsync(UpdateCarShopBrandRequest request);
    Task<bool> DeleteCarShopBrandAsync(int id);
}