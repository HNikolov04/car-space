using CarSpace.Data.Models.Entities.CarService;
using CarSpace.Data.Repositories.Interfaces;
using CarSpace.Services.Common.Messages;
using CarSpace.Services.Core.Contracts.CarService.CarServiceCategory.Requests;
using CarSpace.Services.Core.Exceptions;
using CarSpace.Services.Core.Services;
using CarSpace.Services.Core.Services.Interfaces;
using Moq;

namespace CarSpace.Services.Tests;

[TestFixture]
public class CarServiceCategoryServiceTests
{
    private Mock<ICarServiceCategoryRepository> _carServiceCategoryRepositoryMock;
    private ICarServiceCategoryService _carServiceCategoryService;

    [SetUp]
    public void Setup()
    {
        _carServiceCategoryRepositoryMock = new Mock<ICarServiceCategoryRepository>();
        _carServiceCategoryService = new CarServiceCategoryService(_carServiceCategoryRepositoryMock.Object);
    }

    #region GetAllAsync Tests

    [Test]
    public async Task GetAllAsync_ShouldReturnAllCategories_WhenCategoriesExist()
    {
        // Arrange
        var categories = new List<CarServiceCategory>
        {
            new CarServiceCategory { Id = 1, Name = "Oil Change" },
            new CarServiceCategory { Id = 2, Name = "Tire Service" },
            new CarServiceCategory { Id = 3, Name = "Brake Repair" }
        };

        _carServiceCategoryRepositoryMock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(categories);

        // Act
        var result = await _carServiceCategoryService.GetAllAsync();

        // Assert
        Assert.AreEqual(3, result.Count);
        Assert.AreEqual("Oil Change", result[0].Name);
        Assert.AreEqual("Tire Service", result[1].Name);
        Assert.AreEqual("Brake Repair", result[2].Name);
        Assert.AreEqual(1, result[0].Id);
        Assert.AreEqual(2, result[1].Id);
        Assert.AreEqual(3, result[2].Id);
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoCategories()
    {
        // Arrange
        var categories = new List<CarServiceCategory>();

        _carServiceCategoryRepositoryMock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(categories);

        // Act
        var result = await _carServiceCategoryService.GetAllAsync();

        // Assert
        Assert.AreEqual(0, result.Count);
    }

    #endregion

    #region GetByIdAsync Tests

    [Test]
    public async Task GetByIdAsync_ShouldReturnCategory_WhenCategoryExists()
    {
        // Arrange
        var categoryId = 1;
        var category = new CarServiceCategory { Id = categoryId, Name = "Oil Change" };

        _carServiceCategoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryId))
            .ReturnsAsync(category);

        // Act
        var result = await _carServiceCategoryService.GetByIdAsync(categoryId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(categoryId, result.Id);
        Assert.AreEqual("Oil Change", result.Name);
    }

