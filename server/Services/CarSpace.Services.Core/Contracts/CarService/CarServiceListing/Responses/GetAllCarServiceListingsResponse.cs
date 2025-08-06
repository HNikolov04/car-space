namespace CarSpace.Services.Core.Contracts.CarService.CarServiceListing.Responses;

public sealed record GetAllCarServiceListingsResponse(
    Guid Id,
    string Title,
    string Description,
    string City,
    string CategoryName,
    decimal Price,
    string ImageUrl,
    DateTime UpdatedAt,
    bool IsSavedByCurrentUser,
    string UserNickname
);