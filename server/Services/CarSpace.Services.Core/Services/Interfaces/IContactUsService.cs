using CarSpace.Services.Core.Contracts.Contact.Requests;
using CarSpace.Services.Core.Contracts.Contact.Responses;

namespace CarSpace.Services.Core.Services.Interfaces;

public interface IContactUsService
{
    Task<GetContactUsResponse> GetAsync();
    Task UpdateAsync(UpdateContactUsRequest request);
}