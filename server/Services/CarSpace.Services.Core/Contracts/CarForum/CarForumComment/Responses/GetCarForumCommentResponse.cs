namespace CarSpace.Services.Core.Contracts.CarForum.CarForumComment.Responses;

public sealed record GetCarForumCommentResponse(
    Guid Id,
    string Content,
    DateTime CreatedAt,
    string UserNickname,
    Guid UserId
);