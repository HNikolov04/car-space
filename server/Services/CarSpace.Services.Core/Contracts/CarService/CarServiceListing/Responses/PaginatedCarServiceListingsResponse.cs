namespace CarSpace.Services.Core.Contracts.CarService.CarServiceListing.Responses;

public sealed record PaginatedCarServiceListingsResponse(
    IEnumerable<GetAllCarServiceListingsResponse> Items,
    int CurrentPage,
    int TotalPages,
    int TotalCount
);
