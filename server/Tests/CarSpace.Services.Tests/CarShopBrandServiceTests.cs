using CarSpace.Data.Models.Entities.CarShop;
using CarSpace.Data.Repositories.Interfaces;
using CarSpace.Services.Common.Messages;
using CarSpace.Services.Core.Contracts.CarShop.CarShopBrand.Requests;
using CarSpace.Services.Core.Contracts.CarShop.CarShopBrand.Response;
using CarSpace.Services.Core.Exceptions;
using CarSpace.Services.Core.Services;
using CarSpace.Services.Core.Services.Interfaces;
using Moq;

namespace CarSpace.Services.Tests;

[TestFixture]
public class CarShopBrandServiceTests
{
    private Mock<ICarShopBrandRepository> _carShopBrandRepositoryMock;
    private ICarShopBrandService _carShopBrandService;

    [SetUp]
    public void Setup()
    {
        _carShopBrandRepositoryMock = new Mock<ICarShopBrandRepository>();
        _carShopBrandService = new CarShopBrandService(_carShopBrandRepositoryMock.Object);
    }

    #region GetAllCarShopBrandsAsync Tests

    [Test]
    public async Task GetAllCarShopBrandsAsync_ShouldReturnAllBrands_WhenBrandsExist()
    {
        // Arrange
        var brands = new List<CarShopBrand>
        {
            new CarShopBrand { Id = 1, Name = "BMW" },
            new CarShopBrand { Id = 2, Name = "Mercedes" },
            new CarShopBrand { Id = 3, Name = "Audi" }
        };

        _carShopBrandRepositoryMock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(brands);

        // Act
        var result = await _carShopBrandService.GetAllCarShopBrandsAsync();
        var resultList = result.ToList();

        // Assert
        Assert.AreEqual(3, resultList.Count);
        Assert.AreEqual("BMW", resultList[0].Name);
        Assert.AreEqual("Mercedes", resultList[1].Name);
        Assert.AreEqual("Audi", resultList[2].Name);
        Assert.AreEqual(1, resultList[0].Id);
        Assert.AreEqual(2, resultList[1].Id);
        Assert.AreEqual(3, resultList[2].Id);
    }

    [Test]
    public async Task GetAllCarShopBrandsAsync_ShouldReturnEmptyEnumerable_WhenNoBrands()
    {
        // Arrange
        var brands = new List<CarShopBrand>();

        _carShopBrandRepositoryMock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(brands);

        // Act
        var result = await _carShopBrandService.GetAllCarShopBrandsAsync();

        // Assert
        Assert.AreEqual(0, result.Count());
    }

    #endregion

    #region GetCarShopBrandByIdAsync Tests

    [Test]
    public async Task GetCarShopBrandByIdAsync_ShouldReturnBrand_WhenBrandExists()
    {
        // Arrange
        var brandId = 1;
        var brand = new CarShopBrand { Id = brandId, Name = "BMW" };

        _carShopBrandRepositoryMock.Setup(repo => repo.GetByIdAsync(brandId))
            .ReturnsAsync(brand);

        // Act
        var result = await _carShopBrandService.GetCarShopBrandByIdAsync(brandId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(brandId, result.Id);
        Assert.AreEqual("BMW", result.Name);
    }

    [Test]
    public async Task GetCarShopBrandByIdAsync_ShouldReturnNull_WhenBrandNotExists()
    {
        // Arrange
        var brandId = 999;

        _carShopBrandRepositoryMock.Setup(repo => repo.GetByIdAsync(brandId))
            .ReturnsAsync((CarShopBrand)null);

        // Act
        var result = await _carShopBrandService.GetCarShopBrandByIdAsync(brandId);

        // Assert
        Assert.IsNull(result);
    }

    #endregion

    #region CreateCarShopBrandAsync Tests

    [Test]
    public async Task CreateCarShopBrandAsync_ShouldCreateBrand_WhenValidRequest()
    {
        // Arrange
        var request = new CreateCarShopBrandRequest("Porsche");

        _carShopBrandRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<CarShopBrand>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _carShopBrandService.CreateCarShopBrandAsync(request);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Porsche", result.Name);

        _carShopBrandRepositoryMock.Verify(repo => repo.CreateAsync(It.Is<CarShopBrand>(
            b => b.Name == "Porsche")), Times.Once);
    }

    [Test]
    public void CreateCarShopBrandAsync_ShouldThrowValidationException_WhenNameEmpty()
    {
        // Arrange
        var request = new CreateCarShopBrandRequest("");

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationAppException>(() => 
            _carShopBrandService.CreateCarShopBrandAsync(request));
        Assert.AreEqual(ExceptionMessages.InvalidBrandData, ex.Message);
    }

    [Test]
    public void CreateCarShopBrandAsync_ShouldThrowValidationException_WhenNameWhitespace()
    {
        // Arrange
        var request = new CreateCarShopBrandRequest("   ");

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationAppException>(() => 
            _carShopBrandService.CreateCarShopBrandAsync(request));
        Assert.AreEqual(ExceptionMessages.InvalidBrandData, ex.Message);
    }

    [Test]
    public void CreateCarShopBrandAsync_ShouldThrowValidationException_WhenNameNull()
    {
        // Arrange
        var request = new CreateCarShopBrandRequest(null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationAppException>(() => 
            _carShopBrandService.CreateCarShopBrandAsync(request));
        Assert.AreEqual(ExceptionMessages.InvalidBrandData, ex.Message);
    }

    #endregion

    #region UpdateCarShopBrandAsync Tests

    [Test]
    public async Task UpdateCarShopBrandAsync_ShouldUpdateBrand_WhenValidRequest()
    {
        // Arrange
        var brandId = 1;
        var request = new UpdateCarShopBrandRequest(brandId, "Updated BMW");
        var existingBrand = new CarShopBrand { Id = brandId, Name = "BMW" };

        _carShopBrandRepositoryMock.Setup(repo => repo.GetByIdAsync(brandId))
            .ReturnsAsync(existingBrand);

        _carShopBrandRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<CarShopBrand>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _carShopBrandService.UpdateCarShopBrandAsync(request);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual("Updated BMW", existingBrand.Name);

        _carShopBrandRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<CarShopBrand>(
            b => b.Name == "Updated BMW" && b.Id == brandId)), Times.Once);
    }

