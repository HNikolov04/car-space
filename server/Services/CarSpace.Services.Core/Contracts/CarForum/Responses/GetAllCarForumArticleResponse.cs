namespace CarSpace.Services.Core.Contracts.CarForum.Responses;

public sealed record GetAllCarForumArticleResponse(
    Guid Id,
    string Title,
    string Brand,
    DateTime CreatedAt
);