    [Test]
    public void GetByIdAsync_ShouldThrowNotFoundException_WhenCategoryNotExists()
    {
        // Arrange
        var categoryId = 999;

        _carServiceCategoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryId))
            .ReturnsAsync((CarServiceCategory)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<NotFoundException>(() => 
            _carServiceCategoryService.GetByIdAsync(categoryId));
        Assert.AreEqual(ExceptionMessages.CategoryNotFound, ex.Message);
    }

    #endregion

    #region CreateCarServiceCategoryAsync Tests

    [Test]
    public async Task CreateCarServiceCategoryAsync_ShouldCreateCategory_WhenValidRequest()
    {
        // Arrange
        var request = new CreateCarServiceCategoryRequest("Engine Repair");

        _carServiceCategoryRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<CarServiceCategory>()))
            .Returns(Task.CompletedTask);

        // Act
        await _carServiceCategoryService.CreateCarServiceCategoryAsync(request);

        // Assert
        _carServiceCategoryRepositoryMock.Verify(repo => repo.AddAsync(It.Is<CarServiceCategory>(
            c => c.Name == "Engine Repair")), Times.Once);
    }

    [Test]
    public void CreateCarServiceCategoryAsync_ShouldThrowValidationException_WhenNameEmpty()
    {
        // Arrange
        var request = new CreateCarServiceCategoryRequest("");

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationAppException>(() => 
            _carServiceCategoryService.CreateCarServiceCategoryAsync(request));
        Assert.AreEqual(ExceptionMessages.InvalidCategoryName, ex.Message);
    }

    [Test]
    public void CreateCarServiceCategoryAsync_ShouldThrowValidationException_WhenNameWhitespace()
    {
        // Arrange
        var request = new CreateCarServiceCategoryRequest("   ");

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationAppException>(() => 
            _carServiceCategoryService.CreateCarServiceCategoryAsync(request));
        Assert.AreEqual(ExceptionMessages.InvalidCategoryName, ex.Message);
    }

    [Test]
    public void CreateCarServiceCategoryAsync_ShouldThrowValidationException_WhenNameNull()
    {
        // Arrange
        var request = new CreateCarServiceCategoryRequest(null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationAppException>(() => 
            _carServiceCategoryService.CreateCarServiceCategoryAsync(request));
        Assert.AreEqual(ExceptionMessages.InvalidCategoryName, ex.Message);
    }

    #endregion

    #region UpdateCarServiceCategoryAsync Tests

    [Test]
    public async Task UpdateCarServiceCategoryAsync_ShouldUpdateCategory_WhenValidRequest()
    {
        // Arrange
        var categoryId = 1;
        var request = new UpdateCarServiceCategoryRequest(categoryId, "Updated Oil Change");
        var existingCategory = new CarServiceCategory { Id = categoryId, Name = "Oil Change" };

        _carServiceCategoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryId))
            .ReturnsAsync(existingCategory);

        _carServiceCategoryRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<CarServiceCategory>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _carServiceCategoryService.UpdateCarServiceCategoryAsync(request);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual("Updated Oil Change", existingCategory.Name);

        _carServiceCategoryRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<CarServiceCategory>(
            c => c.Name == "Updated Oil Change" && c.Id == categoryId)), Times.Once);
    }

    [Test]
    public void UpdateCarServiceCategoryAsync_ShouldThrowNotFoundException_WhenCategoryNotExists()
    {
        // Arrange
        var categoryId = 999;
        var request = new UpdateCarServiceCategoryRequest(categoryId, "Updated Category");

        _carServiceCategoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryId))
            .ReturnsAsync((CarServiceCategory)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<NotFoundException>(() => 
            _carServiceCategoryService.UpdateCarServiceCategoryAsync(request));
        Assert.AreEqual(ExceptionMessages.CategoryNotFound, ex.Message);
    }

    [Test]
    public void UpdateCarServiceCategoryAsync_ShouldThrowValidationException_WhenNameEmpty()
    {
        // Arrange
        var request = new UpdateCarServiceCategoryRequest(1, "");

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationAppException>(() => 
            _carServiceCategoryService.UpdateCarServiceCategoryAsync(request));
        Assert.AreEqual(ExceptionMessages.InvalidCategoryName, ex.Message);
    }

    [Test]
    public void UpdateCarServiceCategoryAsync_ShouldThrowValidationException_WhenNameWhitespace()
    {
        // Arrange
        var request = new UpdateCarServiceCategoryRequest(1, "   ");

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationAppException>(() => 
            _carServiceCategoryService.UpdateCarServiceCategoryAsync(request));
        Assert.AreEqual(ExceptionMessages.InvalidCategoryName, ex.Message);
    }

    #endregion

    #region DeleteCarServiceCategoryAsync Tests

    [Test]
    public async Task DeleteCarServiceCategoryAsync_ShouldDeleteCategory_WhenCategoryExists()
    {
        // Arrange
        var categoryId = 1;
        var existingCategory = new CarServiceCategory { Id = categoryId, Name = "Oil Change" };

        _carServiceCategoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryId))
            .ReturnsAsync(existingCategory);

        _carServiceCategoryRepositoryMock.Setup(repo => repo.DeleteAsync(existingCategory))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _carServiceCategoryService.DeleteCarServiceCategoryAsync(categoryId);

        // Assert
        Assert.IsTrue(result);

        _carServiceCategoryRepositoryMock.Verify(repo => repo.DeleteAsync(existingCategory), Times.Once);
    }

    [Test]
    public void DeleteCarServiceCategoryAsync_ShouldThrowNotFoundException_WhenCategoryNotExists()
    {
        // Arrange
        var categoryId = 999;

        _carServiceCategoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryId))
            .ReturnsAsync((CarServiceCategory)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<NotFoundException>(() => 
            _carServiceCategoryService.DeleteCarServiceCategoryAsync(categoryId));
        Assert.AreEqual(ExceptionMessages.CategoryNotFound, ex.Message);
    }

    #endregion
}
