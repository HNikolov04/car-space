using CarSpace.Data.Models.Entities.About;

namespace CarSpace.Data.Repositories.Interfaces;

public interface IAboutUsRepository
{
    Task<AboutUs?> GetAsync();
    Task UpdateAsync(AboutUs entity);
}
