using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.Accounts.Commands;
using OPS.Domain;
using OPS.Domain.Contracts.Core.EmailSender;
using OPS.Domain.Entities.User;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Features.Accounts.Commands;

public class MakeAdminCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccountEmails _accountEmails;
    private readonly MakeAdminCommandHandler _sut;
    private readonly MakeAdminCommandValidator _validator = new();
    private readonly List<Guid> _validAccountIds;

    public MakeAdminCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _accountEmails = Substitute.For<IAccountEmails>();
        _sut = new MakeAdminCommandHandler(_unitOfWork, _accountEmails);
        _validAccountIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
    }

    [Fact]
    public async Task Handle_WhenNonAdminAccountsExist_ShouldMakeThemAdminAndSendEmails()
    {
        // Arrange
        var accounts = new List<Account>
        {
            new() { Id = _validAccountIds[0], Email = "test1@example.com" },
            new() { Id = _validAccountIds[1], Email = "test2@example.com" }
        };

        _unitOfWork.Account.GetNonAdminAccounts(_validAccountIds, Arg.Any<CancellationToken>())
            .Returns(accounts);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(2);

        var command = new MakeAdminCommand(_validAccountIds);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        // Verify AccountRole was added for each account
        _unitOfWork.AccountRole.Received(2).Add(Arg.Is<AccountRole>(ar =>
            ar.RoleId == (int)RoleType.Admin &&
            (_validAccountIds.Contains(ar.AccountId))));

        // Verify emails were sent
        _accountEmails.Received(1).SendAdminGranted(
            Arg.Is<List<string>>(emails =>
                emails.Count == 2 &&
                emails.Contains("test1@example.com") &&
                emails.Contains("test2@example.com")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenNoNonAdminAccountsFound_ShouldReturnNotFoundError()
    {
        // Arrange
        _unitOfWork.Account.GetNonAdminAccounts(_validAccountIds, Arg.Any<CancellationToken>())
            .Returns(new List<Account>());

        var command = new MakeAdminCommand(_validAccountIds);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Description.Should().Be("No non-admin accounts found.");

        // Verify no emails were sent
        _accountEmails.DidNotReceive().SendAdminGranted(
            Arg.Any<List<string>>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenCommitFails_ShouldReturnUnexpectedError()
    {
        // Arrange
        var accounts = new List<Account>
        {
            new() { Id = _validAccountIds[0], Email = "test@example.com" }
        };

        _unitOfWork.Account.GetNonAdminAccounts(_validAccountIds, Arg.Any<CancellationToken>())
            .Returns(accounts);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(0);

        var command = new MakeAdminCommand(_validAccountIds);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);

        // // Verify no emails were sent
        // _accountEmails.DidNotReceive().SendAdminGranted(
        //     Arg.Any<List<string>>(),
        //     Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new MakeAdminCommand(_validAccountIds);

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenAccountIdsContainsEmptyGuid_ShouldHaveValidationError()
    {
        // Arrange
        var invalidAccountIds = new List<Guid> { Guid.NewGuid(), Guid.Empty };
        var command = new MakeAdminCommand(invalidAccountIds);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("AccountIds[1]");
    }

    [Fact]
    public void Validate_WhenAccountIdsIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new MakeAdminCommand([Guid.Empty]);

        // Act & Assert
        _validator.TestValidate(command).ShouldHaveAnyValidationError();
    }
}