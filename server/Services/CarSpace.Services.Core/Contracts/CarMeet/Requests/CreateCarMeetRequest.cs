namespace CarSpace.Services.Core.Contracts.CarMeet.Requests;

public sealed record CreateCarMeetRequest(
    string Title,
    string Description,
    DateTime MeetDate,
    string City,
    string Address,
    string ImageUrl,
    Guid CreatorId
);
