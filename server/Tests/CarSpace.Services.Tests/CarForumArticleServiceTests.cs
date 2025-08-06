using CarSpace.Data.Models.CarForum;
using CarSpace.Data.Models.Entities.CarForum;
using CarSpace.Data.Models.Entities.User;
using CarSpace.Data.Repositories.Interfaces;
using CarSpace.Services.Common.Messages;
using CarSpace.Services.Core.Contracts.CarForum.CarForumArticle.Requests;
using CarSpace.Services.Core.Exceptions;
using CarSpace.Services.Core.Services;
using CarSpace.Services.Core.Services.Interfaces;
using Moq;

namespace CarSpace.Services.Tests;

[TestFixture]
public class CarForumArticleServiceTests
{
    private Mock<ICarForumArticleRepository> _carForumArticleRepositoryMock;
    private ICarForumArticleService _carForumArticleService;

    [SetUp]
    public void Setup()
    {
        _carForumArticleRepositoryMock = new Mock<ICarForumArticleRepository>();
        _carForumArticleService = new CarForumArticleService(_carForumArticleRepositoryMock.Object);
    }

    #region GetCarForumArticlesAsync Tests

    [Test]
    public async Task GetCarForumArticlesAsync_ShouldReturnPaginatedResponse_WhenArticlesExist()
    {
        // Arrange
        var request = new GetCarForumArticlesRequest(null, 1, 10);
        var userId = Guid.NewGuid();
        
        var articles = new List<CarForumArticle>
        {
            new CarForumArticle
            {
                Id = Guid.NewGuid(),
                Title = "Test Article 1",
                Description = "Test Description 1",
                UpdatedAt = DateTime.UtcNow,
                CarBrand = new CarForumBrand { Name = "Toyota" },
                User = new ApplicationUser(Guid.NewGuid(), "test@test.com", "testuser", "img.png"),
                SavedByUsers = new List<UserSavedCarForumArticle>()
            },
            new CarForumArticle
            {
                Id = Guid.NewGuid(),
                Title = "Test Article 2",
                Description = "Test Description 2",
                UpdatedAt = DateTime.UtcNow,
                CarBrand = new CarForumBrand { Name = "Honda" },
                User = new ApplicationUser(Guid.NewGuid(), "test2@test.com", "testuser2", "img2.png"),
                SavedByUsers = new List<UserSavedCarForumArticle>()
            }
        };

        _carForumArticleRepositoryMock.Setup(repo => repo.GetFilteredArticlesAsync(
                null, null, false, false, userId, 1, 10))
            .ReturnsAsync((articles, 2));

        // Act
        var result = await _carForumArticleService.GetCarForumArticlesAsync(request, userId);

        // Assert
        Assert.AreEqual(2, result.Items.Count());
        Assert.AreEqual(1, result.CurrentPage);
        Assert.AreEqual(1, result.TotalPages);
        Assert.AreEqual(2, result.TotalCount);
        Assert.AreEqual("Test Article 1", result.Items.First().Title);
        Assert.AreEqual("Toyota", result.Items.First().BrandName);
    }

    [Test]
    public async Task GetCarForumArticlesAsync_ShouldHandleNullBrandAndUser_WhenDataMissing()
    {
        // Arrange
        var request = new GetCarForumArticlesRequest(null, 1, 10);
        var userId = Guid.NewGuid();
        
        var articles = new List<CarForumArticle>
        {
            new CarForumArticle
            {
                Id = Guid.NewGuid(),
                Title = "Test Article",
                Description = "Test Description",
                UpdatedAt = DateTime.UtcNow,
                CarBrand = null,
                User = null,
                SavedByUsers = new List<UserSavedCarForumArticle>()
            }
        };

        _carForumArticleRepositoryMock.Setup(repo => repo.GetFilteredArticlesAsync(
                null, null, false, false, userId, 1, 10))
            .ReturnsAsync((articles, 1));

        // Act
        var result = await _carForumArticleService.GetCarForumArticlesAsync(request, userId);

        // Assert
        Assert.AreEqual("Unknown", result.Items.First().BrandName);
        Assert.AreEqual("Unknown", result.Items.First().UserNickname);
    }

    #endregion

    #region GetCarForumArticleByIdAsync Tests

