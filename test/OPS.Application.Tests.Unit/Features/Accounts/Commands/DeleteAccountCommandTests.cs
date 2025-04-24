using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.Accounts.Commands;
using OPS.Domain;
using OPS.Domain.Entities.User;

namespace OPS.Application.Tests.Unit.Features.Accounts.Commands;

public class DeleteAccountCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteAccountCommandHandler _sut;
    private readonly DeleteAccountCommandValidator _validator = new();
    private readonly Guid _validAccountId;

    public DeleteAccountCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new DeleteAccountCommandHandler(_unitOfWork);
        _validAccountId = Guid.NewGuid();
    }

    [Fact]
    public async Task Handle_WhenAccountExistsAndNotProtected_ShouldDeleteAccount()
    {
        // Arrange
        var account = new Account
        {
            Id = _validAccountId,
            Username = "testuser"
        };

        _unitOfWork.Account.GetAsync(_validAccountId, Arg.Any<CancellationToken>())
            .Returns(account);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        var command = new DeleteAccountCommand(_validAccountId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);
        _unitOfWork.Account.Received(1).Remove(account);
    }

    [Fact]
    public async Task Handle_WhenAccountNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        _unitOfWork.Account.GetAsync(_validAccountId, Arg.Any<CancellationToken>())
            .Returns((Account)null!);

        var command = new DeleteAccountCommand(_validAccountId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task Handle_WhenAccountIsProtected_ShouldReturnConflictError()
    {
        // Arrange
        var account = new Account
        {
            Id = _validAccountId,
            Username = "rawfin"
        };

        _unitOfWork.Account.GetAsync(_validAccountId, Arg.Any<CancellationToken>())
            .Returns(account);

        var command = new DeleteAccountCommand(_validAccountId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
    }

    [Fact]
    public async Task Handle_WhenCommitFails_ShouldReturnUnexpectedError()
    {
        // Arrange
        var account = new Account { Id = _validAccountId, Username = "testuser" };
        _unitOfWork.Account.GetAsync(_validAccountId, Arg.Any<CancellationToken>())
            .Returns(account);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(0);

        var command = new DeleteAccountCommand(_validAccountId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new DeleteAccountCommand(_validAccountId);

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenAccountIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new DeleteAccountCommand(Guid.Empty);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("AccountId");
    }
}