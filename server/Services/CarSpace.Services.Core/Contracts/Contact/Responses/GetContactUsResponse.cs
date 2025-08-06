namespace CarSpace.Services.Core.Contracts.Contact.Responses;

public sealed record GetContactUsResponse(string Title, string Email, string Phone, string Message);

