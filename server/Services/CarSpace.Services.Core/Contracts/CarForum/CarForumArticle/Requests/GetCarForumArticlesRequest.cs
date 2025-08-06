namespace CarSpace.Services.Core.Contracts.CarForum.CarForumArticle.Requests;

public sealed record GetCarForumArticlesRequest(
    int? BrandId,
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool SavedOnly = false,
    bool MyArticlesOnly = false
);
