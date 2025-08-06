namespace CarSpace.Services.Core.Contracts.CarMeet.Requests;

public sealed record GetCarMeetListingsRequest(
    int Page = 1,
    int PageSize = 8,
    string? SearchTerm = null,
    DateTime? FilterByDate = null,
    bool SavedOnly = false,
    bool MyMeetsOnly = false,
    bool JoinedOnly = false
);