    [Test]
    public async Task GetCarForumArticleByIdAsync_ShouldReturnArticle_WhenExists()
    {
        // Arrange
        var articleId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var article = new CarForumArticle
        {
            Id = articleId,
            Title = "Test Article",
            Description = "Test Description",
            CarBrandId = 1,
            UserId = userId,
            UpdatedAt = DateTime.UtcNow,
            CarBrand = new CarForumBrand { Name = "Toyota" },
            User = new ApplicationUser(userId, "test@test.com", "testuser", "img.png"),
            SavedByUsers = new List<UserSavedCarForumArticle>()
        };

        _carForumArticleRepositoryMock.Setup(repo => repo.GetArticleByIdAsync(articleId))
            .ReturnsAsync(article);

        // Act
        var result = await _carForumArticleService.GetCarForumArticleByIdAsync(articleId, userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(articleId, result.Id);
        Assert.AreEqual("Test Article", result.Title);
        Assert.AreEqual("Toyota", result.BrandName);
        Assert.IsFalse(result.IsSavedByCurrentUser);
    }

    [Test]
    public async Task GetCarForumArticleByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        var articleId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _carForumArticleRepositoryMock.Setup(repo => repo.GetArticleByIdAsync(articleId))
            .ReturnsAsync((CarForumArticle)null);

        // Act
        var result = await _carForumArticleService.GetCarForumArticleByIdAsync(articleId, userId);

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public async Task GetCarForumArticleByIdAsync_ShouldReturnSavedTrue_WhenUserSavedArticle()
    {
        // Arrange
        var articleId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var article = new CarForumArticle
        {
            Id = articleId,
            Title = "Test Article",
            Description = "Test Description",
            CarBrandId = 1,
            UserId = userId,
            UpdatedAt = DateTime.UtcNow,
            CarBrand = new CarForumBrand { Name = "Toyota" },
            User = new ApplicationUser(userId, "test@test.com", "testuser", "img.png"),
            SavedByUsers = new List<UserSavedCarForumArticle>
            {
                new UserSavedCarForumArticle { UserId = userId, CarForumArticleId = articleId }
            }
        };

        _carForumArticleRepositoryMock.Setup(repo => repo.GetArticleByIdAsync(articleId))
            .ReturnsAsync(article);

        // Act
        var result = await _carForumArticleService.GetCarForumArticleByIdAsync(articleId, userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.IsSavedByCurrentUser);
    }

    #endregion

    #region CreateCarForumArticleAsync Tests

    [Test]
    public async Task CreateCarForumArticleAsync_ShouldCreateArticle_WhenValidRequest()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new CreateCarForumArticleRequest("Test Title", "Test Description", 1);
        
        var createdArticle = new CarForumArticle
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            CarBrandId = request.BrandId,
            UserId = userId,
            UpdatedAt = DateTime.UtcNow,
            CarBrand = new CarForumBrand { Name = "Toyota" },
            User = new ApplicationUser(userId, "test@test.com", "testuser", "img.png"),
            SavedByUsers = new List<UserSavedCarForumArticle>()
        };

        _carForumArticleRepositoryMock.Setup(repo => repo.AddArticleAsync(It.IsAny<CarForumArticle>()))
            .Returns(Task.CompletedTask);

        _carForumArticleRepositoryMock.Setup(repo => repo.GetArticleByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(createdArticle);

        // Act
        var result = await _carForumArticleService.CreateCarForumArticleAsync(request, userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Test Title", result.Title);
        Assert.AreEqual("Test Description", result.Description);
        Assert.AreEqual(1, result.BrandId);
        Assert.AreEqual(userId, result.UserId);
        Assert.IsFalse(result.IsSavedByCurrentUser);

        _carForumArticleRepositoryMock.Verify(repo => repo.AddArticleAsync(It.IsAny<CarForumArticle>()), Times.Once);
    }

    [Test]
    public void CreateCarForumArticleAsync_ShouldThrowValidationException_WhenTitleEmpty()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new CreateCarForumArticleRequest("", "Test Description", 1);

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationAppException>(() => 
            _carForumArticleService.CreateCarForumArticleAsync(request, userId));
        Assert.AreEqual(ExceptionMessages.InvalidArticleData, ex.Message);
    }

