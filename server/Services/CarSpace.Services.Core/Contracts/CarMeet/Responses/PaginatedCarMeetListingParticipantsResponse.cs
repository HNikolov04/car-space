namespace CarSpace.Services.Core.Contracts.CarMeet.Responses;

public sealed record PaginatedCarMeetListingParticipantsResponse(
    IEnumerable<GetCarMeetListingParticipantResponse> Items,
    int CurrentPage,
    int TotalPages,
    int TotalCount
);
