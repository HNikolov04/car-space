namespace CarSpace.Services.Core.Contracts.CarMeet.Responses;

public sealed record GetCarMeetListingParticipantResponse(
    Guid UserId,
    string Username
);
