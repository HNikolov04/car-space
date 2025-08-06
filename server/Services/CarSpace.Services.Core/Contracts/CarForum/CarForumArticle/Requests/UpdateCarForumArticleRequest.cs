namespace CarSpace.Services.Core.Contracts.CarForum.CarForumArticle.Requests;

public sealed record UpdateCarForumArticleRequest(
    Guid Id,
    string Title,
    string Description,
    int BrandId
);
