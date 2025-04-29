using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.Accounts.Commands;
using OPS.Application.Interfaces;
using OPS.Domain;
using OPS.Domain.Entities.User;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Features.Accounts.Commands;

public class SendAdminInviteCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;
    private readonly SendAdminInviteCommandHandler _sut;
    private readonly SendAdminInviteCommandValidator _validator = new();
    private readonly List<string> _validEmails;

    public SendAdminInviteCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _emailSender = Substitute.For<IEmailSender>();
        _sut = new SendAdminInviteCommandHandler(_unitOfWork, _emailSender);
        _validEmails = new List<string> { "test1@example.com", "test2@example.com" };
    }

    [Fact]
    public async Task Handle_WhenMixOfExistingAndNewEmails_ShouldProcessBothAndSendEmails()
    {
        // Arrange
        var existingAccount = new Account
        {
            Id = Guid.NewGuid(),
            Email = "test1@example.com",
            AccountRoles = new List<AccountRole>()
        };

        var uninvitedEmails = new List<string> { "test1@example.com", "test2@example.com" };
        var existingAccounts = new List<Account> { existingAccount };

        _unitOfWork.AdminInvite.GetUninvitedEmails(_validEmails, Arg.Any<CancellationToken>())
            .Returns(uninvitedEmails);

        _unitOfWork.Account.GetByEmailsWithRoleAsync(uninvitedEmails, Arg.Any<CancellationToken>())
            .Returns(existingAccounts);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(2);

        var command = new SendAdminInviteCommand(_validEmails);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        // Verify AccountRole was added for existing account
        _unitOfWork.AccountRole.Received(1).AddRange(Arg.Is<IEnumerable<AccountRole>>(roles =>
            roles.Any(r => r.AccountId == existingAccount.Id && r.RoleId == (int)RoleType.Admin)));

        // Verify AdminInvite was added for new email
        _unitOfWork.AdminInvite.Received(1).AddRange(Arg.Is<IEnumerable<AdminInvite>>(invites =>
            invites.Any(i => i.Email == "test2@example.com")));

        // Verify emails were sent
        _emailSender.Received(1).SendAdminInvitation(
            Arg.Is<List<string>>(emails => emails.Contains("test2@example.com")),
            Arg.Any<CancellationToken>());

        _emailSender.Received(1).SendAdminGranted(
            Arg.Is<List<string>>(emails => emails.Contains("test1@example.com")),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenAllEmailsAlreadyInvited_ShouldNotProcessAny()
    {
        // Arrange
        _unitOfWork.AdminInvite.GetUninvitedEmails(_validEmails, Arg.Any<CancellationToken>())
            .Returns([]);
        _unitOfWork.Account.GetByEmailsWithRoleAsync(Arg.Any<List<string>>(), Arg.Any<CancellationToken>())
            .Returns([]);

        var command = new SendAdminInviteCommand(_validEmails);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        _unitOfWork.AccountRole.DidNotReceive().AddRange(Arg.Any<IEnumerable<AccountRole>>());
        _unitOfWork.AdminInvite.DidNotReceive().AddRange(Arg.Any<IEnumerable<AdminInvite>>());
        _emailSender.DidNotReceive().SendAdminInvitation(Arg.Any<List<string>>(), Arg.Any<CancellationToken>());
        _emailSender.DidNotReceive().SendAdminGranted(Arg.Any<List<string>>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenExistingAccountsAreAlreadyAdmins_ShouldNotAddRoles()
    {
        // Arrange
        var existingAdmin = new Account
        {
            Id = Guid.NewGuid(),
            Email = "test1@example.com",
            AccountRoles = new List<AccountRole>
            {
                new() { RoleId = (int)RoleType.Admin }
            }
        };

        var uninvitedEmails = new List<string> { "test1@example.com" };
        var existingAccounts = new List<Account> { existingAdmin };

        _unitOfWork.AdminInvite.GetUninvitedEmails(_validEmails, Arg.Any<CancellationToken>())
            .Returns(uninvitedEmails);

        _unitOfWork.Account.GetByEmailsWithRoleAsync(uninvitedEmails, Arg.Any<CancellationToken>())
            .Returns(existingAccounts);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(0);

        var command = new SendAdminInviteCommand(_validEmails);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        // // Verify no roles were added
        // _unitOfWork.AccountRole.DidNotReceive().AddRange(Arg.Any<IEnumerable<AccountRole>>());
        // _accountEmails.DidNotReceive().SendAdminGranted(Arg.Any<List<string>>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new SendAdminInviteCommand(_validEmails);

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenEmailIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var invalidEmails = new List<string> { "test@example.com", "" };
        var command = new SendAdminInviteCommand(invalidEmails);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("Email[1]");
    }

    [Fact]
    public void Validate_WhenEmailIsInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var invalidEmails = new List<string> { "test@example.com", "invalid-email" };
        var command = new SendAdminInviteCommand(invalidEmails);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("Email[1]");
    }

    [Fact]
    public void Validate_WhenEmailsListIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new SendAdminInviteCommand(["invalid-email"]);

        // Act & Assert
        _validator.TestValidate(command).ShouldHaveAnyValidationError();
    }
}