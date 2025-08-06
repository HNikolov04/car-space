namespace CarSpace.Services.Core.Contracts.CarShop.CarShopListing.Responses;

public record PaginatedCarShopListingsResponse(
    List<GetAllCarShopListingsResponse> Items,
    int CurrentPage,
    int TotalPages,
    int TotalCount);
