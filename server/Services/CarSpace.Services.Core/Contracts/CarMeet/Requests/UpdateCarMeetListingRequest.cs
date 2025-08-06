using Microsoft.AspNetCore.Http;

namespace CarSpace.Services.Core.Contracts.CarMeet.Requests;

public sealed record UpdateCarMeetListingRequest(
    Guid Id,
    string Title,
    string Description,
    DateTime MeetDate,
    string City,
    string Address,
    IFormFile? ImageFile
);
