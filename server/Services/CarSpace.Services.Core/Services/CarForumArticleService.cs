using CarSpace.Data.Models.CarForum;
using CarSpace.Data.Repositories.Interfaces;
using CarSpace.Services.Common.Constants;
using CarSpace.Services.Common.Messages;
using CarSpace.Services.Core.Contracts.CarForum.CarForumArticle.Requests;
using CarSpace.Services.Core.Contracts.CarForum.CarForumArticle.Responses;
using CarSpace.Services.Core.Exceptions;
using CarSpace.Services.Core.Services.Interfaces;

namespace CarSpace.Services.Core.Services;

public class CarForumArticleService : ICarForumArticleService
{
    private readonly ICarForumArticleRepository _carForumArticleRepository;

    public CarForumArticleService(ICarForumArticleRepository carForumArticleRepository)
    {
        _carForumArticleRepository = carForumArticleRepository;
    }

    public async Task<PaginatedCarForumArticlesResponse> GetCarForumArticlesAsync(GetCarForumArticlesRequest request, Guid? userId)
    {
        var (articles, totalCount) = await _carForumArticleRepository.GetFilteredArticlesAsync(
            request.SearchTerm,
            request.BrandId,
            request.SavedOnly,
            request.MyArticlesOnly,
            userId,
            request.Page,
            request.PageSize
        );

        var responses = articles.Select(a => new GetAllCarForumArticlesResponse(
            a.Id,
            a.Title,
            a.Description,
            a.CarBrand?.Name ?? ServiceDefaults.UnknownBrand,
            a.User?.UserName ?? ServiceDefaults.UnknownUser,
            a.UpdatedAt,
            userId.HasValue && a.SavedByUsers.Any(s => s.UserId == userId)
        )).ToList();

        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        return new PaginatedCarForumArticlesResponse(
            Items: responses,
            CurrentPage: request.Page,
            TotalPages: totalPages,
            TotalCount: totalCount
        );
    }

    public async Task<GetCarForumArticleByIdResponse?> GetCarForumArticleByIdAsync(Guid id, Guid? userId)
    {
        var article = await _carForumArticleRepository.GetArticleByIdAsync(id);

        if (article is null)
        {
            return null;
        }

        var isSaved = userId.HasValue && article.SavedByUsers.Any(s => s.UserId == userId.Value);

        return new GetCarForumArticleByIdResponse(
            article.Id,
            article.Title,
            article.Description,
            article.CarBrandId,
            article.CarBrand?.Name ?? ServiceDefaults.UnknownBrand,
            article.UpdatedAt,
            article.User?.UserName ?? ServiceDefaults.UnknownUser,
            article.UserId,
            isSaved
        );
    }

    public async Task<GetCarForumArticleByIdResponse> CreateCarForumArticleAsync(CreateCarForumArticleRequest request, Guid userId)
    {
        ValidateCreateRequest(request);

        var article = new CarForumArticle
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            CarBrandId = request.BrandId,
            UserId = userId,
            UpdatedAt = DateTime.UtcNow
        };

        await _carForumArticleRepository.AddArticleAsync(article);

        var saved = await _carForumArticleRepository.GetArticleByIdAsync(article.Id);

        if (saved is null)
        {
            throw new NotFoundException(ExceptionMessages.ArticleNotFound);
        }

        return new GetCarForumArticleByIdResponse(
            saved.Id,
            saved.Title,
            saved.Description,
            saved.CarBrandId,
            saved.CarBrand?.Name ?? ServiceDefaults.UnknownBrand,
            saved.UpdatedAt,
            saved.User?.UserName ?? ServiceDefaults.UnknownUser,
            saved.UserId,
            false
        );
    }

    public async Task<bool> UpdateCarForumArticleAsync(Guid id, UpdateCarForumArticleRequest request, Guid userId)
    {
        ValidateUpdateRequest(request);

        var article = await _carForumArticleRepository.GetArticleByIdAsync(id);

        if (article is null)
        {
            throw new NotFoundException(ExceptionMessages.ArticleNotFound);
        }

        if (article.UserId != userId)
        {
            throw new UnauthorizedAccessAppException(ExceptionMessages.UnauthorizedListingAccess);
        }

        article.Title = request.Title;
        article.Description = request.Description;
        article.CarBrandId = request.BrandId;
        article.UpdatedAt = DateTime.UtcNow;

        await _carForumArticleRepository.UpdateArticleAsync(article);

        return true;
    }

    public async Task<bool> DeleteCarForumArticleAsync(Guid id, Guid userId)
    {
        var article = await _carForumArticleRepository.GetArticleByIdAsync(id);

        if (article is null)
        {
            throw new NotFoundException(ExceptionMessages.ArticleNotFound);
        }

        if (article.UserId != userId)
        {
            throw new UnauthorizedAccessAppException(ExceptionMessages.UnauthorizedListingAccess);
        }

        await _carForumArticleRepository.DeleteArticleAsync(id);

        return true;
    }

    public async Task<bool> SaveCarForumArticleAsync(Guid articleId, Guid userId)
    {
        return await _carForumArticleRepository.SaveArticleAsync(articleId, userId);
    }

    public async Task<bool> UnsaveCarForumArticleAsync(Guid articleId, Guid userId)
    {
        return await _carForumArticleRepository.UnsaveArticleAsync(articleId, userId);
    }

    private static void ValidateCreateRequest(CreateCarForumArticleRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title) ||
            string.IsNullOrWhiteSpace(request.Description) ||
            request.BrandId <= 0)
        {
            throw new ValidationAppException(ExceptionMessages.InvalidArticleData);
        }
    }

    private static void ValidateUpdateRequest(UpdateCarForumArticleRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title) ||
            string.IsNullOrWhiteSpace(request.Description) ||
            request.BrandId <= 0)
        {
            throw new ValidationAppException(ExceptionMessages.InvalidArticleData);
        }
    }
}
