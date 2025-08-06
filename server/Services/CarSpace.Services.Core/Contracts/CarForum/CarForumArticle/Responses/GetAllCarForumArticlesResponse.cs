namespace CarSpace.Services.Core.Contracts.CarForum.CarForumArticle.Responses;

public sealed record GetAllCarForumArticlesResponse(
    Guid Id,
    string Title,
    string Description,
    string BrandName,
    string UserNickname,
    DateTime UpdatedAt,
    bool IsSavedByCurrentUser
);