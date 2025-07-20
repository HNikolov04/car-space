namespace CarSpace.Services.Core.Contracts.CarServiceListing.Responses;

public sealed record GetCarServiceListingByIdResponse(
    Guid Id,
    string Title,
    string Description,
    string Category,
    string PhoneNumber,
    string City,
    string Address,
    decimal? Price,
    string ImageUrl,
    DateTime CreatedAt,
    Guid UserId
);
