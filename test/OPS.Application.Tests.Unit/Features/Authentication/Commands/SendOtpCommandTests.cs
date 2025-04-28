using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.Authentication.Commands;
using OPS.Application.Interfaces;
using OPS.Application.Interfaces.Auth;
using OPS.Domain;
using OPS.Domain.Entities.User;

namespace OPS.Application.Tests.Unit.Features.Authentication.Commands;

public class SendOtpCommandTests
{
    private readonly IEmailSender _emailSender;
    private readonly IOtpGenerator _otpGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SendOtpCommandHandler _sut;
    private readonly SendOtpCommandValidator _validator = new();
    private readonly Otp _existingOtp;

    public SendOtpCommandTests()
    {
        _emailSender = Substitute.For<IEmailSender>();
        _otpGenerator = Substitute.For<IOtpGenerator>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new SendOtpCommandHandler(_emailSender, _otpGenerator, _unitOfWork);

        _existingOtp = new Otp
        {
            Email = "test@example.com",
            Code = "1234",
            ExpiresAt = DateTime.UtcNow.AddMinutes(5)
        };
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldGenerateAndSendOtp()
    {
        // Arrange
        var command = new SendOtpCommand("test@example.com");
        var newOtpCode = "5678";

        _otpGenerator.Generate().Returns(newOtpCode);
        _unitOfWork.Otp.GetOtpAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns((Otp?)null);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        // result.Value.Should().Be(Unit.Value);

        _unitOfWork.Otp.Received(1).Add(Arg.Is<Otp>(o =>
            o.Email == command.Email &&
            o.Code == newOtpCode &&
            o.ExpiresAt > DateTime.UtcNow));

        _emailSender.Received(1).SendOtp(command.Email, newOtpCode, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenExistingOtpExists_ShouldRemoveAndCreateNewOtp()
    {
        // Arrange
        var command = new SendOtpCommand("test@example.com");
        var newOtpCode = "5678";

        _otpGenerator.Generate().Returns(newOtpCode);
        _unitOfWork.Otp.GetOtpAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(_existingOtp);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        // result.Value.Should().Be(Unit.Value);

        _unitOfWork.Otp.Received(1).Remove(_existingOtp);
        _unitOfWork.Otp.Received(1).Add(Arg.Is<Otp>(o =>
            o.Email == command.Email &&
            o.Code == newOtpCode &&
            o.ExpiresAt > DateTime.UtcNow));

        _emailSender.Received(1).SendOtp(command.Email, newOtpCode, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new SendOtpCommand("test@example.com");

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenEmailIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new SendOtpCommand("");

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_WhenEmailIsInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var command = new SendOtpCommand("invalid-email");

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.Email);
    }
}