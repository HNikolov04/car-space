namespace CarSpace.Services.Core.Contracts.CarForum.CarForumComment.Requests;

public sealed record CreateCarForumCommentRequest(
    Guid ArticleId,
    string Content
);
