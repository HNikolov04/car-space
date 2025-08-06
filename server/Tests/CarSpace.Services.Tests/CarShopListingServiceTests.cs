using CarSpace.Data.Models.Entities.CarShop;
using CarSpace.Data.Models.Entities.User;
using CarSpace.Data.Repositories.Interfaces;
using CarSpace.Services.Common.Messages;
using CarSpace.Services.Core.Contracts.CarShop.CarShopListing.Requests;
using CarSpace.Services.Core.Contracts.CarShop.CarShopListing.Responses;
using CarSpace.Services.Core.Exceptions;
using CarSpace.Services.Core.Interfaces;
using CarSpace.Services.Core.Services.Interfaces;
using CarSpace.Services.Core.Services;
using Microsoft.AspNetCore.Http;
using Moq;

namespace CarSpace.Services.Tests;

[TestFixture]
public class CarShopListingServiceTests
{
    private Mock<ICarShopListingRepository> _carShopListingRepositoryMock;
    private Mock<IImageService> _imageServiceMock;
    private ICarShopListingService _carShopListingService;

    [SetUp]
    public void Setup()
    {
        _carShopListingRepositoryMock = new Mock<ICarShopListingRepository>();
        _imageServiceMock = new Mock<IImageService>();
        _carShopListingService = new CarShopListingService(_carShopListingRepositoryMock.Object, _imageServiceMock.Object);
    }

    #region GetCarShopListingsAsync Tests

    [Test]
    public async Task GetCarShopListingsAsync_ShouldReturnPaginatedResponse_WhenListingsExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new GetCarShopListingsRequest(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, false, false, 1, 10);

        var listings = new List<CarShopListing>
        {
            new CarShopListing
            {
                Id = Guid.NewGuid(),
                Title = "BMW 320i",
                Description = "Great car",
                Brand = new CarShopBrand { Name = "BMW" },
                Model = "320i",
                Year = 2020,
                FuelType = "Petrol",
                Mileage = 50000,
                Price = 25000,
                City = "Sofia",
                ImageUrl = "test.jpg",
                UpdatedAt = DateTime.UtcNow,
                User = new ApplicationUser(Guid.NewGuid(), "test@test.com", "testuser", "User") { UserName = "testuser" },
                SavedByUsers = new List<UserSavedCarShopListing>()
            },
            new CarShopListing
            {
                Id = Guid.NewGuid(),
                Title = "Audi A4",
                Description = "Excellent condition",
                Brand = new CarShopBrand { Name = "Audi" },
                Model = "A4",
                Year = 2019,
                FuelType = "Diesel",
                Mileage = 60000,
                Price = 28000,
                City = "Plovdiv",
                ImageUrl = "test2.jpg",
                UpdatedAt = DateTime.UtcNow,
                User = new ApplicationUser(Guid.NewGuid(), "another@test.com", "anotheruser", "User") { UserName = "anotheruser" },
                SavedByUsers = new List<UserSavedCarShopListing>()
            }
        };

