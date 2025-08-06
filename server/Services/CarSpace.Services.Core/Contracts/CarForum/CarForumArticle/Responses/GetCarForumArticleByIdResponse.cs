namespace CarSpace.Services.Core.Contracts.CarForum.CarForumArticle.Responses;

public sealed record GetCarForumArticleByIdResponse(
    Guid Id,
    string Title,
    string Description,
    int BrandId,
    string BrandName,
    DateTime UpdatedAt,
    string UserNickname,
    Guid UserId,
    bool IsSavedByCurrentUser
);