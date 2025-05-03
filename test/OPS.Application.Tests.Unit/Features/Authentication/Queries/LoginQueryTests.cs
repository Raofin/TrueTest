using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Dtos;
using OPS.Application.Features.Authentication.Queries;
using OPS.Application.Interfaces.Auth;
using OPS.Application.Mappers;
using OPS.Domain;
using OPS.Domain.Entities.User;

namespace OPS.Application.Tests.Unit.Features.Authentication.Queries;

public class LoginQueryTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAuthService _authService;
    private readonly LoginQueryHandler _sut;
    private readonly LoginQueryValidator _validator = new();
    private readonly Account _existingAccount;

    public LoginQueryTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _authService = Substitute.For<IAuthService>();
        _sut = new LoginQueryHandler(_unitOfWork, _passwordHasher, _authService);

        _existingAccount = new Account
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hash",
            Salt = "salt"
        };
    }

    [Fact]
    public async Task Handle_WhenValidCredentials_ShouldReturnAuthResponse()
    {
        // Arrange
        var query = new LoginQuery("testuser", "password123");
        var authResponse = new AuthenticationResponse("token", _existingAccount.MapToDtoWithDetails()!);

        _unitOfWork.Account.GetWithDetails(query.UsernameOrEmail, Arg.Any<CancellationToken>())
            .Returns(_existingAccount);

        _passwordHasher.VerifyPassword(_existingAccount.PasswordHash, _existingAccount.Salt, query.Password)
            .Returns(true);

        _authService.AuthenticateUser(_existingAccount)
            .Returns(authResponse);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(authResponse);

        await _unitOfWork.Account.Received(1).GetWithDetails(
            query.UsernameOrEmail,
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenAccountNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var query = new LoginQuery("nonexistent", "password123");

        _unitOfWork.Account.GetWithDetails(query.UsernameOrEmail, Arg.Any<CancellationToken>())
            .Returns((Account?)null);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);

        await _unitOfWork.Account.Received(1).GetWithDetails(
            query.UsernameOrEmail,
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenPasswordInvalid_ShouldReturnForbiddenError()
    {
        // Arrange
        var query = new LoginQuery("testuser", "wrongpassword");

        _unitOfWork.Account.GetWithDetails(query.UsernameOrEmail, Arg.Any<CancellationToken>())
            .Returns(_existingAccount);

        _passwordHasher.VerifyPassword(_existingAccount.PasswordHash, _existingAccount.Salt, query.Password)
            .Returns(false);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Forbidden);

        await _unitOfWork.Account.Received(1).GetWithDetails(
            query.UsernameOrEmail,
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenValidCredentials_ShouldCallAuthenticateUser()
    {
        // Arrange
        var query = new LoginQuery("testuser", "password123");
        var authResponse = new AuthenticationResponse("token", _existingAccount.MapToDtoWithDetails()!);

        _unitOfWork.Account.GetWithDetails(query.UsernameOrEmail, Arg.Any<CancellationToken>())
            .Returns(_existingAccount);

        _passwordHasher.VerifyPassword(_existingAccount.PasswordHash, _existingAccount.Salt, query.Password)
            .Returns(true);

        _authService.AuthenticateUser(_existingAccount)
            .Returns(authResponse);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(authResponse);

        _authService.Received(1).AuthenticateUser(_existingAccount);
    }

    [Fact]
    public void Validate_WhenValidQuery_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var query = new LoginQuery("testuser", "password123");

        // Act & Assert
        _validator.TestValidate(query).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenUsernameOrEmailIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var query = new LoginQuery("", "password123");

        // Act & Assert
        _validator.TestValidate(query)
            .ShouldHaveValidationErrorFor(x => x.UsernameOrEmail);
    }

    [Fact]
    public void Validate_WhenUsernameOrEmailIsTooShort_ShouldHaveValidationError()
    {
        // Arrange
        var query = new LoginQuery("usr", "password123");

        // Act & Assert
        _validator.TestValidate(query)
            .ShouldHaveValidationErrorFor(x => x.UsernameOrEmail);
    }

    [Fact]
    public void Validate_WhenPasswordIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var query = new LoginQuery("testuser", "");

        // Act & Assert
        _validator.TestValidate(query)
            .ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_WhenPasswordIsTooShort_ShouldHaveValidationError()
    {
        // Arrange
        var query = new LoginQuery("testuser", "pwd");

        // Act & Assert
        _validator.TestValidate(query)
            .ShouldHaveValidationErrorFor(x => x.Password);
    }
}