        _carShopListingRepositoryMock.Setup(repo => repo.GetFilteredListingsAsync(
            It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(),
            It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<int?>(),
            It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>(),
            It.IsAny<string>(), It.IsAny<decimal?>(), It.IsAny<decimal?>(),
            It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<Guid?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((listings, 2));

        // Act
        var result = await _carShopListingService.GetCarShopListingsAsync(request, userId);

        // Assert
        Assert.AreEqual(2, result.Items.Count());
        Assert.AreEqual(1, result.CurrentPage);
        Assert.AreEqual(1, result.TotalPages);
        Assert.AreEqual(2, result.TotalCount);
        Assert.AreEqual("BMW 320i", result.Items.First().Title);
        Assert.AreEqual("BMW", result.Items.First().BrandName);
        Assert.AreEqual("testuser", result.Items.First().UserNickname);
    }

    [Test]
    public async Task GetCarShopListingsAsync_ShouldHandleNullBrandAndUser_WhenDataMissing()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new GetCarShopListingsRequest(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, false, false, 1, 10);

        var listings = new List<CarShopListing>
        {
            new CarShopListing
            {
                Id = Guid.NewGuid(),
                Title = "Test Car",
                Description = "Test Description",
                Brand = null,
                Model = "Test Model",
                Year = 2020,
                FuelType = "Petrol",
                Mileage = 50000,
                Price = 25000,
                City = "Sofia",
                ImageUrl = "test.jpg",
                UpdatedAt = DateTime.UtcNow,
                User = null,
                SavedByUsers = new List<UserSavedCarShopListing>()
            }
        };

        _carShopListingRepositoryMock.Setup(repo => repo.GetFilteredListingsAsync(
            It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(),
            It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<int?>(),
            It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>(),
            It.IsAny<string>(), It.IsAny<decimal?>(), It.IsAny<decimal?>(),
            It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<Guid?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((listings, 1));

        // Act
        var result = await _carShopListingService.GetCarShopListingsAsync(request, userId);

        // Assert
        Assert.AreEqual("Unknown", result.Items.First().BrandName);
        Assert.AreEqual("Unknown", result.Items.First().UserNickname);
    }

    #endregion

    #region GetCarShopListingByIdAsync Tests

    [Test]
    public async Task GetCarShopListingByIdAsync_ShouldReturnListing_WhenExists()
    {
        // Arrange
        var listingId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var listing = new CarShopListing
        {
            Id = listingId,
            Title = "BMW 320i",
            Description = "Great car",
            CarBrandId = 1,
            Brand = new CarShopBrand { Name = "BMW" },
            Model = "320i",
            Year = 2020,
            Mileage = 50000,
            Horsepower = 180,
            Transmission = "Automatic",
            FuelType = "Petrol",
            Color = "Black",
            EuroStandard = "Euro 6",
            Doors = 4,
            Price = 25000,
            City = "Sofia",
            Address = "Test Address",
            ImageUrl = "test.jpg",
            UpdatedAt = DateTime.UtcNow,
            UserId = Guid.NewGuid(),
            User = new ApplicationUser(Guid.NewGuid(), "test@test.com", "testuser", "User") { UserName = "testuser" },
            SavedByUsers = new List<UserSavedCarShopListing>()
        };

        _carShopListingRepositoryMock.Setup(repo => repo.GetByIdAsync(listingId))
            .ReturnsAsync(listing);

        // Act
        var result = await _carShopListingService.GetCarShopListingByIdAsync(listingId, userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(listingId, result.Id);
        Assert.AreEqual("BMW 320i", result.Title);
        Assert.AreEqual("BMW", result.BrandName);
        Assert.IsFalse(result.IsSavedByCurrentUser);
    }

    [Test]
    public async Task GetCarShopListingByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        var listingId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _carShopListingRepositoryMock.Setup(repo => repo.GetByIdAsync(listingId))
            .ReturnsAsync((CarShopListing)null);

        // Act
        var result = await _carShopListingService.GetCarShopListingByIdAsync(listingId, userId);

        // Assert
        Assert.IsNull(result);
    }

    #endregion

    #region CreateCarShopListingAsync Tests

    [Test]
    public async Task CreateCarShopListingAsync_ShouldCreateListing_WhenValidRequest()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var mockFile = new Mock<IFormFile>();
        var request = new CreateCarShopListingRequest(
            "BMW 320i",
            "Great car",
            1,
            "320i",
            2020,
            50000,
            180,
            "Automatic",
            "Petrol",
            "Black",
            "Euro 6",
            4,
            25000,
            "Sofia",
            "Test Address",
            mockFile.Object
        );

        var createdListing = new CarShopListing
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            CarBrandId = request.BrandId,
            Brand = new CarShopBrand { Name = "BMW" },
            Model = request.Model,
            Year = request.Year,
            Mileage = request.Mileage,
            Horsepower = request.Horsepower,
            Transmission = request.Transmission,
            FuelType = request.FuelType,
            Color = request.Color,
            EuroStandard = request.EuroStandard,
            Doors = request.Doors,
            Price = request.Price,
            City = request.City,
            Address = request.Address,
            ImageUrl = "saved-image.jpg",
            UpdatedAt = DateTime.UtcNow,
            UserId = userId,
            User = new ApplicationUser(Guid.NewGuid(), "test@test.com", "testuser", "User") { UserName = "testuser" }
        };

        _imageServiceMock.Setup(service => service.SaveImageOrDefaultAsync(
            It.IsAny<IFormFile>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync("saved-image.jpg");

        _carShopListingRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<CarShopListing>()))
            .Returns(Task.CompletedTask);

        _carShopListingRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(createdListing);

        // Act
        var result = await _carShopListingService.CreateCarShopListingAsync(request, userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("BMW 320i", result.Title);
        Assert.AreEqual("BMW", result.BrandName);
        Assert.AreEqual("saved-image.jpg", result.ImageUrl);
        Assert.IsFalse(result.IsSavedByCurrentUser);

        _carShopListingRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<CarShopListing>()), Times.Once);
    }

    [Test]
    public void CreateCarShopListingAsync_ShouldThrowValidationException_WhenTitleEmpty()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var mockFile = new Mock<IFormFile>();
        var request = new CreateCarShopListingRequest(
            "",
            "Great car",
            1,
            "320i",
            2020,
            50000,
            180,
            "Automatic",
            "Petrol",
            "Black",
            "Euro 6",
            4,
            25000,
            "Sofia",
            "Test Address",
            mockFile.Object
        );

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationAppException>(() => 
            _carShopListingService.CreateCarShopListingAsync(request, userId));
        Assert.AreEqual(ExceptionMessages.InvalidListingData, ex.Message);
    }

    [Test]
    public void CreateCarShopListingAsync_ShouldThrowValidationException_WhenYearTooOld()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var mockFile = new Mock<IFormFile>();
        var request = new CreateCarShopListingRequest(
            "Old Car",
            "Very old car",
            1,
            "Model T",
            1800,
            50000,
            180,
            "Manual",
            "Petrol",
            "Black",
            "Euro 1",
            4,
            25000,
            "Sofia",
            "Test Address",
            mockFile.Object
        );

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationAppException>(() => 
            _carShopListingService.CreateCarShopListingAsync(request, userId));
        Assert.AreEqual(ExceptionMessages.InvalidListingData, ex.Message);
    }

    #endregion

    #region UpdateCarShopListingAsync Tests

    [Test]
    public async Task UpdateCarShopListingAsync_ShouldUpdateListing_WhenValidRequest()
    {
        // Arrange
        var listingId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var mockFile = new Mock<IFormFile>();
        var request = new UpdateCarShopListingRequest(
            listingId,
            "Updated BMW 320i",
            "Updated description",
            1,
            "320i",
            2021,
            45000,
            200,
            "Automatic",
            "Petrol",
            "Blue",
            "Euro 6",
            4,
            30000,
            "Plovdiv",
            "Updated Address",
            null
        );

        var existingListing = new CarShopListing
        {
            Id = listingId,
            Title = "BMW 320i",
            UserId = userId,
            ImageUrl = "old-image.jpg"
        };

        _carShopListingRepositoryMock.Setup(repo => repo.GetByIdAsync(listingId))
            .ReturnsAsync(existingListing);

        _imageServiceMock.Setup(service => service.UpdateImageIfProvidedAsync(
            It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>()))
            .ReturnsAsync("updated-image.jpg");

        _carShopListingRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<CarShopListing>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _carShopListingService.UpdateCarShopListingAsync(request, userId);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual("Updated BMW 320i", existingListing.Title);
        Assert.AreEqual("Updated description", existingListing.Description);
        Assert.AreEqual(2021, existingListing.Year);

        _carShopListingRepositoryMock.Verify(repo => repo.UpdateAsync(existingListing), Times.Once);
    }

    [Test]
    public void UpdateCarShopListingAsync_ShouldThrowNotFoundException_WhenListingNotExists()
    {
        // Arrange
        var listingId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var request = new UpdateCarShopListingRequest(
            listingId, "Title", "Description", 1, "Model", 2020, 50000, 180,
            "Automatic", "Petrol", "Black", "Euro 6", 4, 25000, "Sofia", "Address", null
        );

        _carShopListingRepositoryMock.Setup(repo => repo.GetByIdAsync(listingId))
            .ReturnsAsync((CarShopListing)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<NotFoundException>(() => 
            _carShopListingService.UpdateCarShopListingAsync(request, userId));
        Assert.AreEqual(ExceptionMessages.ListingNotFound, ex.Message);
    }

    [Test]
    public void UpdateCarShopListingAsync_ShouldThrowUnauthorizedException_WhenNotOwner()
    {
        // Arrange
        var listingId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var differentUserId = Guid.NewGuid();
        var request = new UpdateCarShopListingRequest(
            listingId, "Title", "Description", 1, "Model", 2020, 50000, 180,
            "Automatic", "Petrol", "Black", "Euro 6", 4, 25000, "Sofia", "Address", null
        );

        var existingListing = new CarShopListing
        {
            Id = listingId,
            UserId = differentUserId
        };

        _carShopListingRepositoryMock.Setup(repo => repo.GetByIdAsync(listingId))
            .ReturnsAsync(existingListing);

        // Act & Assert
        var ex = Assert.ThrowsAsync<UnauthorizedAccessAppException>(() => 
            _carShopListingService.UpdateCarShopListingAsync(request, userId));
        Assert.AreEqual(ExceptionMessages.UnauthorizedListingAccess, ex.Message);
    }

    #endregion

    #region DeleteCarShopListingAsync Tests

    [Test]
    public async Task DeleteCarShopListingAsync_ShouldDeleteListing_WhenValidRequest()
    {
        // Arrange
        var listingId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var existingListing = new CarShopListing
        {
            Id = listingId,
            UserId = userId,
            ImageUrl = "test-image.jpg"
        };

        _carShopListingRepositoryMock.Setup(repo => repo.GetByIdAsync(listingId))
            .ReturnsAsync(existingListing);

        _imageServiceMock.Setup(service => service.DeleteImageAsync(It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        _carShopListingRepositoryMock.Setup(repo => repo.DeleteAsync(listingId))
            .Returns(Task.CompletedTask);

        // Act
        await _carShopListingService.DeleteCarShopListingAsync(listingId, userId);

        // Assert
        _imageServiceMock.Verify(service => service.DeleteImageAsync("test-image.jpg"), Times.Once);
        _carShopListingRepositoryMock.Verify(repo => repo.DeleteAsync(listingId), Times.Once);
    }

    [Test]
    public void DeleteCarShopListingAsync_ShouldThrowNotFoundException_WhenListingNotExists()
    {
        // Arrange
        var listingId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _carShopListingRepositoryMock.Setup(repo => repo.GetByIdAsync(listingId))
            .ReturnsAsync((CarShopListing)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<NotFoundException>(() => 
            _carShopListingService.DeleteCarShopListingAsync(listingId, userId));
        Assert.AreEqual(ExceptionMessages.ListingNotFound, ex.Message);
    }

    [Test]
    public void DeleteCarShopListingAsync_ShouldThrowUnauthorizedException_WhenNotOwner()
    {
        // Arrange
        var listingId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var differentUserId = Guid.NewGuid();

        var existingListing = new CarShopListing
        {
            Id = listingId,
            UserId = differentUserId,
            ImageUrl = "test-image.jpg"
        };

        _carShopListingRepositoryMock.Setup(repo => repo.GetByIdAsync(listingId))
            .ReturnsAsync(existingListing);

        // Act & Assert
        var ex = Assert.ThrowsAsync<UnauthorizedAccessAppException>(() => 
            _carShopListingService.DeleteCarShopListingAsync(listingId, userId));
        Assert.AreEqual(ExceptionMessages.UnauthorizedListingAccess, ex.Message);
    }

    #endregion

    #region Save/Unsave Tests

    [Test]
    public async Task SaveCarShopListingAsync_ShouldReturnTrue_WhenSuccessful()
    {
        // Arrange
        var listingId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _carShopListingRepositoryMock.Setup(repo => repo.SaveAsync(listingId, userId))
            .ReturnsAsync(true);

        // Act
        var result = await _carShopListingService.SaveCarShopListingAsync(listingId, userId);

        // Assert
        Assert.IsTrue(result);
        _carShopListingRepositoryMock.Verify(repo => repo.SaveAsync(listingId, userId), Times.Once);
    }

    [Test]
    public async Task UnsaveCarShopListingAsync_ShouldReturnTrue_WhenSuccessful()
    {
        // Arrange
        var listingId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _carShopListingRepositoryMock.Setup(repo => repo.UnsaveAsync(listingId, userId))
            .ReturnsAsync(true);

        // Act
        var result = await _carShopListingService.UnsaveCarShopListingAsync(listingId, userId);

        // Assert
        Assert.IsTrue(result);
        _carShopListingRepositoryMock.Verify(repo => repo.UnsaveAsync(listingId, userId), Times.Once);
    }

    #endregion
}
