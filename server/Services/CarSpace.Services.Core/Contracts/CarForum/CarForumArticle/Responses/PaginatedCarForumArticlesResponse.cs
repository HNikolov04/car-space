namespace CarSpace.Services.Core.Contracts.CarForum.CarForumArticle.Responses;

public sealed record PaginatedCarForumArticlesResponse(
    IEnumerable<GetAllCarForumArticlesResponse> Items,
    int CurrentPage,
    int TotalPages,
    int TotalCount
);