    [Test]
    public void UpdateCarShopBrandAsync_ShouldThrowNotFoundException_WhenBrandNotExists()
    {
        // Arrange
        var brandId = 999;
        var request = new UpdateCarShopBrandRequest(brandId, "Updated Brand");

        _carShopBrandRepositoryMock.Setup(repo => repo.GetByIdAsync(brandId))
            .ReturnsAsync((CarShopBrand)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<NotFoundException>(() => 
            _carShopBrandService.UpdateCarShopBrandAsync(request));
        Assert.AreEqual(ExceptionMessages.BrandNotFound, ex.Message);
    }

    [Test]
    public async Task UpdateCarShopBrandAsync_ShouldUpdateBrand_WhenNameEmpty()
    {
        // Arrange
        var brandId = 1;
        var request = new UpdateCarShopBrandRequest(brandId, "");
        var existingBrand = new CarShopBrand { Id = brandId, Name = "BMW" };

        _carShopBrandRepositoryMock.Setup(repo => repo.GetByIdAsync(brandId))
            .ReturnsAsync(existingBrand);

        _carShopBrandRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<CarShopBrand>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _carShopBrandService.UpdateCarShopBrandAsync(request);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual("", existingBrand.Name);

        _carShopBrandRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<CarShopBrand>(
            b => b.Name == "" && b.Id == brandId)), Times.Once);
    }

    #endregion

    #region DeleteCarShopBrandAsync Tests

    [Test]
    public async Task DeleteCarShopBrandAsync_ShouldDeleteBrand_WhenBrandExists()
    {
        // Arrange
        var brandId = 1;
        var existingBrand = new CarShopBrand { Id = brandId, Name = "BMW" };

        _carShopBrandRepositoryMock.Setup(repo => repo.GetByIdAsync(brandId))
            .ReturnsAsync(existingBrand);

        _carShopBrandRepositoryMock.Setup(repo => repo.DeleteAsync(existingBrand))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _carShopBrandService.DeleteCarShopBrandAsync(brandId);

        // Assert
        Assert.IsTrue(result);

        _carShopBrandRepositoryMock.Verify(repo => repo.DeleteAsync(existingBrand), Times.Once);
    }

    [Test]
    public void DeleteCarShopBrandAsync_ShouldThrowNotFoundException_WhenBrandNotExists()
    {
        // Arrange
        var brandId = 999;

        _carShopBrandRepositoryMock.Setup(repo => repo.GetByIdAsync(brandId))
            .ReturnsAsync((CarShopBrand)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<NotFoundException>(() => 
            _carShopBrandService.DeleteCarShopBrandAsync(brandId));
        Assert.AreEqual(ExceptionMessages.BrandNotFound, ex.Message);
    }

    #endregion
}
