using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.Authentication.Queries;
using OPS.Domain;

namespace OPS.Application.Tests.Unit.Features.Authentication.Queries;

public class IsValidOtpQueryTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly VerifyOtpQueryHandler _sut;
    private readonly IsValidOtpQueryValidator _validator = new();

    public IsValidOtpQueryTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new VerifyOtpQueryHandler(_unitOfWork);
    }

    [Fact]
    public async Task Handle_WhenOtpIsValid_ShouldReturnTrue()
    {
        // Arrange
        var query = new IsValidOtpQuery("test@example.com", "1234");

        _unitOfWork.Otp.IsValidOtpAsync(query.Email, query.Otp, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeTrue();

        await _unitOfWork.Otp.Received(1).IsValidOtpAsync(
            query.Email,
            query.Otp,
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenOtpIsInvalid_ShouldReturnFalse()
    {
        // Arrange
        var query = new IsValidOtpQuery("test@example.com", "1234");

        _unitOfWork.Otp.IsValidOtpAsync(query.Email, query.Otp, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeFalse();

        await _unitOfWork.Otp.Received(1).IsValidOtpAsync(
            query.Email,
            query.Otp,
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Validate_WhenValidQuery_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var query = new IsValidOtpQuery("test@example.com", "1234");

        // Act & Assert
        _validator.TestValidate(query).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenEmailIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var query = new IsValidOtpQuery("", "1234");

        // Act & Assert
        _validator.TestValidate(query)
            .ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_WhenEmailIsInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var query = new IsValidOtpQuery("invalid-email", "1234");

        // Act & Assert
        _validator.TestValidate(query)
            .ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_WhenOtpIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var query = new IsValidOtpQuery("test@example.com", "");

        // Act & Assert
        _validator.TestValidate(query)
            .ShouldHaveValidationErrorFor(x => x.Otp);
    }

    [Fact]
    public void Validate_WhenOtpLengthIsInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var query = new IsValidOtpQuery("test@example.com", "123");

        // Act & Assert
        _validator.TestValidate(query)
            .ShouldHaveValidationErrorFor(x => x.Otp);
    }
}