namespace CarSpace.Services.Core.Contracts.CarForum.CarForumComment.Requests;

public sealed record GetCarForumCommentsRequest(
    Guid ArticleId,
    int Page = 1,
    int PageSize = 10
);
