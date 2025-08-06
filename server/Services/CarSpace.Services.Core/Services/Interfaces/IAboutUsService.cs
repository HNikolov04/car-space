using CarSpace.Services.Core.Contracts.About.Request;
using CarSpace.Services.Core.Contracts.About.Responses;

namespace CarSpace.Services.Core.Services.Interfaces;

public interface IAboutUsService
{
    Task<GetAboutUsResponse> GetAsync();
    Task UpdateAsync(UpdateAboutUsRequest request);
}
