namespace CarSpace.Services.Core.Contracts.CarForum.Responses;

public sealed record CreateCarForumCommentResponse(
    Guid Id,
    string Content,
    DateTime CreatedAt,
    Guid UserId,
    Guid CarForumArticleId
);
