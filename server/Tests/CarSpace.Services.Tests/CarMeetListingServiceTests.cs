using CarSpace.Data.Models.Entities.CarMeet;
using CarSpace.Data.Models.Entities.User;
using CarSpace.Data.Repositories.Interfaces;
using CarSpace.Services.Common.Messages;
using CarSpace.Services.Core.Contracts.CarMeet.Requests;
using CarSpace.Services.Core.Exceptions;
using CarSpace.Services.Core.Services;
using CarSpace.Services.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;

namespace CarSpace.Services.Tests;

[TestFixture]
public class CarMeetListingServiceTests
{
    private Mock<ICarMeetListingRepository> _carMeetListingRepositoryMock;
    private Mock<IImageService> _imageServiceMock;
    private ICarMeetListingService _carMeetListingService;

    [SetUp]
    public void Setup()
    {
        _carMeetListingRepositoryMock = new Mock<ICarMeetListingRepository>();
        _imageServiceMock = new Mock<IImageService>();
        _carMeetListingService = new CarMeetListingService(_carMeetListingRepositoryMock.Object, _imageServiceMock.Object);
    }

    #region GetCarMeetsAsync Tests

    [Test]
    public async Task GetCarMeetsAsync_ShouldReturnPaginatedResponse_WhenMeetsExist()
    {
        // Arrange
        var request = new GetCarMeetListingsRequest(1, 10);
        var userId = Guid.NewGuid();
        var futureDate = DateTime.UtcNow.AddDays(7);

        var meets = new List<CarMeetListing>
        {
            new CarMeetListing
            {
                Id = Guid.NewGuid(),
                Title = "BMW Meet",
                Description = "BMW car meet",
                City = "Los Angeles",
                ImageUrl = "meet1.jpg",
                MeetDate = futureDate,
                UpdatedAt = DateTime.UtcNow,
                User = new ApplicationUser(Guid.NewGuid(), "test@test.com", "testuser", "img.png"),
                SavedByUsers = new List<UserSavedCarMeetListing>(),
                Participants = new List<UserJoinedCarMeetListing>()
            }
        };

        _carMeetListingRepositoryMock.Setup(repo => repo.GetFilteredMeetsAsync(
                null, null, false, false, false, userId, 1, 10))
            .ReturnsAsync((meets, 1));

        // Act
        var result = await _carMeetListingService.GetCarMeetsAsync(request, userId);

        // Assert
        Assert.AreEqual(1, result.Items.Count());
        Assert.AreEqual(1, result.CurrentPage);
        Assert.AreEqual(1, result.TotalPages);
        Assert.AreEqual(1, result.TotalCount);
        Assert.AreEqual("BMW Meet", result.Items.First().Title);
        Assert.AreEqual("Los Angeles", result.Items.First().City);
    }

    [Test]
    public async Task GetCarMeetsAsync_ShouldHandleNullUser_WhenUserDataMissing()
    {
        // Arrange
        var request = new GetCarMeetListingsRequest(1, 10);
        var userId = Guid.NewGuid();

        var meets = new List<CarMeetListing>
        {
            new CarMeetListing
            {
                Id = Guid.NewGuid(),
                Title = "Test Meet",
                Description = "Test Description",
                City = "Test City",
                ImageUrl = "test.jpg",
                MeetDate = DateTime.UtcNow.AddDays(1),
                UpdatedAt = DateTime.UtcNow,
                User = null,
                SavedByUsers = new List<UserSavedCarMeetListing>(),
                Participants = new List<UserJoinedCarMeetListing>()
            }
        };

        _carMeetListingRepositoryMock.Setup(repo => repo.GetFilteredMeetsAsync(
                null, null, false, false, false, userId, 1, 10))
            .ReturnsAsync((meets, 1));

        // Act
        var result = await _carMeetListingService.GetCarMeetsAsync(request, userId);

        // Assert
        Assert.AreEqual("Unknown", result.Items.First().UserNickname);
    }

    #endregion

    #region GetCarMeetByIdAsync Tests

