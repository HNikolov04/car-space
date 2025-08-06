using CarSpace.Services.Core.Services;
using CarSpace.Services.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CarSpace.Services.Tests;

[TestFixture]
public class JwtServiceTests
{
    private Mock<IConfiguration> _configurationMock;
    private IJwtService _jwtService;

    [SetUp]
    public void Setup()
    {
        _configurationMock = new Mock<IConfiguration>();
        _jwtService = new JwtService(_configurationMock.Object);
    }

    [Test]
    public void GenerateToken_ShouldReturnValidToken_WhenConfigurationIsValid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var email = "test@example.com";
        var role = "User";

        SetupValidConfiguration();

        // Act
        var token = _jwtService.GenerateToken(userId, email, role);

        // Assert
        Assert.IsNotNull(token);
        Assert.IsNotEmpty(token);
        
        // Verify token structure (JWT has 3 parts separated by dots)
        var tokenParts = token.Split('.');
        Assert.AreEqual(3, tokenParts.Length);
    }

    [Test]
    public void GenerateToken_ShouldContainCorrectClaims_WhenTokenIsGenerated()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var email = "test@example.com";
        var role = "Admin";

        SetupValidConfiguration();

        // Act
        var token = _jwtService.GenerateToken(userId, email, role);

        // Assert
        var tokenHandler = new JwtSecurityTokenHandler();
        var jsonToken = tokenHandler.ReadJwtToken(token);

        // Verify claims
        var subClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
        var emailClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);
        var roleClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

        Assert.IsNotNull(subClaim);
        Assert.AreEqual(userId.ToString(), subClaim.Value);
        
        Assert.IsNotNull(emailClaim);
        Assert.AreEqual(email, emailClaim.Value);
        
        Assert.IsNotNull(roleClaim);
        Assert.AreEqual(role, roleClaim.Value);
    }

    [Test]
    public void GenerateToken_ShouldHaveCorrectIssuerAndAudience_WhenTokenIsGenerated()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var email = "test@example.com";
        var role = "User";
        var expectedIssuer = "CarSpaceApp";
        var expectedAudience = "CarSpaceUsers";

        SetupValidConfiguration(issuer: expectedIssuer, audience: expectedAudience);

        // Act
        var token = _jwtService.GenerateToken(userId, email, role);

        // Assert
        var tokenHandler = new JwtSecurityTokenHandler();
        var jsonToken = tokenHandler.ReadJwtToken(token);

        Assert.AreEqual(expectedIssuer, jsonToken.Issuer);
        Assert.Contains(expectedAudience, jsonToken.Audiences.ToArray());
    }

    [Test]
    public void GenerateToken_ShouldHaveExpirationTime_WhenTokenIsGenerated()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var email = "test@example.com";
        var role = "User";

        SetupValidConfiguration();

        var beforeGeneration = DateTime.UtcNow;

        // Act
        var token = _jwtService.GenerateToken(userId, email, role);

        // Assert
        var tokenHandler = new JwtSecurityTokenHandler();
        var jsonToken = tokenHandler.ReadJwtToken(token);

        // Token should expire approximately 2 hours from now
        var expectedExpiry = beforeGeneration.AddHours(2);
        var actualExpiry = jsonToken.ValidTo;

        // Allow 1 minute tolerance for test execution time
        var timeDifference = Math.Abs((expectedExpiry - actualExpiry).TotalMinutes);
        Assert.IsTrue(timeDifference < 1, $"Token expiry time difference is {timeDifference} minutes, expected < 1 minute");
    }

    [Test]
    public void GenerateToken_ShouldThrowException_WhenJwtKeyIsNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var email = "test@example.com";
        var role = "User";

        _configurationMock.Setup(c => c["Jwt:Key"]).Returns((string)null);
        _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("CarSpaceApp");
        _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("CarSpaceUsers");

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => 
            _jwtService.GenerateToken(userId, email, role));
        
        Assert.AreEqual("JWT key is not configured.", ex.Message);
    }

    [Test]
    public void GenerateToken_ShouldThrowException_WhenJwtKeyIsEmpty()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var email = "test@example.com";
        var role = "User";

        _configurationMock.Setup(c => c["Jwt:Key"]).Returns("");
        _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("CarSpaceApp");
        _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("CarSpaceUsers");

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => 
            _jwtService.GenerateToken(userId, email, role));
        
        Assert.AreEqual("JWT key is not configured.", ex.Message);
    }

    [Test]
    public void GenerateToken_ShouldThrowException_WhenJwtKeyIsWhitespace()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var email = "test@example.com";
        var role = "User";

        _configurationMock.Setup(c => c["Jwt:Key"]).Returns("   ");
        _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("CarSpaceApp");
        _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("CarSpaceUsers");

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => 
            _jwtService.GenerateToken(userId, email, role));
        
        Assert.AreEqual("JWT key is not configured.", ex.Message);
    }

    [Test]
    public void GenerateToken_ShouldThrowException_WhenIssuerIsNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var email = "test@example.com";
        var role = "User";

        _configurationMock.Setup(c => c["Jwt:Key"]).Returns("super-secret-key-that-is-at-least-32-characters-long-for-hmac-sha256");
        _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns((string)null);
        _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("CarSpaceUsers");

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => 
            _jwtService.GenerateToken(userId, email, role));
        
        Assert.AreEqual("JWT issuer is not configured.", ex.Message);
    }

    [Test]
    public void GenerateToken_ShouldThrowException_WhenAudienceIsNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var email = "test@example.com";
        var role = "User";

        _configurationMock.Setup(c => c["Jwt:Key"]).Returns("super-secret-key-that-is-at-least-32-characters-long-for-hmac-sha256");
        _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("CarSpaceApp");
        _configurationMock.Setup(c => c["Jwt:Audience"]).Returns((string)null);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => 
            _jwtService.GenerateToken(userId, email, role));
        
        Assert.AreEqual("JWT audience is not configured.", ex.Message);
    }

    [Test]
    public void GenerateToken_ShouldGenerateDifferentTokens_ForDifferentUsers()
    {
        // Arrange
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var email1 = "user1@example.com";
        var email2 = "user2@example.com";
        var role = "User";

        SetupValidConfiguration();

        // Act
        var token1 = _jwtService.GenerateToken(userId1, email1, role);
        var token2 = _jwtService.GenerateToken(userId2, email2, role);

        // Assert
        Assert.AreNotEqual(token1, token2);
    }

    [Test]
    public void GenerateToken_ShouldGenerateDifferentTokens_ForSameUserAtDifferentTimes()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var email = "test@example.com";
        var role = "User";

        SetupValidConfiguration();

        // Act
        var token1 = _jwtService.GenerateToken(userId, email, role);
        
        // Wait a small amount to ensure different timestamps
        Thread.Sleep(1000);
        
        var token2 = _jwtService.GenerateToken(userId, email, role);

        // Assert
        Assert.AreNotEqual(token1, token2);
    }

    private void SetupValidConfiguration(string key = null, string issuer = null, string audience = null)
    {
        _configurationMock.Setup(c => c["Jwt:Key"]).Returns(key ?? "super-secret-key-that-is-at-least-32-characters-long-for-hmac-sha256");
        _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns(issuer ?? "CarSpaceApp");
        _configurationMock.Setup(c => c["Jwt:Audience"]).Returns(audience ?? "CarSpaceUsers");
    }
}
