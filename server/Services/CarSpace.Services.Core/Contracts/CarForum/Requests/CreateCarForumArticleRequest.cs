namespace CarSpace.Services.Core.Contracts.CarForum.Requests;

public sealed record CreateCarForumArticleRequest(
    string Title,
    string Description,
    string Brand,
    Guid UserId
);
