namespace CarSpace.Services.Core.Contracts.CarMeet.Responses;

public sealed record PaginatedCarMeetListingsResponse(
    IEnumerable<GetAllCarMeetListingsResponse> Items,
    int CurrentPage,
    int TotalPages,
    int TotalCount
);
