using FluentAssertions;
using NSubstitute;
using OPS.Application.Interfaces.Auth;
using OPS.Application.Services;
using OPS.Domain.Entities.User;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Services;

public class AuthServiceTests
{
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IAuthService _sut;

    public AuthServiceTests()
    {
        _jwtGenerator = Substitute.For<IJwtGenerator>();
        _sut = new AuthService(_jwtGenerator);
    }

    [Fact]
    public void AuthenticateUser_WhenValidAccount_ShouldReturnAuthenticationResponse()
    {
        // Arrange
        var account = new Account
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            Username = "John",
            AccountRoles = new List<AccountRole> { new() { RoleId = (int)RoleType.Admin } },
        };

        var expectedToken = "test-jwt-token";
        _jwtGenerator.CreateToken(account).Returns(expectedToken);

        // Act
        var result = _sut.AuthenticateUser(account);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().Be(expectedToken);

        _jwtGenerator.Received(1).CreateToken(account);
    }
}