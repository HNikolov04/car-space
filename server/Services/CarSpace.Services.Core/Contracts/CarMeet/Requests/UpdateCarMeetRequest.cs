namespace CarSpace.Services.Core.Contracts.CarMeet.Requests;

public sealed record UpdateCarMeetRequest(
Guid Id,
string Title,
string Description,
DateTime MeetDate,
string City,
string Address,
string ImageUrl
);
