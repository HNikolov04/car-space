namespace CarSpace.Services.Core.Contracts.Contact.Requests;

public sealed record UpdateContactUsRequest(string Title, string Email, string Phone, string Message);

