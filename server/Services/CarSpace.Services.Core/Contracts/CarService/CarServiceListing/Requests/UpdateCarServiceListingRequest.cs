using Microsoft.AspNetCore.Http;

namespace CarSpace.Services.Core.Contracts.CarService.CarServiceListing.Requests;

public sealed record UpdateCarServiceListingRequest(
    Guid Id,
    string Title,
    string Description,
    int CategoryId,
    string PhoneNumber,
    string City,
    string Address,
    decimal Price,
    IFormFile? ImageFile
);
