using CarSpace.Data.Models.Entities.User;
using CarSpace.Services.Common.Messages;
using CarSpace.Services.Core.Contracts.User.Requests;
using CarSpace.Services.Core.Exceptions;
using CarSpace.Services.Core.Services;
using CarSpace.Services.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace CarSpace.Services.Tests;

[TestFixture]
public class ApplicationUserServiceTests
{
    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private Mock<SignInManager<ApplicationUser>> _signInManagerMock;
    private Mock<IJwtService> _jwtServiceMock;
    private Mock<IImageService> _imageServiceMock;
    private ApplicationUserService _userService;

    [SetUp]
    public void Setup()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            store.Object, null, null, null, null, null, null, null, null);

        var contextAccessor = new Mock<IHttpContextAccessor>();
        var claimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
        _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
            _userManagerMock.Object,
            contextAccessor.Object,
            claimsFactory.Object,
            null, null, null, null);

        _jwtServiceMock = new Mock<IJwtService>();
        _imageServiceMock = new Mock<IImageService>();

        _userService = new ApplicationUserService(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _jwtServiceMock.Object,
            _imageServiceMock.Object
        );
    }

    [Test]
    public async Task RegisterAsync_ShouldReturnSuccess_WhenValid()
    {
        var request = new RegisterApplicationUserRequest("test@email.com", "Password123!", "username");

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "User"))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _userService.RegisterAsync(request);

        Assert.IsTrue(result.Succeeded);
    }

    [Test]
    public void RegisterAsync_ShouldThrowValidationException_WhenInvalid()
    {
        var request = new RegisterApplicationUserRequest("", "", "");

        var ex = Assert.ThrowsAsync<ValidationAppException>(() => _userService.RegisterAsync(request));
        Assert.AreEqual(ExceptionMessages.InvalidUserData, ex.Message);
    }

    [Test]
    public async Task LoginAsync_ShouldReturnToken_WhenValid()
    {
        var request = new LoginApplicationUserRequest("test@email.com", "Password123!");

        var user = new ApplicationUser(Guid.NewGuid(), request.Email, "username", "img.png");

        var users = new List<ApplicationUser> { user }.AsQueryable();
        _userManagerMock.Setup(x => x.Users).Returns(users);

        _signInManagerMock.Setup(x =>
                x.CheckPasswordSignInAsync(user, request.Password, false))
            .ReturnsAsync(SignInResult.Success);

        _userManagerMock.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "User" });

        _jwtServiceMock.Setup(x => x.GenerateToken(user.Id, user.Email, "User"))
            .Returns("fake-token");

        var result = await _userService.LoginAsync(request);

        Assert.AreEqual("fake-token", result.JwtToken);
    }

    [Test]
    public void LoginAsync_ShouldThrowNotFound_WhenUserMissing()
    {
        var request = new LoginApplicationUserRequest("notfound@email.com", "Password123!");

        var users = new List<ApplicationUser>().AsQueryable();
        _userManagerMock.Setup(x => x.Users).Returns(users);

        var ex = Assert.ThrowsAsync<NotFoundException>(() => _userService.LoginAsync(request));
        Assert.AreEqual(ExceptionMessages.UserNotFound, ex.Message);
    }
}
