namespace CarSpace.Services.Core.Contracts.CarMeet.Responses;

public sealed record GetCarMeetByIdResponse(
    Guid Id,
    string Title,
    string Description,
    DateTime MeetDate,
    string City,
    string Address,
    string ImageUrl,
    DateTime CreatedAt,
    Guid CreatorId
);
