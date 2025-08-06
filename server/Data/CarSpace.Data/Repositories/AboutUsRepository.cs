using CarSpace.Data.Models.Entities.About;
using CarSpace.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarSpace.Data.Repositories;

public class AboutUsRepository : IAboutUsRepository
{
    private readonly CarSpaceDbContext _context;

    public AboutUsRepository(CarSpaceDbContext context)
    {
        _context = context;
    }

    public async Task<AboutUs?> GetAsync()
    {
        return await _context.AboutUs.FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(AboutUs entity)
    {
        _context.AboutUs.Update(entity);

        await _context.SaveChangesAsync();
    }
}