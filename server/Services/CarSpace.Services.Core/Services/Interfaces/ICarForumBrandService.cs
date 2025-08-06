using CarSpace.Services.Core.Contracts.CarForum.CarForumBrand.Requests;
using CarSpace.Services.Core.Contracts.CarForum.CarForumBrand.Response;

namespace CarSpace.Services.Core.Services.Interfaces;

public interface ICarForumBrandService
{
    Task<IEnumerable<CarForumBrandResponse>> GetAllAsync();
    Task<CarForumBrandResponse?> GetByIdAsync(int id);
    Task<CarForumBrandResponse> CreateAsync(CreateCarForumBrandRequest request);
    Task<bool> UpdateAsync(UpdateCarForumBrandRequest request);
    Task<bool> DeleteAsync(int id);
}