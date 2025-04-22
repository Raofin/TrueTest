using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.Accounts.Commands;
using OPS.Application.Dtos;
using OPS.Domain;
using OPS.Domain.Entities.User;

namespace OPS.Application.Tests.Unit.Features.Accounts.Commands;

public class UpdateAccountCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UpdateAccountCommandHandler _sut;
    private readonly UpdateAccountCommandValidator _validator = new();
    private readonly Guid _validAccountId;
    private readonly string _validUsername;
    private readonly string _validEmail;

    public UpdateAccountCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new UpdateAccountCommandHandler(_unitOfWork);
        _validAccountId = Guid.NewGuid();
        _validUsername = "testuser";
        _validEmail = "test@example.com";
    }

    [Fact]
    public async Task Handle_WhenUpdatingUsername_ShouldUpdateAndReturnAccount()
    {
        // Arrange
        var account = new Account
        {
            Id = _validAccountId,
            Username = "oldusername",
            Email = _validEmail
        };

        _unitOfWork.Account.GetWithDetails(_validAccountId, Arg.Any<CancellationToken>())
            .Returns(account);

        _unitOfWork.Account.IsUsernameOrEmailUniqueAsync(_validUsername, null, Arg.Any<CancellationToken>())
            .Returns(true);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        var command = new UpdateAccountCommand(_validAccountId, _validUsername, null);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<AccountWithDetailsResponse>();
        account.Username.Should().Be(_validUsername);
        account.Email.Should().Be(_validEmail); // Email should remain unchanged
    }

    [Fact]
    public async Task Handle_WhenUpdatingEmail_ShouldUpdateAndReturnAccount()
    {
        // Arrange
        var account = new Account
        {
            Id = _validAccountId,
            Username = _validUsername,
            Email = "old@example.com"
        };

        _unitOfWork.Account.GetWithDetails(_validAccountId, Arg.Any<CancellationToken>())
            .Returns(account);

        _unitOfWork.Account.IsUsernameOrEmailUniqueAsync(null, _validEmail, Arg.Any<CancellationToken>())
            .Returns(true);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        var command = new UpdateAccountCommand(_validAccountId, null, _validEmail);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<AccountWithDetailsResponse>();
        account.Email.Should().Be(_validEmail);
        account.Username.Should().Be(_validUsername); // Username should remain unchanged
    }

    [Fact]
    public async Task Handle_WhenUpdatingBothUsernameAndEmail_ShouldUpdateAndReturnAccount()
    {
        // Arrange
        var account = new Account
        {
            Id = _validAccountId,
            Username = "oldusername",
            Email = "old@example.com"
        };

        _unitOfWork.Account.GetWithDetails(_validAccountId, Arg.Any<CancellationToken>())
            .Returns(account);

        _unitOfWork.Account.IsUsernameOrEmailUniqueAsync(_validUsername, _validEmail, Arg.Any<CancellationToken>())
            .Returns(true);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        var command = new UpdateAccountCommand(_validAccountId, _validUsername, _validEmail);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<AccountWithDetailsResponse>();
        account.Username.Should().Be(_validUsername);
        account.Email.Should().Be(_validEmail);
    }

    [Fact]
    public async Task Handle_WhenAccountNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        _unitOfWork.Account.GetWithDetails(_validAccountId, Arg.Any<CancellationToken>())
            .Returns((Account)null!);

        var command = new UpdateAccountCommand(_validAccountId, _validUsername, _validEmail);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task Handle_WhenUsernameOrEmailNotUnique_ShouldReturnConflictError()
    {
        // Arrange
        var account = new Account
        {
            Id = _validAccountId,
            Username = "oldusername",
            Email = "old@example.com"
        };

        _unitOfWork.Account.GetWithDetails(_validAccountId, Arg.Any<CancellationToken>())
            .Returns(account);

        _unitOfWork.Account.IsUsernameOrEmailUniqueAsync(_validUsername, _validEmail, Arg.Any<CancellationToken>())
            .Returns(false);

        var command = new UpdateAccountCommand(_validAccountId, _validUsername, _validEmail);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new UpdateAccountCommand(_validAccountId, _validUsername, _validEmail);

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenAccountIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateAccountCommand(Guid.Empty, _validUsername, _validEmail);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("AccountId");
    }

    [Fact]
    public void Validate_WhenUsernameIsTooShort_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateAccountCommand(_validAccountId, "abc", _validEmail);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("Username");
    }

    [Fact]
    public void Validate_WhenUsernameIsTooLong_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateAccountCommand(_validAccountId, new string('a', 51), _validEmail);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("Username");
    }

    [Fact]
    public void Validate_WhenEmailIsInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateAccountCommand(_validAccountId, _validUsername, "invalid-email");

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("Email");
    }
}