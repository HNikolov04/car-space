namespace CarSpace.Services.Core.Contracts.CarMeet.Responses;

public sealed record GetCarMeetListingByIdResponse(
    Guid Id,
    string Title,
    string Description,
    string City,
    string Address,
    string ImageUrl,
    DateTime MeetDate,
    DateTime UpdatedAt,
    Guid UserId,
    string UserNickname,
    bool IsSavedByCurrentUser,
    bool IsJoinedByCurrentUser
);