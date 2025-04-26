using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.User.Commands;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;
using OPS.Domain.Entities.User;

namespace OPS.Application.Tests.Unit.Features.User.Commands;

public class UpdateAccountSettingsCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserInfoProvider _userInfoProvider;
    private readonly IPasswordHasher _passwordHasher;
    private readonly UpdateAccountSettingsCommandHandler _sut;
    private readonly UpdateAccountSettingsCommandValidator _validator = new();
    private readonly Guid _accountId;

    public UpdateAccountSettingsCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _userInfoProvider = Substitute.For<IUserInfoProvider>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _sut = new UpdateAccountSettingsCommandHandler(_userInfoProvider, _passwordHasher, _unitOfWork);
        _accountId = Guid.NewGuid();

        _userInfoProvider.AccountId().Returns(_accountId);
    }

    [Fact]
    public async Task Handle_WhenUpdatingUsername_ShouldUpdateAndReturnAccount()
    {
        // Arrange
        var existingAccount = new Account
        {
            Id = _accountId,
            Username = "oldusername",
            PasswordHash = "hash",
            Salt = "salt"
        };

        _unitOfWork.Account.GetWithDetails(_accountId, Arg.Any<CancellationToken>())
            .Returns(existingAccount);

        _unitOfWork.Account.IsUsernameOrEmailUniqueAsync("newusername", null, Arg.Any<CancellationToken>())
            .Returns(true);

        var command = new UpdateAccountSettingsCommand("newusername", null, null);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Username.Should().Be("newusername");

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenUpdatingPassword_ShouldUpdateAndReturnAccount()
    {
        // Arrange
        var existingAccount = new Account
        {
            Id = _accountId,
            Username = "username",
            PasswordHash = "oldhash",
            Salt = "oldsalt"
        };

        _unitOfWork.Account.GetWithDetails(_accountId, Arg.Any<CancellationToken>())
            .Returns(existingAccount);

        _passwordHasher.VerifyPassword("oldhash", "oldsalt", "currentpassword")
            .Returns(true);

        _passwordHasher.HashPassword("newpassword")
            .Returns(("newhash", "newsalt"));

        var command = new UpdateAccountSettingsCommand(null, "newpassword", "currentpassword");

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        existingAccount.PasswordHash.Should().Be("newhash");
        existingAccount.Salt.Should().Be("newsalt");

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenUpdatingBothUsernameAndPassword_ShouldUpdateAndReturnAccount()
    {
        // Arrange
        var existingAccount = new Account
        {
            Id = _accountId,
            Username = "oldusername",
            PasswordHash = "oldhash",
            Salt = "oldsalt"
        };

        _unitOfWork.Account.GetWithDetails(_accountId, Arg.Any<CancellationToken>())
            .Returns(existingAccount);

        _unitOfWork.Account.IsUsernameOrEmailUniqueAsync("newusername", null, Arg.Any<CancellationToken>())
            .Returns(true);

        _passwordHasher.VerifyPassword("oldhash", "oldsalt", "currentpassword")
            .Returns(true);

        _passwordHasher.HashPassword("newpassword")
            .Returns(("newhash", "newsalt"));

        var command = new UpdateAccountSettingsCommand("newusername", "newpassword", "currentpassword");

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Username.Should().Be("newusername");
        existingAccount.PasswordHash.Should().Be("newhash");
        existingAccount.Salt.Should().Be("newsalt");

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenUsernameNotUnique_ShouldReturnConflictError()
    {
        // Arrange
        var existingAccount = new Account
        {
            Id = _accountId,
            Username = "oldusername",
            PasswordHash = "hash",
            Salt = "salt"
        };

        _unitOfWork.Account.GetWithDetails(_accountId, Arg.Any<CancellationToken>())
            .Returns(existingAccount);

        _unitOfWork.Account.IsUsernameOrEmailUniqueAsync("newusername", null, Arg.Any<CancellationToken>())
            .Returns(false);

        var command = new UpdateAccountSettingsCommand("newusername", null, null);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
        result.FirstError.Description.Should().Be("Username is already taken");

        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenCurrentPasswordInvalid_ShouldReturnUnauthorizedError()
    {
        // Arrange
        var existingAccount = new Account
        {
            Id = _accountId,
            Username = "username",
            PasswordHash = "hash",
            Salt = "salt"
        };

        _unitOfWork.Account.GetWithDetails(_accountId, Arg.Any<CancellationToken>())
            .Returns(existingAccount);

        _passwordHasher.VerifyPassword("hash", "salt", "wrongpassword")
            .Returns(false);

        var command = new UpdateAccountSettingsCommand(null, "newpassword", "wrongpassword");

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Forbidden);
        result.FirstError.Description.Should().Be("Invalid current password");

        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new UpdateAccountSettingsCommand("username", "NewPass123!", "CurrentPass123!");

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenUsernameTooShort_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateAccountSettingsCommand("abc", null, null);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void Validate_WhenNewPasswordInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateAccountSettingsCommand(null, "weak", "CurrentPass123!");

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.NewPassword);
    }

    [Fact]
    public void Validate_WhenNewPasswordWithoutCurrentPassword_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateAccountSettingsCommand(null, "NewPass123!", null);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.CurrentPassword);
    }
}