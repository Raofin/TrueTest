using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.Accounts.Commands;
using OPS.Domain;
using OPS.Domain.Entities.User;

namespace OPS.Application.Tests.Unit.Features.Accounts.Commands;

public class ChangeActiveStatusCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ChangeActiveStatusCommandHandler _sut;
    private readonly ChangeActiveStatusValidator _validator = new();
    private readonly Guid _validAccountId;

    public ChangeActiveStatusCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new ChangeActiveStatusCommandHandler(_unitOfWork);
        _validAccountId = Guid.NewGuid();
    }

    [Fact]
    public async Task Handle_WhenAccountExists_ShouldToggleActiveStatus()
    {
        // Arrange
        var account = new Account
        {
            Id = _validAccountId,
            IsActive = true
        };

        _unitOfWork.Account.GetAsync(_validAccountId, Arg.Any<CancellationToken>())
            .Returns(account);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        var command = new ChangeActiveStatusCommand(_validAccountId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.IsActive.Should().BeFalse();
        account.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WhenAccountNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        _unitOfWork.Account.GetAsync(_validAccountId, Arg.Any<CancellationToken>())
            .Returns((Account)null!);

        var command = new ChangeActiveStatusCommand(_validAccountId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task Handle_WhenCommitFails_ShouldReturnUnexpectedError()
    {
        // Arrange
        var account = new Account { Id = _validAccountId };
        _unitOfWork.Account.GetAsync(_validAccountId, Arg.Any<CancellationToken>())
            .Returns(account);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(0);

        var command = new ChangeActiveStatusCommand(_validAccountId);

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
        var command = new ChangeActiveStatusCommand(_validAccountId);

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenAccountIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new ChangeActiveStatusCommand(Guid.Empty);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("AccountId");
    }
}