    [Test]
    public async Task GetCarMeetByIdAsync_ShouldReturnMeet_WhenExists()
    {
        // Arrange
        var meetId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var meet = new CarMeetListing
        {
            Id = meetId,
            Title = "Test Meet",
            Description = "Test Description",
            City = "Test City",
            Address = "123 Test St",
            ImageUrl = "test.jpg",
            MeetDate = DateTime.UtcNow.AddDays(1),
            UpdatedAt = DateTime.UtcNow,
            UserId = userId,
            User = new ApplicationUser(userId, "test@test.com", "testuser", "img.png"),
            SavedByUsers = new List<UserSavedCarMeetListing>(),
            Participants = new List<UserJoinedCarMeetListing>()
        };

        _carMeetListingRepositoryMock.Setup(repo => repo.GetCarMeetByIdAsync(meetId))
            .ReturnsAsync(meet);

        // Act
        var result = await _carMeetListingService.GetCarMeetByIdAsync(meetId, userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(meetId, result.Id);
        Assert.AreEqual("Test Meet", result.Title);
        Assert.AreEqual("Test City", result.City);
        Assert.IsFalse(result.IsSavedByCurrentUser);
        Assert.IsFalse(result.IsJoinedByCurrentUser);
    }

    [Test]
    public async Task GetCarMeetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        var meetId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _carMeetListingRepositoryMock.Setup(repo => repo.GetCarMeetByIdAsync(meetId))
            .ReturnsAsync((CarMeetListing)null);

        // Act
        var result = await _carMeetListingService.GetCarMeetByIdAsync(meetId, userId);

        // Assert
        Assert.IsNull(result);
    }

    #endregion

    #region CreateCarMeetAsync Tests

    [Test]
    public async Task CreateCarMeetAsync_ShouldCreateMeet_WhenValidRequest()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var futureDate = DateTime.UtcNow.AddDays(7);
        var imageFileMock = new Mock<IFormFile>();
        var request = new CreateCarMeetListingRequest(
            "Test Meet",
            "Test Description",
            futureDate,
            "Test City",
            "123 Test St",
            imageFileMock.Object
        );