    [Test]
    public void CreateCarForumArticleAsync_ShouldThrowValidationException_WhenDescriptionEmpty()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new CreateCarForumArticleRequest("Test Title", "", 1);

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationAppException>(() => 
            _carForumArticleService.CreateCarForumArticleAsync(request, userId));
        Assert.AreEqual(ExceptionMessages.InvalidArticleData, ex.Message);
    }

    [Test]
    public void CreateCarForumArticleAsync_ShouldThrowValidationException_WhenBrandIdInvalid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new CreateCarForumArticleRequest("Test Title", "Test Description", 0);

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationAppException>(() => 
            _carForumArticleService.CreateCarForumArticleAsync(request, userId));
        Assert.AreEqual(ExceptionMessages.InvalidArticleData, ex.Message);
    }

    [Test]
    public void CreateCarForumArticleAsync_ShouldThrowNotFoundException_WhenArticleNotFoundAfterCreation()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new CreateCarForumArticleRequest("Test Title", "Test Description", 1);

        _carForumArticleRepositoryMock.Setup(repo => repo.AddArticleAsync(It.IsAny<CarForumArticle>()))
            .Returns(Task.CompletedTask);

        _carForumArticleRepositoryMock.Setup(repo => repo.GetArticleByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((CarForumArticle)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<NotFoundException>(() => 
            _carForumArticleService.CreateCarForumArticleAsync(request, userId));
        Assert.AreEqual(ExceptionMessages.ArticleNotFound, ex.Message);
    }

    #endregion

    #region UpdateCarForumArticleAsync Tests

    [Test]
    public async Task UpdateCarForumArticleAsync_ShouldUpdateArticle_WhenValidRequest()
    {
        // Arrange
        var articleId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var request = new UpdateCarForumArticleRequest(articleId, "Updated Title", "Updated Description", 2);
        
        var existingArticle = new CarForumArticle
        {
            Id = articleId,
            Title = "Original Title",
            Description = "Original Description",
            CarBrandId = 1,
            UserId = userId,
            UpdatedAt = DateTime.UtcNow.AddDays(-1)
        };

        _carForumArticleRepositoryMock.Setup(repo => repo.GetArticleByIdAsync(articleId))
            .ReturnsAsync(existingArticle);

        _carForumArticleRepositoryMock.Setup(repo => repo.UpdateArticleAsync(It.IsAny<CarForumArticle>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _carForumArticleService.UpdateCarForumArticleAsync(articleId, request, userId);

        // Assert
        Assert.IsTrue(result);
        _carForumArticleRepositoryMock.Verify(repo => repo.UpdateArticleAsync(It.Is<CarForumArticle>(
            a => a.Title == "Updated Title" && 
                 a.Description == "Updated Description" && 
                 a.CarBrandId == 2)), Times.Once);
    }

    [Test]
    public void UpdateCarForumArticleAsync_ShouldThrowNotFoundException_WhenArticleNotExists()
    {
        // Arrange
        var articleId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var request = new UpdateCarForumArticleRequest(articleId, "Updated Title", "Updated Description", 2);

        _carForumArticleRepositoryMock.Setup(repo => repo.GetArticleByIdAsync(articleId))
            .ReturnsAsync((CarForumArticle)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<NotFoundException>(() => 
            _carForumArticleService.UpdateCarForumArticleAsync(articleId, request, userId));
        Assert.AreEqual(ExceptionMessages.ArticleNotFound, ex.Message);
    }

    [Test]
    public void UpdateCarForumArticleAsync_ShouldThrowUnauthorized_WhenUserNotOwner()
    {
        // Arrange
        var articleId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var differentUserId = Guid.NewGuid();
        var request = new UpdateCarForumArticleRequest(articleId, "Updated Title", "Updated Description", 2);
        
        var existingArticle = new CarForumArticle
        {
            Id = articleId,
            Title = "Original Title",
            Description = "Original Description",
            CarBrandId = 1,
            UserId = differentUserId,
            UpdatedAt = DateTime.UtcNow.AddDays(-1)
        };

        _carForumArticleRepositoryMock.Setup(repo => repo.GetArticleByIdAsync(articleId))
            .ReturnsAsync(existingArticle);

        // Act & Assert
        var ex = Assert.ThrowsAsync<UnauthorizedAccessAppException>(() => 
            _carForumArticleService.UpdateCarForumArticleAsync(articleId, request, userId));
        Assert.AreEqual(ExceptionMessages.UnauthorizedListingAccess, ex.Message);
    }

    [Test]
    public void UpdateCarForumArticleAsync_ShouldThrowValidationException_WhenRequestInvalid()
    {
        // Arrange
        var articleId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var request = new UpdateCarForumArticleRequest(articleId, "", "Updated Description", 2);

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationAppException>(() => 
            _carForumArticleService.UpdateCarForumArticleAsync(articleId, request, userId));
        Assert.AreEqual(ExceptionMessages.InvalidArticleData, ex.Message);
    }

    #endregion

    #region DeleteCarForumArticleAsync Tests

    [Test]
    public async Task DeleteCarForumArticleAsync_ShouldDeleteArticle_WhenValidRequest()
    {
        // Arrange
        var articleId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        
        var existingArticle = new CarForumArticle
        {
            Id = articleId,
            Title = "Test Article",
            Description = "Test Description",
            CarBrandId = 1,
            UserId = userId,
            UpdatedAt = DateTime.UtcNow
        };

        _carForumArticleRepositoryMock.Setup(repo => repo.GetArticleByIdAsync(articleId))
            .ReturnsAsync(existingArticle);

        _carForumArticleRepositoryMock.Setup(repo => repo.DeleteArticleAsync(articleId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _carForumArticleService.DeleteCarForumArticleAsync(articleId, userId);

        // Assert
        Assert.IsTrue(result);
        _carForumArticleRepositoryMock.Verify(repo => repo.DeleteArticleAsync(articleId), Times.Once);
    }

    [Test]
    public void DeleteCarForumArticleAsync_ShouldThrowNotFoundException_WhenArticleNotExists()
    {
        // Arrange
        var articleId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _carForumArticleRepositoryMock.Setup(repo => repo.GetArticleByIdAsync(articleId))
            .ReturnsAsync((CarForumArticle)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<NotFoundException>(() => 
            _carForumArticleService.DeleteCarForumArticleAsync(articleId, userId));
        Assert.AreEqual(ExceptionMessages.ArticleNotFound, ex.Message);
    }

    [Test]
    public void DeleteCarForumArticleAsync_ShouldThrowUnauthorized_WhenUserNotOwner()
    {
        // Arrange
        var articleId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var differentUserId = Guid.NewGuid();
        
        var existingArticle = new CarForumArticle
        {
            Id = articleId,
            Title = "Test Article",
            Description = "Test Description",
            CarBrandId = 1,
            UserId = differentUserId,
            UpdatedAt = DateTime.UtcNow
        };

        _carForumArticleRepositoryMock.Setup(repo => repo.GetArticleByIdAsync(articleId))
            .ReturnsAsync(existingArticle);

        // Act & Assert
        var ex = Assert.ThrowsAsync<UnauthorizedAccessAppException>(() => 
            _carForumArticleService.DeleteCarForumArticleAsync(articleId, userId));
        Assert.AreEqual(ExceptionMessages.UnauthorizedListingAccess, ex.Message);
    }

    #endregion

    #region Save/Unsave Tests

    [Test]
    public async Task SaveCarForumArticleAsync_ShouldCallRepository_WithCorrectParameters()
    {
        // Arrange
        var articleId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _carForumArticleRepositoryMock.Setup(repo => repo.SaveArticleAsync(articleId, userId))
            .ReturnsAsync(true);

        // Act
        var result = await _carForumArticleService.SaveCarForumArticleAsync(articleId, userId);

        // Assert
        Assert.IsTrue(result);
        _carForumArticleRepositoryMock.Verify(repo => repo.SaveArticleAsync(articleId, userId), Times.Once);
    }

    [Test]
    public async Task UnsaveCarForumArticleAsync_ShouldCallRepository_WithCorrectParameters()
    {
        // Arrange
        var articleId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _carForumArticleRepositoryMock.Setup(repo => repo.UnsaveArticleAsync(articleId, userId))
            .ReturnsAsync(true);

        // Act
        var result = await _carForumArticleService.UnsaveCarForumArticleAsync(articleId, userId);

        // Assert
        Assert.IsTrue(result);
        _carForumArticleRepositoryMock.Verify(repo => repo.UnsaveArticleAsync(articleId, userId), Times.Once);
    }

    #endregion
}
