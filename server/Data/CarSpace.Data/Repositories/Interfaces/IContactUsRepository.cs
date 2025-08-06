using CarSpace.Data.Models.Entities.Contact;

namespace CarSpace.Data.Repositories.Interfaces;

public interface IContactUsRepository
{
    Task<ContactUs?> GetAsync();
    Task UpdateAsync(ContactUs entity);
}