        _imageServiceMock.Setup(s => s.SaveImageOrDefaultAsync(It.IsAny<IFormFile>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync("saved-image.jpg");

        _carMeetListingRepositoryMock.Setup(repo => repo.AddCarMeetAsync(It.IsAny<CarMeetListing>()))
            .Returns(Task.CompletedTask);

        _carMeetListingRepositoryMock.Setup(repo => repo.JoinCarMeetAsync(It.IsAny<Guid>(), userId))
            .ReturnsAsync(true);

        // Act
        var result = await _carMeetListingService.CreateCarMeetAsync(request, userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Test Meet", result.Title);
        Assert.AreEqual("Test City", result.City);
        Assert.AreEqual("saved-image.jpg", result.ImageUrl);
        Assert.IsTrue(result.IsJoinedByCurrentUser);
        Assert.IsFalse(result.IsSavedByCurrentUser);

        _carMeetListingRepositoryMock.Verify(repo => repo.AddCarMeetAsync(It.IsAny<CarMeetListing>()), Times.Once);
        _carMeetListingRepositoryMock.Verify(repo => repo.JoinCarMeetAsync(It.IsAny<Guid>(), userId), Times.Once);
    }

    [Test]
    public void CreateCarMeetAsync_ShouldThrowValidationException_WhenTitleEmpty()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var futureDate = DateTime.UtcNow.AddDays(7);
        var request = new CreateCarMeetListingRequest("", "Description", futureDate, "City", "Address", null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationAppException>(() => 
            _carMeetListingService.CreateCarMeetAsync(request, userId));
        Assert.AreEqual(ExceptionMessages.InvalidMeetData, ex.Message);
    }

    [Test]
    public void CreateCarMeetAsync_ShouldThrowValidationException_WhenMeetDateInPast()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var pastDate = DateTime.UtcNow.AddDays(-1);
        var request = new CreateCarMeetListingRequest("Title", "Description", pastDate, "City", "Address", null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationAppException>(() => 
            _carMeetListingService.CreateCarMeetAsync(request, userId));
        Assert.AreEqual(ExceptionMessages.InvalidMeetData, ex.Message);
    }

    #endregion

    #region UpdateCarMeetAsync Tests

    [Test]
    public async Task UpdateCarMeetAsync_ShouldUpdateMeet_WhenValidRequest()
    {
        // Arrange
        var meetId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var futureDate = DateTime.UtcNow.AddDays(7);
        var imageFileMock = new Mock<IFormFile>();
        var request = new UpdateCarMeetListingRequest(
            meetId,
            "Updated Title",
            "Updated Description",
            futureDate,
            "Updated City",
            "Updated Address",
            imageFileMock.Object
        );

        var existingMeet = new CarMeetListing
        {
            Id = meetId,
            Title = "Original Title",
            Description = "Original Description",
            City = "Original City",
            Address = "Original Address",
            ImageUrl = "original.jpg",
            UserId = userId,
            UpdatedAt = DateTime.UtcNow.AddDays(-1)
        };

        _carMeetListingRepositoryMock.Setup(repo => repo.GetCarMeetByIdAsync(meetId))
            .ReturnsAsync(existingMeet);

        _imageServiceMock.Setup(s => s.UpdateImageIfProvidedAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>()))
            .ReturnsAsync("updated-image.jpg");

        _carMeetListingRepositoryMock.Setup(repo => repo.UpdateCarMeetAsync(It.IsAny<CarMeetListing>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _carMeetListingService.UpdateCarMeetAsync(request, userId);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual("Updated Title", existingMeet.Title);
        Assert.AreEqual("Updated City", existingMeet.City);
        Assert.AreEqual("updated-image.jpg", existingMeet.ImageUrl);

        _carMeetListingRepositoryMock.Verify(repo => repo.UpdateCarMeetAsync(It.IsAny<CarMeetListing>()), Times.Once);
    }

    [Test]
    public void UpdateCarMeetAsync_ShouldThrowNotFoundException_WhenMeetNotExists()
    {
        // Arrange
        var meetId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var request = new UpdateCarMeetListingRequest(meetId, "Title", "Description", DateTime.UtcNow.AddDays(1), "City", "Address", null);

        _carMeetListingRepositoryMock.Setup(repo => repo.GetCarMeetByIdAsync(meetId))
            .ReturnsAsync((CarMeetListing)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<NotFoundException>(() => 
            _carMeetListingService.UpdateCarMeetAsync(request, userId));
        Assert.AreEqual(ExceptionMessages.MeetNotFound, ex.Message);
    }

    [Test]
    public void UpdateCarMeetAsync_ShouldThrowUnauthorized_WhenUserNotOwner()
    {
        // Arrange
        var meetId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var differentUserId = Guid.NewGuid();
        var request = new UpdateCarMeetListingRequest(meetId, "Title", "Description", DateTime.UtcNow.AddDays(1), "City", "Address", null);

        var existingMeet = new CarMeetListing
        {
            Id = meetId,
            UserId = differentUserId
        };

        _carMeetListingRepositoryMock.Setup(repo => repo.GetCarMeetByIdAsync(meetId))
            .ReturnsAsync(existingMeet);

        // Act & Assert
        var ex = Assert.ThrowsAsync<UnauthorizedAccessAppException>(() => 
            _carMeetListingService.UpdateCarMeetAsync(request, userId));
        Assert.AreEqual(ExceptionMessages.UnauthorizedMeetAccess, ex.Message);
    }

    #endregion

    #region DeleteCarMeetAsync Tests

    [Test]
    public async Task DeleteCarMeetAsync_ShouldDeleteMeet_WhenValidRequest()
    {
        // Arrange
        var meetId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var existingMeet = new CarMeetListing
        {
            Id = meetId,
            UserId = userId,
            ImageUrl = "test.jpg"
        };

        _carMeetListingRepositoryMock.Setup(repo => repo.GetCarMeetByIdAsync(meetId))
            .ReturnsAsync(existingMeet);

        _imageServiceMock.Setup(s => s.DeleteImageAsync("test.jpg"))
            .Returns(Task.CompletedTask);

        _carMeetListingRepositoryMock.Setup(repo => repo.DeleteCarMeetAsync(meetId))
            .Returns(Task.CompletedTask);

        // Act
        await _carMeetListingService.DeleteCarMeetAsync(meetId, userId);

        // Assert
        _imageServiceMock.Verify(s => s.DeleteImageAsync("test.jpg"), Times.Once);
        _carMeetListingRepositoryMock.Verify(repo => repo.DeleteCarMeetAsync(meetId), Times.Once);
    }

    [Test]
    public void DeleteCarMeetAsync_ShouldThrowNotFoundException_WhenMeetNotExists()
    {
        // Arrange
        var meetId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _carMeetListingRepositoryMock.Setup(repo => repo.GetCarMeetByIdAsync(meetId))
            .ReturnsAsync((CarMeetListing)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<NotFoundException>(() => 
            _carMeetListingService.DeleteCarMeetAsync(meetId, userId));
        Assert.AreEqual(ExceptionMessages.MeetNotFound, ex.Message);
    }

    [Test]
    public void DeleteCarMeetAsync_ShouldThrowUnauthorized_WhenUserNotOwner()
    {
        // Arrange
        var meetId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var differentUserId = Guid.NewGuid();

        var existingMeet = new CarMeetListing
        {
            Id = meetId,
            UserId = differentUserId
        };

        _carMeetListingRepositoryMock.Setup(repo => repo.GetCarMeetByIdAsync(meetId))
            .ReturnsAsync(existingMeet);

        // Act & Assert
        var ex = Assert.ThrowsAsync<UnauthorizedAccessAppException>(() => 
            _carMeetListingService.DeleteCarMeetAsync(meetId, userId));
        Assert.AreEqual(ExceptionMessages.UnauthorizedMeetAccess, ex.Message);
    }

    #endregion

    #region Save/Unsave/Join/Leave Tests

    [Test]
    public async Task SaveCarMeetAsync_ShouldCallRepository_WithCorrectParameters()
    {
        // Arrange
        var meetId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _carMeetListingRepositoryMock.Setup(repo => repo.SaveMeetAsync(meetId, userId))
            .ReturnsAsync(true);

        // Act
        var result = await _carMeetListingService.SaveCarMeetAsync(meetId, userId);

        // Assert
        Assert.IsTrue(result);
        _carMeetListingRepositoryMock.Verify(repo => repo.SaveMeetAsync(meetId, userId), Times.Once);
    }

    [Test]
    public async Task UnsaveCarMeetAsync_ShouldCallRepository_WithCorrectParameters()
    {
        // Arrange
        var meetId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _carMeetListingRepositoryMock.Setup(repo => repo.UnsaveMeetAsync(meetId, userId))
            .ReturnsAsync(true);

        // Act
        var result = await _carMeetListingService.UnsaveCarMeetAsync(meetId, userId);

        // Assert
        Assert.IsTrue(result);
        _carMeetListingRepositoryMock.Verify(repo => repo.UnsaveMeetAsync(meetId, userId), Times.Once);
    }

    [Test]
    public async Task JoinCarMeetAsync_ShouldCallRepository_WithCorrectParameters()
    {
        // Arrange
        var meetId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _carMeetListingRepositoryMock.Setup(repo => repo.JoinCarMeetAsync(meetId, userId))
            .ReturnsAsync(true);

        // Act
        var result = await _carMeetListingService.JoinCarMeetAsync(meetId, userId);

        // Assert
        Assert.IsTrue(result);
        _carMeetListingRepositoryMock.Verify(repo => repo.JoinCarMeetAsync(meetId, userId), Times.Once);
    }

    [Test]
    public async Task LeaveCarMeetAsync_ShouldCallRepository_WithCorrectParameters()
    {
        // Arrange
        var meetId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _carMeetListingRepositoryMock.Setup(repo => repo.LeaveCarMeetAsync(meetId, userId))
            .ReturnsAsync(true);

        // Act
        var result = await _carMeetListingService.LeaveCarMeetAsync(meetId, userId);

        // Assert
        Assert.IsTrue(result);
        _carMeetListingRepositoryMock.Verify(repo => repo.LeaveCarMeetAsync(meetId, userId), Times.Once);
    }

    #endregion

    #region GetCarMeetParticipantsAsync Tests

    [Test]
    public async Task GetCarMeetParticipantsAsync_ShouldReturnParticipants_WhenParticipantsExist()
    {
        // Arrange
        var meetId = Guid.NewGuid();
        var participants = new List<ApplicationUser>
        {
            new ApplicationUser(Guid.NewGuid(), "user1@test.com", "user1", "img1.png"),
            new ApplicationUser(Guid.NewGuid(), "user2@test.com", "user2", "img2.png")
        };

        _carMeetListingRepositoryMock.Setup(repo => repo.GetParticipantsAsync(meetId, 1, 10))
            .ReturnsAsync((participants, 2));

        // Act
        var result = await _carMeetListingService.GetCarMeetParticipantsAsync(meetId, 1, 10);

        // Assert
        Assert.AreEqual(2, result.Items.Count());
        Assert.AreEqual(1, result.CurrentPage);
        Assert.AreEqual(1, result.TotalPages);
        Assert.AreEqual(2, result.TotalCount);
        Assert.AreEqual("user1", result.Items.First().Username);
    }

    [Test]
    public async Task GetCarMeetParticipantsAsync_ShouldReturnEmptyList_WhenNoParticipants()
    {
        // Arrange
        var meetId = Guid.NewGuid();
        var participants = new List<ApplicationUser>();

        _carMeetListingRepositoryMock.Setup(repo => repo.GetParticipantsAsync(meetId, 1, 10))
            .ReturnsAsync((participants, 0));

        // Act
        var result = await _carMeetListingService.GetCarMeetParticipantsAsync(meetId, 1, 10);

        // Assert
        Assert.AreEqual(0, result.Items.Count());
        Assert.AreEqual(0, result.TotalCount);
    }

    #endregion
}
