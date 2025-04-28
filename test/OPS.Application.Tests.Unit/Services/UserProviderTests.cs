using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using OPS.Application.Services;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Services;

public class UserProviderTests
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserProvider _sut;
    private static readonly string[] s_expectation = ["Permission1", "Permission2"];

    public UserProviderTests()
    {
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        _sut = new UserProvider(_httpContextAccessor);
    }

    [Fact]
    public void IsAuthenticated_WhenUserIsAuthenticated_ReturnsTrue()
    {
        // Arrange
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim("AccountId", Guid.NewGuid().ToString()),
            new Claim("Username", "testuser"),
            new Claim("Email", "test@example.com")
        }, "TestAuth"));

        var httpContext = new DefaultHttpContext { User = claimsPrincipal };
        _httpContextAccessor.HttpContext.Returns(httpContext);

        // Act
        var result = _sut.IsAuthenticated();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsAuthenticated_WhenUserIsNotAuthenticated_ReturnsFalse()
    {
        // Arrange
        var httpContext = new DefaultHttpContext { User = new ClaimsPrincipal() };
        _httpContextAccessor.HttpContext.Returns(httpContext);

        // Act
        var result = _sut.IsAuthenticated();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetCurrentUser_WhenUserIsAuthenticated_ReturnsCurrentUser()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var username = "testuser";
        var email = "test@example.com";
        var permissions = new[] { "Permission1", "Permission2" };
        var roles = new[] { RoleType.Admin.ToString(), RoleType.Candidate.ToString() };

        var claims = new List<Claim>
        {
            new("AccountId", accountId.ToString()),
            new("Username", username),
            new("Email", email)
        };

        claims.AddRange(permissions.Select(p => new Claim("Permission", p)));
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"));
        var httpContext = new DefaultHttpContext { User = claimsPrincipal };
        _httpContextAccessor.HttpContext.Returns(httpContext);

        // Act
        var result = _sut.GetCurrentUser();

        // Assert
        result.Should().NotBeNull();
        result.AccountId.Should().Be(accountId);
        result.Username.Should().Be(username);
        result.Email.Should().Be(email);
        result.Permissions.Should().BeEquivalentTo(permissions);
        result.Roles.Should().BeEquivalentTo(roles.Select(r => Enum.Parse<RoleType>(r)));
    }

    [Fact]
    public void GetPermissions_WhenUserHasPermissions_ReturnsPermissions()
    {
        // Arrange
        var permissions = new[] { "Permission1", "Permission2" };
        var claims = permissions.Select(p => new Claim("Permission", p)).ToList();
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"));
        var httpContext = new DefaultHttpContext { User = claimsPrincipal };
        _httpContextAccessor.HttpContext.Returns(httpContext);

        // Act
        var result = _sut.GetPermissions();

        // Assert
        result.Should().BeEquivalentTo(permissions);
    }

    [Fact]
    public void AccountId_WhenUserIsAuthenticated_ReturnsAccountId()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim("AccountId", accountId.ToString())
        }, "TestAuth"));
        var httpContext = new DefaultHttpContext { User = claimsPrincipal };
        _httpContextAccessor.HttpContext.Returns(httpContext);

        // Act
        var result = _sut.AccountId();

        // Assert
        result.Should().Be(accountId);
    }

    [Fact]
    public void AccountId_WhenAccountIdIsMissing_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        var httpContext = new DefaultHttpContext { User = claimsPrincipal };
        _httpContextAccessor.HttpContext.Returns(httpContext);

        // Act
        var act = () => _sut.AccountId();

        // Assert
        act.Should().Throw<UnauthorizedAccessException>()
            .WithMessage("Account ID is missing or invalid.");
    }

    [Fact]
    public void TryGetAccountId_WhenUserIsAuthenticated_ReturnsAccountId()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim("AccountId", accountId.ToString())
        }, "TestAuth"));
        var httpContext = new DefaultHttpContext { User = claimsPrincipal };
        _httpContextAccessor.HttpContext.Returns(httpContext);

        // Act
        var result = _sut.TryGetAccountId();

        // Assert
        result.Should().Be(accountId);
    }

    [Fact]
    public void TryGetAccountId_WhenAccountIdIsMissing_ReturnsNull()
    {
        // Arrange
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        var httpContext = new DefaultHttpContext { User = claimsPrincipal };
        _httpContextAccessor.HttpContext.Returns(httpContext);

        // Act
        var result = _sut.TryGetAccountId();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void DecodeToken_WhenUserHasClaims_ReturnsDecodedClaims()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new("AccountId", Guid.NewGuid().ToString()),
            new("Username", "testuser"),
            new("Email", "test@example.com"),
            new("Permission", "Permission1"),
            new("Permission", "Permission2"),
            new(ClaimTypes.Role, RoleType.Admin.ToString())
        };

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"));
        var httpContext = new DefaultHttpContext { User = claimsPrincipal };
        _httpContextAccessor.HttpContext.Returns(httpContext);

        // Act
        var result = _sut.DecodeToken() as Dictionary<string, object>;
        result.Should().NotBeNull();

        // Assert
        result["AccountId"].Should().Be(claims[0].Value);
        result["Username"].Should().Be(claims[1].Value);
        result["Email"].Should().Be(claims[2].Value);
        result["Permission"].Should().BeEquivalentTo(s_expectation);
    }
}