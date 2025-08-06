namespace CarSpace.Services.Core.Contracts.CarForum.CarForumComment.Responses;

public sealed record PaginatedCarForumCommentsResponse(
    IEnumerable<GetAllCarForumCommentsForArticlesResponse> Items,
    int CurrentPage,
    int TotalPages,
    int TotalCount
);
