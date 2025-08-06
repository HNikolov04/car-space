using CarSpace.Data.Repositories.Interfaces;
using CarSpace.Services.Common.Messages;
using CarSpace.Services.Core.Contracts.Contact.Requests;
using CarSpace.Services.Core.Contracts.Contact.Responses;
using CarSpace.Services.Core.Exceptions;
using CarSpace.Services.Core.Services.Interfaces;

namespace CarSpace.Services.Core.Services;

public class ContactUsService : IContactUsService
{
    private readonly IContactUsRepository _contactUsRepository;

    public ContactUsService(IContactUsRepository contactUsRepository)
    {
        _contactUsRepository = contactUsRepository;
    }

    public async Task<GetContactUsResponse> GetAsync()
    {
        var contact = await _contactUsRepository.GetAsync();

        if (contact is null)
        {
            throw new NotFoundException(ExceptionMessages.ContactInfoNotSet);
        }

        return new GetContactUsResponse(
            contact.Title,
            contact.Email,
            contact.Phone,
            contact.Message
        );
    }

    public async Task UpdateAsync(UpdateContactUsRequest request)
    {
        var contact = await _contactUsRepository.GetAsync();

        if (contact is null)
        {
            throw new NotFoundException(ExceptionMessages.ContactInfoNotSet);
        }

        contact.Email = request.Email;
        contact.Phone = request.Phone;
        contact.Message = request.Message;
        contact.UpdatedAt = DateTime.UtcNow;

        await _contactUsRepository.UpdateAsync(contact);
    }
}
