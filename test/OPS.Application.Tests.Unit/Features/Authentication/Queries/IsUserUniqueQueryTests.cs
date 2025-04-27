using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.Authentication.Queries;
using OPS.Domain;

namespace OPS.Application.Tests.Unit.Features.Authentication.Queries;

public class IsUserUniqueQueryTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IsUserUniqueQueryHandler _sut;
    private readonly IsUserUniqueQueryValidator _validator = new();

    public IsUserUniqueQueryTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new IsUserUniqueQueryHandler(_unitOfWork);
    }

    [Fact]
    public async Task Handle_WhenUsernameAndEmailAreUnique_ShouldReturnTrue()
    {
        // Arrange
        var query = new IsUserUniqueQuery("newuser", "new@example.com");

        _unitOfWork.Account.IsUsernameOrEmailUniqueAsync(query.Username, query.Email, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeTrue();

        await _unitOfWork.Account.Received(1).IsUsernameOrEmailUniqueAsync(
            query.Username,
            query.Email,
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenUsernameOrEmailExists_ShouldReturnFalse()
    {
        // Arrange
        var query = new IsUserUniqueQuery("existinguser", "existing@example.com");

        _unitOfWork.Account.IsUsernameOrEmailUniqueAsync(query.Username, query.Email, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeFalse();

        await _unitOfWork.Account.Received(1).IsUsernameOrEmailUniqueAsync(
            query.Username,
            query.Email,
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenOnlyUsernameProvided_ShouldCheckUsernameUniqueness()
    {
        // Arrange
        var query = new IsUserUniqueQuery("newuser", null);

        _unitOfWork.Account.IsUsernameOrEmailUniqueAsync(query.Username, null, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeTrue();

        await _unitOfWork.Account.Received(1).IsUsernameOrEmailUniqueAsync(
            query.Username,
            null,
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenOnlyEmailProvided_ShouldCheckEmailUniqueness()
    {
        // Arrange
        var query = new IsUserUniqueQuery(null, "new@example.com");

        _unitOfWork.Account.IsUsernameOrEmailUniqueAsync(null, query.Email, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeTrue();

        await _unitOfWork.Account.Received(1).IsUsernameOrEmailUniqueAsync(
            null,
            query.Email,
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Validate_WhenValidQuery_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var query = new IsUserUniqueQuery("newuser", "new@example.com");

        // Act & Assert
        _validator.TestValidate(query).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenUsernameTooShort_ShouldHaveValidationError()
    {
        // Arrange
        var query = new IsUserUniqueQuery("usr", "new@example.com");

        // Act & Assert
        _validator.TestValidate(query)
            .ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void Validate_WhenUsernameTooLong_ShouldHaveValidationError()
    {
        // Arrange
        var query = new IsUserUniqueQuery(new string('a', 51), "new@example.com");

        // Act & Assert
        _validator.TestValidate(query)
            .ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void Validate_WhenEmailInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var query = new IsUserUniqueQuery("newuser", "invalid-email");

        // Act & Assert
        _validator.TestValidate(query)
            .ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_WhenBothUsernameAndEmailAreNull_ShouldHaveValidationError()
    {
        // Arrange
        var query = new IsUserUniqueQuery(null, null);

        // Act & Assert
        _validator.TestValidate(query)
            .ShouldHaveValidationErrorFor("UsernameOrEmail");
    }
}