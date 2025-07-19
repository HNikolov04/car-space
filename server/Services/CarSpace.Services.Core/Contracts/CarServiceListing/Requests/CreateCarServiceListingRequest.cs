namespace CarSpace.Services.Core.Contracts.CarServiceListing.Requests;

public sealed record CreateCarServiceListingRequest(
    string Title,
    string Description,
    string Category,
    string PhoneNumber,
    string City,
    string Address,
    decimal? Price,
    Guid UserId
);
