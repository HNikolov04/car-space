using CarSpace.Data.Models.Entities.Contact;
using CarSpace.Data.Repositories.Interfaces;
using CarSpace.Services.Common.Messages;
using CarSpace.Services.Core.Contracts.Contact.Requests;
using CarSpace.Services.Core.Contracts.Contact.Responses;
using CarSpace.Services.Core.Exceptions;
using CarSpace.Services.Core.Services;
using CarSpace.Services.Core.Services.Interfaces;
using Moq;

namespace CarSpace.Services.Tests;

[TestFixture]
public class ContactUsServiceTests
{
    private Mock<IContactUsRepository> _contactUsRepositoryMock;
    private IContactUsService _contactUsService;

    [SetUp]
    public void Setup()
    {
        _contactUsRepositoryMock = new Mock<IContactUsRepository>();
        _contactUsService = new ContactUsService(_contactUsRepositoryMock.Object);
    }

    #region GetAsync Tests

    [Test]
    public async Task GetAsync_ShouldReturnContactInfo_WhenContactExists()
    {
        // Arrange
        var contact = new ContactUs
        {
            Id = Guid.NewGuid(),
            Title = "Contact CarSpace",
            Email = "info@carspace.com",
            Phone = "+359 888 123 456",
            Message = "Feel free to contact us anytime!",
            UpdatedAt = DateTime.UtcNow
        };

        _contactUsRepositoryMock.Setup(repo => repo.GetAsync())
            .ReturnsAsync(contact);

        // Act
        var result = await _contactUsService.GetAsync();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Contact CarSpace", result.Title);
        Assert.AreEqual("info@carspace.com", result.Email);
        Assert.AreEqual("+359 888 123 456", result.Phone);
        Assert.AreEqual("Feel free to contact us anytime!", result.Message);

        _contactUsRepositoryMock.Verify(repo => repo.GetAsync(), Times.Once);
    }

    [Test]
    public void GetAsync_ShouldThrowNotFoundException_WhenContactNotExists()
    {
        // Arrange
        _contactUsRepositoryMock.Setup(repo => repo.GetAsync())
            .ReturnsAsync((ContactUs)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<NotFoundException>(() => _contactUsService.GetAsync());
        Assert.AreEqual(ExceptionMessages.ContactInfoNotSet, ex.Message);

        _contactUsRepositoryMock.Verify(repo => repo.GetAsync(), Times.Once);
    }

    #endregion

    #region UpdateAsync Tests

    [Test]
    public async Task UpdateAsync_ShouldUpdateContactInfo_WhenContactExists()
    {
        // Arrange
        var existingContact = new ContactUs
        {
            Id = Guid.NewGuid(),
            Title = "Old Title",
            Email = "old@carspace.com",
            Phone = "+359 888 000 000",
            Message = "Old message",
            UpdatedAt = DateTime.UtcNow.AddDays(-1)
        };

        var request = new UpdateContactUsRequest(
            "New Contact Title",
            "new@carspace.com",
            "+359 888 999 999",
            "Updated contact message"
        );

        _contactUsRepositoryMock.Setup(repo => repo.GetAsync())
            .ReturnsAsync(existingContact);

        _contactUsRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<ContactUs>()))
            .Returns(Task.CompletedTask);

        // Act
        await _contactUsService.UpdateAsync(request);

        // Assert
        Assert.AreEqual("new@carspace.com", existingContact.Email);
        Assert.AreEqual("+359 888 999 999", existingContact.Phone);
        Assert.AreEqual("Updated contact message", existingContact.Message);
        Assert.That(existingContact.UpdatedAt, Is.GreaterThan(DateTime.UtcNow.AddMinutes(-1)));

        _contactUsRepositoryMock.Verify(repo => repo.GetAsync(), Times.Once);
        _contactUsRepositoryMock.Verify(repo => repo.UpdateAsync(existingContact), Times.Once);
    }

    [Test]
    public void UpdateAsync_ShouldThrowNotFoundException_WhenContactNotExists()
    {
        // Arrange
        var request = new UpdateContactUsRequest(
            "New Title",
            "new@carspace.com",
            "+359 888 999 999",
            "New message"
        );

        _contactUsRepositoryMock.Setup(repo => repo.GetAsync())
            .ReturnsAsync((ContactUs)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<NotFoundException>(() => _contactUsService.UpdateAsync(request));
        Assert.AreEqual(ExceptionMessages.ContactInfoNotSet, ex.Message);

        _contactUsRepositoryMock.Verify(repo => repo.GetAsync(), Times.Once);
        _contactUsRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<ContactUs>()), Times.Never);
    }

    #endregion
}
