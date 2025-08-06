using CarSpace.Data.Models.Entities.Contact;
using CarSpace.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarSpace.Data.Repositories;

public class ContactUsRepository : IContactUsRepository
{
    private readonly CarSpaceDbContext _context;

    public ContactUsRepository(CarSpaceDbContext context)
    {
        _context = context;
    }

    public async Task<ContactUs?> GetAsync()
    {
        return await _context.ContactUs.FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(ContactUs entity)
    {
        _context.ContactUs.Update(entity);

        await _context.SaveChangesAsync();
    }
}