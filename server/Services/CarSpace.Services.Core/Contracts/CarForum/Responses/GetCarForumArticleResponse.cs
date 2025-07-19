namespace CarSpace.Services.Core.Contracts.CarForum.Responses;

public sealed record GetCarForumArticleResponse(
    Guid Id,
    string Title,
    string Description,
    string Brand,
    DateTime CreatedAt,
    Guid UserId
);
