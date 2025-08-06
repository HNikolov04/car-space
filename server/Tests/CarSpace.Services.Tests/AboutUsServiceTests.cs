using CarSpace.Data.Models.Entities.About;
using CarSpace.Data.Repositories.Interfaces;
using CarSpace.Services.Common.Messages;
using CarSpace.Services.Core.Contracts.About.Request;
using CarSpace.Services.Core.Exceptions;
using CarSpace.Services.Core.Services;
using CarSpace.Services.Core.Services.Interfaces;
using Moq;

namespace CarSpace.Services.Tests;

[TestFixture]
public class AboutUsServiceTests
{
    private Mock<IAboutUsRepository> _aboutUsRepositoryMock;
    private IAboutUsService _aboutUsService;

    [SetUp]
    public void Setup()
    {
        _aboutUsRepositoryMock = new Mock<IAboutUsRepository>();
        _aboutUsService = new AboutUsService(_aboutUsRepositoryMock.Object);
    }

    [Test]
    public async Task GetAsync_ShouldReturnAboutUs_WhenExists()
    {
        // Arrange
        var expected = new AboutUs
        {
            Title = "Welcome",
            Message = "We are CarSpace!"
        };

        _aboutUsRepositoryMock.Setup(repo => repo.GetAsync())
            .ReturnsAsync(expected);

        // Act
        var result = await _aboutUsService.GetAsync();

        // Assert
        Assert.AreEqual(expected.Title, result.Title);
        Assert.AreEqual(expected.Message, result.Message);
    }

    [Test]
    public void GetAsync_ShouldThrowNotFound_WhenAboutUsIsNull()
    {
        // Arrange
        _aboutUsRepositoryMock.Setup(repo => repo.GetAsync())
            .ReturnsAsync((AboutUs)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<NotFoundException>(() => _aboutUsService.GetAsync());
        Assert.AreEqual(ExceptionMessages.AboutSectionNotFound, ex.Message);
    }

    [Test]
    public async Task UpdateAsync_ShouldUpdate_WhenValid()
    {
        // Arrange
        var aboutEntity = new AboutUs
        {
            Title = "Old",
            Message = "Old message"
        };

        var updateRequest = new UpdateAboutUsRequest("New Title", "New message");

        _aboutUsRepositoryMock.Setup(repo => repo.GetAsync())
            .ReturnsAsync(aboutEntity);

        _aboutUsRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<AboutUs>()))
            .Returns(Task.CompletedTask);

        // Act
        await _aboutUsService.UpdateAsync(updateRequest);

        // Assert
        _aboutUsRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<AboutUs>(
            a => a.Title == updateRequest.Title && a.Message == updateRequest.Message
        )), Times.Once);
    }

    [Test]
    public void UpdateAsync_ShouldThrowNotFound_WhenAboutUsIsNull()
    {
        // Arrange
        var updateRequest = new UpdateAboutUsRequest("Any", "Any");

        _aboutUsRepositoryMock.Setup(repo => repo.GetAsync())
            .ReturnsAsync((AboutUs)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<NotFoundException>(() => _aboutUsService.UpdateAsync(updateRequest));
        Assert.AreEqual(ExceptionMessages.AboutSectionNotFound, ex.Message);
    }

    [TestCase("", "Message")]
    [TestCase("Title", "")]
    [TestCase("   ", "Valid")]
    [TestCase("Valid", "")]
    public void UpdateAsync_ShouldThrowValidationException_WhenRequestInvalid(string title, string message)
    {
        // Arrange
        var invalidRequest = new UpdateAboutUsRequest(title, message);

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationAppException>(() => _aboutUsService.UpdateAsync(invalidRequest));
        Assert.AreEqual(ExceptionMessages.InvalidAboutData, ex.Message);
    }
}
