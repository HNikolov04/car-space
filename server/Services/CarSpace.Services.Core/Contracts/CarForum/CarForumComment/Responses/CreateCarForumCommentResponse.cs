namespace CarSpace.Services.Core.Contracts.CarForum.CarForumComment.Responses;

public sealed record CreateCarForumCommentResponse(
    Guid Id,
    string Content,
    DateTime UpdatedAt,
    Guid UserId,
    string UserNickname,
    Guid ArticleId
);