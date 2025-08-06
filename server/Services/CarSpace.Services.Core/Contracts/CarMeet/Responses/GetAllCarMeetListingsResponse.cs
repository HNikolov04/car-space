namespace CarSpace.Services.Core.Contracts.CarMeet.Responses;

public sealed record GetAllCarMeetListingsResponse(
    Guid Id,
    string Title,
    string Description,
    string City,
    string ImageUrl,
    DateTime MeetDate,
    DateTime UpdatedAt,
    string UserNickname,
    bool IsSavedByCurrentUser,
    bool IsJoinedByCurrentUser
);