using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.Accounts.Commands;
using OPS.Domain;
using OPS.Domain.Entities.User;

namespace OPS.Application.Tests.Unit.Features.Accounts.Commands;

public class UpdateAccountCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UpdateAccountCommandHandler _sut;
    private readonly UpdateAccountCommandValidator _validator = new();
    private readonly Guid _accountId;

    public UpdateAccountCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new UpdateAccountCommandHandler(_unitOfWork);
        _accountId = Guid.NewGuid();
    }

    [Fact]
    public async Task Handle_WhenUpdatingUsername_ShouldUpdateAndReturnAccount()
    {
        // Arrange
        var existingAccount = new Account
        {
            Id = _accountId,
            Username = "oldusername",
            Email = "old@email.com"
        };

        _unitOfWork.Account.GetWithDetails(_accountId, Arg.Any<CancellationToken>())
            .Returns(existingAccount);

        _unitOfWork.Account.IsUsernameOrEmailUniqueAsync("newusername", null, Arg.Any<CancellationToken>())
            .Returns(true);

        var command = new UpdateAccountCommand(_accountId, "newusername", null);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Username.Should().Be("newusername");
        result.Value.Email.Should().Be("old@email.com");

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenUpdatingEmail_ShouldUpdateAndReturnAccount()
    {
        // Arrange
        var existingAccount = new Account
        {
            Id = _accountId,
            Username = "username",
            Email = "old@email.com"
        };

        _unitOfWork.Account.GetWithDetails(_accountId, Arg.Any<CancellationToken>())
            .Returns(existingAccount);

        _unitOfWork.Account.IsUsernameOrEmailUniqueAsync(null, "new@email.com", Arg.Any<CancellationToken>())
            .Returns(true);

        var command = new UpdateAccountCommand(_accountId, null, "new@email.com");

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Username.Should().Be("username");
        result.Value.Email.Should().Be("new@email.com");

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenAccountNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        _unitOfWork.Account.GetWithDetails(_accountId, Arg.Any<CancellationToken>())
            .Returns((Account?)null);

        var command = new UpdateAccountCommand(_accountId, "newusername", null);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);

        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenUsernameOrEmailNotUnique_ShouldReturnConflictError()
    {
        // Arrange
        var existingAccount = new Account
        {
            Id = _accountId,
            Username = "username",
            Email = "email@email.com"
        };

        _unitOfWork.Account.GetWithDetails(_accountId, Arg.Any<CancellationToken>())
            .Returns(existingAccount);

        _unitOfWork.Account.IsUsernameOrEmailUniqueAsync("newusername", null, Arg.Any<CancellationToken>())
            .Returns(false);

        var command = new UpdateAccountCommand(_accountId, "newusername", null);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);

        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new UpdateAccountCommand(_accountId, "username", "email@email.com");

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenAccountIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateAccountCommand(Guid.Empty, "username", "email@email.com");

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.AccountId);
    }

    [Fact]
    public void Validate_WhenUsernameTooShort_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateAccountCommand(_accountId, "abc", "email@email.com");

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void Validate_WhenEmailInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateAccountCommand(_accountId, "username", "invalid-email");

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.Email);
    }
}