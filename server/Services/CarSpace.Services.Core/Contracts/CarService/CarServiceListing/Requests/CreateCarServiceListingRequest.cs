using Microsoft.AspNetCore.Http;

namespace CarSpace.Services.Core.Contracts.CarService.CarServiceListing.Requests;

public sealed record CreateCarServiceListingRequest(
    string Title,
    string Description,
    int CategoryId,
    string PhoneNumber,
    string City,
    string Address,
    decimal Price,
    IFormFile? ImageFile
);
