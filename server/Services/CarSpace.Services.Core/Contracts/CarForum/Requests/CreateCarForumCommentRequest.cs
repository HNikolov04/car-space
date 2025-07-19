namespace CarSpace.Services.Core.Contracts.CarForum.Requests;

public sealed record CreateCarForumCommentRequest(
    string Content,
    Guid UserId,
    Guid CarForumArticleId
);
