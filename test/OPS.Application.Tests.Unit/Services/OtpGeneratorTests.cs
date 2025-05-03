using FluentAssertions;
using OPS.Application.Services;

namespace OPS.Application.Tests.Unit.Services;

public class OtpGeneratorTests
{
    private readonly OtpGenerator _sut = new();

    [Fact]
    public void Generate_DefaultLength_Returns4DigitOtp()
    {
        // Act
        var result = _sut.Generate();

        // Assert
        result.Should().MatchRegex(@"^\d{4}$");
    }

    [Theory]
    [InlineData(4)]
    [InlineData(6)]
    [InlineData(8)]
    [InlineData(10)]
    public void Generate_ValidLength_ReturnsCorrectLengthOtp(int length)
    {
        // Act
        var result = _sut.Generate(length);

        // Assert
        result.Should().MatchRegex($"^\\d{{{length}}}$");
    }

    [Theory]
    [InlineData(3)]
    [InlineData(11)]
    public void Generate_InvalidLength_ThrowsArgumentOutOfRangeException(int length)
    {
        // Act
        var act = () => _sut.Generate(length);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*OTP length must be between 4 and 10.*");
    }

    [Fact]
    public void Generate_MultipleCalls_ReturnsDifferentOtps()
    {
        // Act
        var otp1 = _sut.Generate();
        var otp2 = _sut.Generate();

        // Assert
        otp1.Should().NotBe(otp2);
    }
}