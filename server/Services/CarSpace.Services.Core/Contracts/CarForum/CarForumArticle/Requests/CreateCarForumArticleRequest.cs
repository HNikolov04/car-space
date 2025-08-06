namespace CarSpace.Services.Core.Contracts.CarForum.CarForumArticle.Requests;

public sealed record CreateCarForumArticleRequest(
    string Title,
    string Description,
    int BrandId
);
