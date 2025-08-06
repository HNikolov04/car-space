namespace CarSpace.Services.Core.Contracts.CarForum.CarForumComment.Responses;

public sealed record GetAllCarForumCommentsForArticlesResponse(
    Guid Id,
    string Content,
    DateTime CreatedAt,
    Guid UserId,
    string UserNickname,
    Guid ArticleId
);