using CarSpace.Data.Models.Entities.CarForum;
using CarSpace.Data.Repositories.Interfaces;
using CarSpace.Services.Common.Messages;
using CarSpace.Services.Core.Contracts.CarForum.CarForumBrand.Requests;
using CarSpace.Services.Core.Exceptions;
using CarSpace.Services.Core.Services;
using CarSpace.Services.Core.Services.Interfaces;
using Moq;

namespace CarSpace.Services.Tests;

[TestFixture]
public class CarForumBrandServiceTests
{
    private Mock<ICarForumBrandRepository> _carForumBrandRepositoryMock;
    private ICarForumBrandService _carForumBrandService;

    [SetUp]
    public void Setup()
    {
        _carForumBrandRepositoryMock = new Mock<ICarForumBrandRepository>();
        _carForumBrandService = new CarForumBrandService(_carForumBrandRepositoryMock.Object);
    }

    #region GetAllAsync Tests

    [Test]
    public async Task GetAllAsync_ShouldReturnAllBrands_WhenBrandsExist()
    {
        // Arrange
        var brands = new List<CarForumBrand>
        {
            new CarForumBrand { Id = 1, Name = "Toyota" },
            new CarForumBrand { Id = 2, Name = "Honda" },
            new CarForumBrand { Id = 3, Name = "BMW" }
        };

        _carForumBrandRepositoryMock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(brands);

        // Act
        var result = await _carForumBrandService.GetAllAsync();

        // Assert
        var brandList = result.ToList();
        Assert.AreEqual(3, brandList.Count);
        Assert.AreEqual("Toyota", brandList[0].Name);
        Assert.AreEqual("Honda", brandList[1].Name);
        Assert.AreEqual("BMW", brandList[2].Name);
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoBrands()
    {
        // Arrange
        var brands = new List<CarForumBrand>();

        _carForumBrandRepositoryMock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(brands);

        // Act
        var result = await _carForumBrandService.GetAllAsync();

        // Assert
        Assert.AreEqual(0, result.Count());
    }

    #endregion

    #region GetByIdAsync Tests

    [Test]
    public async Task GetByIdAsync_ShouldReturnBrand_WhenBrandExists()
    {
        // Arrange
        var brandId = 1;
        var brand = new CarForumBrand { Id = brandId, Name = "Toyota" };

        _carForumBrandRepositoryMock.Setup(repo => repo.GetByIdAsync(brandId))
            .ReturnsAsync(brand);

        // Act
        var result = await _carForumBrandService.GetByIdAsync(brandId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(brandId, result.Id);
        Assert.AreEqual("Toyota", result.Name);
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnNull_WhenBrandNotExists()
    {
        // Arrange
        var brandId = 999;

        _carForumBrandRepositoryMock.Setup(repo => repo.GetByIdAsync(brandId))
            .ReturnsAsync((CarForumBrand)null);

        // Act
        var result = await _carForumBrandService.GetByIdAsync(brandId);

        // Assert
        Assert.IsNull(result);
    }

    #endregion

    #region CreateAsync Tests

    [Test]
    public async Task CreateAsync_ShouldCreateBrand_WhenValidRequest()
    {
        // Arrange
        var request = new CreateCarForumBrandRequest("Tesla");
        var createdBrand = new CarForumBrand { Id = 1, Name = "Tesla" };

        _carForumBrandRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<CarForumBrand>()))
            .Callback<CarForumBrand>(brand => brand.Id = 1)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _carForumBrandService.CreateAsync(request);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Tesla", result.Name);

        _carForumBrandRepositoryMock.Verify(repo => repo.CreateAsync(It.Is<CarForumBrand>(
            b => b.Name == "Tesla")), Times.Once);
    }

    [Test]
    public void CreateAsync_ShouldThrowValidationException_WhenNameEmpty()
    {
        // Arrange
        var request = new CreateCarForumBrandRequest("");

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationAppException>(() => 
            _carForumBrandService.CreateAsync(request));
        Assert.AreEqual(ExceptionMessages.InvalidBrandData, ex.Message);
    }

    [Test]
    public void CreateAsync_ShouldThrowValidationException_WhenNameWhitespace()
    {
        // Arrange
        var request = new CreateCarForumBrandRequest("   ");

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationAppException>(() => 
            _carForumBrandService.CreateAsync(request));
        Assert.AreEqual(ExceptionMessages.InvalidBrandData, ex.Message);
    }

    #endregion

    #region UpdateAsync Tests

    [Test]
    public async Task UpdateAsync_ShouldUpdateBrand_WhenValidRequest()
    {
        // Arrange
        var brandId = 1;
        var request = new UpdateCarForumBrandRequest(brandId, "Updated Toyota");
        var existingBrand = new CarForumBrand { Id = brandId, Name = "Toyota" };

        _carForumBrandRepositoryMock.Setup(repo => repo.GetByIdAsync(brandId))
            .ReturnsAsync(existingBrand);

        _carForumBrandRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<CarForumBrand>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _carForumBrandService.UpdateAsync(request);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual("Updated Toyota", existingBrand.Name);

        _carForumBrandRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<CarForumBrand>(
            b => b.Name == "Updated Toyota" && b.Id == brandId)), Times.Once);
    }

    [Test]
    public void UpdateAsync_ShouldThrowNotFoundException_WhenBrandNotExists()
    {
        // Arrange
        var brandId = 999;
        var request = new UpdateCarForumBrandRequest(brandId, "Updated Brand");

        _carForumBrandRepositoryMock.Setup(repo => repo.GetByIdAsync(brandId))
            .ReturnsAsync((CarForumBrand)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<NotFoundException>(() => 
            _carForumBrandService.UpdateAsync(request));
        Assert.AreEqual(ExceptionMessages.BrandNotFound, ex.Message);
    }

    [Test]
    public void UpdateAsync_ShouldThrowValidationException_WhenIdInvalid()
    {
        // Arrange
        var request = new UpdateCarForumBrandRequest(0, "Valid Name");

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationAppException>(() => 
            _carForumBrandService.UpdateAsync(request));
        Assert.AreEqual(ExceptionMessages.InvalidBrandData, ex.Message);
    }

    [Test]
    public void UpdateAsync_ShouldThrowValidationException_WhenNameEmpty()
    {
        // Arrange
        var request = new UpdateCarForumBrandRequest(1, "");

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationAppException>(() => 
            _carForumBrandService.UpdateAsync(request));
        Assert.AreEqual(ExceptionMessages.InvalidBrandData, ex.Message);
    }

    #endregion

    #region DeleteAsync Tests

    [Test]
    public async Task DeleteAsync_ShouldDeleteBrand_WhenBrandExists()
    {
        // Arrange
        var brandId = 1;
        var existingBrand = new CarForumBrand { Id = brandId, Name = "Toyota" };

        _carForumBrandRepositoryMock.Setup(repo => repo.GetByIdAsync(brandId))
            .ReturnsAsync(existingBrand);

        _carForumBrandRepositoryMock.Setup(repo => repo.DeleteAsync(brandId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _carForumBrandService.DeleteAsync(brandId);

        // Assert
        Assert.IsTrue(result);

        _carForumBrandRepositoryMock.Verify(repo => repo.DeleteAsync(brandId), Times.Once);
    }

    [Test]
    public void DeleteAsync_ShouldThrowNotFoundException_WhenBrandNotExists()
    {
        // Arrange
        var brandId = 999;

        _carForumBrandRepositoryMock.Setup(repo => repo.GetByIdAsync(brandId))
            .ReturnsAsync((CarForumBrand)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<NotFoundException>(() => 
            _carForumBrandService.DeleteAsync(brandId));
        Assert.AreEqual(ExceptionMessages.BrandNotFound, ex.Message);
    }

    #endregion
}
