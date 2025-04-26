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
using OPS.Domain.Entities.User;

namespace OPS.Application.Tests.Unit.Features.Authentication.Commands;

public class PasswordRecoveryCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAuthService _authService;
    private readonly ResetPasswordCommandHandler _sut;
    private readonly ResetPasswordCommandValidator _validator = new();
    private readonly Account _existingAccount;

    public PasswordRecoveryCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _authService = Substitute.For<IAuthService>();
        _sut = new ResetPasswordCommandHandler(_unitOfWork, _passwordHasher, _authService);

        _existingAccount = new Account
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            PasswordHash = "oldhash",
            Salt = "oldsalt"
        };
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldUpdatePasswordAndReturnAuthResponse()
    {
        // Arrange
        var command = new PasswordRecoveryCommand(
            "test@example.com",
            "NewPass123!",
            "1234"
        );

        _unitOfWork.Account.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(_existingAccount);

        _unitOfWork.Otp.IsValidOtpAsync(command.Email, command.Otp, Arg.Any<CancellationToken>())
            .Returns(true);

        _passwordHasher.HashPassword(command.NewPassword)
            .Returns(("newhash", "newsalt"));

        var authResponse = new AuthenticationResponse("token", _existingAccount.MapToDtoWithDetails()!);
        _authService.AuthenticateUser(Arg.Any<Account>())
            .Returns(authResponse);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(authResponse);
        _existingAccount.PasswordHash.Should().Be("newhash");
        _existingAccount.Salt.Should().Be("newsalt");

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenAccountNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new PasswordRecoveryCommand(
            "nonexistent@example.com",
            "NewPass123!",
            "1234"
        );

        _unitOfWork.Account.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns((Account?)null);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);

        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenInvalidOtp_ShouldReturnForbiddenError()
    {
        // Arrange
        var command = new PasswordRecoveryCommand(
            "test@example.com",
            "NewPass123!",
            "1234"
        );

        _unitOfWork.Account.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(_existingAccount);

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
        var command = new PasswordRecoveryCommand(
            "test@example.com",
            "NewPass123!",
            "1234"
        );

        _unitOfWork.Account.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(_existingAccount);

        _unitOfWork.Otp.IsValidOtpAsync(command.Email, command.Otp, Arg.Any<CancellationToken>())
            .Returns(true);

        _passwordHasher.HashPassword(command.NewPassword)
            .Returns(("newhash", "newsalt"));

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
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new PasswordRecoveryCommand(
            "test@example.com",
            "NewPass123!",
            "1234"
        );

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenEmailInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var command = new PasswordRecoveryCommand(
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
        var command = new PasswordRecoveryCommand(
            "test@example.com",
            "weak",
            "1234"
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.NewPassword);
    }

    [Fact]
    public void Validate_WhenOtpInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var command = new PasswordRecoveryCommand(
            "test@example.com",
            "NewPass123!",
            "123"
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.Otp);
    }
}