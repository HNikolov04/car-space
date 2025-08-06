namespace CarSpace.Services.Core.Contracts.CarService.CarServiceListing.Responses;

public sealed record GetCarServiceListingByIdResponse(
    Guid Id,
    string Title,
    string Description,
    string CategoryName,
    int CategoryId,
    string PhoneNumber,
    string City,
    string Address,
    decimal Price,
    string ImageUrl,
    bool IsSavedByCurrentUser,
    DateTime UpdatedAt,
    string UserNickname,
    Guid UserId
);