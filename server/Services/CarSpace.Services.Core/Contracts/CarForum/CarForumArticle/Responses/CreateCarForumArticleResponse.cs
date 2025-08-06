namespace CarSpace.Services.Core.Contracts.CarForum.CarForumArticle.Responses;

public sealed record CreateCarForumArticleResponse(
    Guid Id,
    string Title,
    string Description,
    string BrandName,
    DateTime UpdatedAt,
    Guid UserId
);
