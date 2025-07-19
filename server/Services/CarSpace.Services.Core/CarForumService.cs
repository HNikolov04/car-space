using CarSpace.Data.Models.CarForum;
using CarSpace.Data.Models.Entities.CarForum;
using CarSpace.Data.Repositories.Interfaces;
using CarSpace.Services.Core.Contracts.CarForum.Requests;
using CarSpace.Services.Core.Contracts.CarForum.Responses;
using CarSpace.Services.Core.Interfaces;

namespace CarSpace.Services.Core;

public class CarForumService : ICarForumService
{
    private readonly ICarForumRepository _carForumRepository;

    public CarForumService(ICarForumRepository carForumRepository)
    {
        _carForumRepository = carForumRepository;
    }

    public async Task<IEnumerable<GetCarForumArticleResponse>> GetAllArticlesAsync()
    {
        var articles = await _carForumRepository.GetAllArticlesAsync();

        return articles.Select(a => new GetCarForumArticleResponse(
            a.Id, a.Title, a.Description, a.Brand, a.CreatedAt, a.UserId));
    }

    public async Task<GetCarForumArticleResponse?> GetArticleByIdAsync(Guid id)
    {
        var a = await _carForumRepository.GetArticleByIdAsync(id);

        return a is null ? null : new GetCarForumArticleResponse(
            a.Id, a.Title, a.Description, a.Brand, a.CreatedAt, a.UserId);
    }

    public async Task<CreateCarForumArticleResponse> AddArticleAsync(CreateCarForumArticleRequest request)
    {
        var entity = new CarForumArticle
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Brand = request.Brand,
            UserId = request.UserId,
            CreatedAt = DateTime.UtcNow
        };

        await _carForumRepository.AddArticleAsync(entity);

        return new CreateCarForumArticleResponse(
            entity.Id, entity.Title, entity.Description, entity.Brand, entity.CreatedAt, entity.UserId);
    }

    public async Task DeleteArticleAsync(Guid id)
    {
        await _carForumRepository.DeleteArticleAsync(id);
    }

    public async Task<IEnumerable<GetAllCarForumCommentsForArticleResponse>> GetCommentsForArticleAsync(Guid articleId)
    {
        var comments = await _carForumRepository.GetCommentsForArticleAsync(articleId);

        return comments.Select(c => new GetAllCarForumCommentsForArticleResponse(
            c.Id, c.Content, c.CreatedAt, c.UserId, c.CarForumArticleId));
    }

    public async Task<GetCarForumCommentResponse?> GetCommentByIdAsync(Guid commentId)
    {
        var c = await _carForumRepository.GetCommentByIdAsync(commentId);

        return c is null ? null : new GetCarForumCommentResponse(
            c.Id, c.Content, c.CreatedAt, c.UserId, c.CarForumArticleId);
    }

    public async Task<CreateCarForumCommentResponse> AddCommentAsync(CreateCarForumCommentRequest request)
    {
        var entity = new CarForumComment
        {
            Id = Guid.NewGuid(),
            Content = request.Content,
            CreatedAt = DateTime.UtcNow,
            UserId = request.UserId,
            CarForumArticleId = request.CarForumArticleId
        };

        await _carForumRepository.AddCommentAsync(entity);

        return new CreateCarForumCommentResponse(
            entity.Id, entity.Content, entity.CreatedAt, entity.UserId, entity.CarForumArticleId);
    }

    public async Task DeleteCommentAsync(Guid id)
    {
        await _carForumRepository.DeleteCommentAsync(id);
    }
}