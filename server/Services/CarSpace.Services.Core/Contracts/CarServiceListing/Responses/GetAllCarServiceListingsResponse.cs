namespace CarSpace.Services.Core.Contracts.CarServiceListing.Responses;

public sealed record GetAllCarServiceListingsResponse(
    Guid Id,
    string Title,
    string City,
    string Category,
    decimal? Price
);
