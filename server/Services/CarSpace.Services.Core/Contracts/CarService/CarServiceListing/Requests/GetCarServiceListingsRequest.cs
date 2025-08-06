namespace CarSpace.Services.Core.Contracts.CarService.CarServiceListing.Requests;

public sealed record GetCarServiceListingsRequest(
    int Page = 1,
    int PageSize = 8,
    string? SearchTerm = null,
    int? CategoryId = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    bool SavedOnly = false,
    bool MyServicesOnly = false
);

