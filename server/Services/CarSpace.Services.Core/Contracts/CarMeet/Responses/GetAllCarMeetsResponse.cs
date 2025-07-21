namespace CarSpace.Services.Core.Contracts.CarMeet.Responses;

public sealed record GetAllCarMeetsResponse(
    Guid Id,
    string Title,
    DateTime MeetDate,
    string City,
    string ImageUrl
);
