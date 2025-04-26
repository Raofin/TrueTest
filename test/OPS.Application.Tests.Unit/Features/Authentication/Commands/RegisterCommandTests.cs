using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Dtos;
using OPS.Application.Features.Authentication.Commands;
using OPS.Application.Mappers;
using OPS.Application.Services.AuthService;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.User;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Features.Authentication.Commands;

public class RegisterCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAuthService _authService;
    private readonly RegisterCommandHandler _sut;
    private readonly RegisterCommandValidator _validator = new();
    private readonly Account _newAccount;
    private readonly AdminInvite _adminInvite;
    private readonly List<ExamCandidate> _examInvites;

    public RegisterCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _authService = Substitute.For<IAuthService>();
        _sut = new RegisterCommandHandler(_unitOfWork, _passwordHasher, _authService);

        _newAccount = new Account
        {
            Id = Guid.NewGuid(),
            Username = "newuser",
            Email = "new@example.com",
            PasswordHash = "hash",
            Salt = "salt"
        };

        _adminInvite = new AdminInvite
        {
            Id = Guid.NewGuid(),
            Email = "new@example.com"
        };

        _examInvites = new List<ExamCandidate>
        {
            new() { Id = Guid.NewGuid(), CandidateEmail = "new@example.com" },
            new() { Id = Guid.NewGuid(), CandidateEmail = "new@example.com" }
        };
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldCreateAccountAndReturnAuthResponse()
    {
        // Arrange
        var command = new RegisterCommand(
            "newuser",
            "new@example.com",
            "NewPass123!",
            "1234"
        );

        _unitOfWork.Account.IsUsernameOrEmailUniqueAsync(command.Username, command.Email, Arg.Any<CancellationToken>())
            .Returns(true);

        _unitOfWork.Otp.IsValidOtpAsync(command.Email, command.Otp, Arg.Any<CancellationToken>())
            .Returns(true);

        _passwordHasher.HashPassword(command.Password)
            .Returns(("hash", "salt"));

        var authResponse = new AuthenticationResponse("token", _newAccount.MapToDtoWithDetails()!);
        _authService.AuthenticateUser(Arg.Any<Account>())
            .Returns(authResponse);

        _unitOfWork.ExamCandidate.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns([]);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(authResponse);

        _unitOfWork.Account.Received(1).Add(Arg.Is<Account>(a =>
            a.Username == command.Username &&
            a.Email == command.Email &&
            a.PasswordHash == "hash" &&
            a.Salt == "salt" &&
            a.AccountRoles.Count == 1 &&
            a.AccountRoles.First().RoleId == (int)RoleType.Candidate));

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenUsernameOrEmailExists_ShouldReturnConflictError()
    {
        // Arrange
        var command = new RegisterCommand(
            "existinguser",
            "existing@example.com",
            "NewPass123!",
            "1234"
        );

        _unitOfWork.Account.IsUsernameOrEmailUniqueAsync(command.Username, command.Email, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);

        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenInvalidOtp_ShouldReturnForbiddenError()
    {
        // Arrange
        var command = new RegisterCommand(
            "newuser",
            "new@example.com",
            "NewPass123!",
            "1234"
        );

        _unitOfWork.Account.IsUsernameOrEmailUniqueAsync(command.Username, command.Email, Arg.Any<CancellationToken>())
            .Returns(true);

        _unitOfWork.Otp.IsValidOtpAsync(command.Email, command.Otp, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Forbidden);
        result.FirstError.Description.Should().Be("Invalid OTP.");

        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenCommitFails_ShouldReturnUnexpectedError()
    {
        // Arrange
        var command = new RegisterCommand(
            "newuser",
            "new@example.com",
            "NewPass123!",
            "1234"
        );

        _unitOfWork.Account.IsUsernameOrEmailUniqueAsync(command.Username, command.Email, Arg.Any<CancellationToken>())
            .Returns(true);

        _unitOfWork.Otp.IsValidOtpAsync(command.Email, command.Otp, Arg.Any<CancellationToken>())
            .Returns(true);

        _passwordHasher.HashPassword(command.Password)
            .Returns(("hash", "salt"));

        _unitOfWork.ExamCandidate.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns([]);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(0);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenAdminInviteExists_ShouldAddAdminRole()
    {
        // Arrange
        var command = new RegisterCommand(
            "newuser",
            "new@example.com",
            "NewPass123!",
            "1234"
        );

        _unitOfWork.Account.IsUsernameOrEmailUniqueAsync(command.Username, command.Email, Arg.Any<CancellationToken>())
            .Returns(true);

        _unitOfWork.Otp.IsValidOtpAsync(command.Email, command.Otp, Arg.Any<CancellationToken>())
            .Returns(true);

        _unitOfWork.AdminInvite.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(_adminInvite);

        _unitOfWork.ExamCandidate.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns([]);

        _passwordHasher.HashPassword(command.Password)
            .Returns(("hash", "salt"));

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();

        _unitOfWork.AdminInvite.Received(1).Remove(_adminInvite);
        _unitOfWork.AccountRole.Received(1).Add(Arg.Is<AccountRole>(ar =>
            ar.RoleId == (int)RoleType.Admin));
    }

    [Fact]
    public async Task Handle_WhenExamInvitesExist_ShouldUpdateExamInvites()
    {
        // Arrange
        var command = new RegisterCommand(
            "newuser",
            "new@example.com",
            "NewPass123!",
            "1234"
        );

        _unitOfWork.Account.IsUsernameOrEmailUniqueAsync(command.Username, command.Email, Arg.Any<CancellationToken>())
            .Returns(true);

        _unitOfWork.Otp.IsValidOtpAsync(command.Email, command.Otp, Arg.Any<CancellationToken>())
            .Returns(true);

        _unitOfWork.ExamCandidate.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(_examInvites);

        _passwordHasher.HashPassword(command.Password)
            .Returns(("hash", "salt"));

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new RegisterCommand(
            "newuser",
            "new@example.com",
            "NewPass123!",
            "1234"
        );

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenUsernameInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var command = new RegisterCommand(
            "usr",
            "new@example.com",
            "NewPass123!",
            "1234"
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void Validate_WhenEmailInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var command = new RegisterCommand(
            "newuser",
            "invalid-email",
            "NewPass123!",
            "1234"
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_WhenPasswordInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var command = new RegisterCommand(
            "newuser",
            "new@example.com",
            "weak",
            "1234"
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_WhenOtpInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var command = new RegisterCommand(
            "newuser",
            "new@example.com",
            "NewPass123!",
            "123"
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.Otp);
    }
}