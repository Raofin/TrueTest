using ErrorOr;
using FluentAssertions;
using NSubstitute;
using OPS.Application.Features.User.Queries;
using OPS.Application.Interfaces.Auth;
using OPS.Domain;
using OPS.Domain.Entities.User;

namespace OPS.Application.Tests.Unit.Features.User.Queries;

public class GetUserDetailsQueryTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserProvider _userProvider;
    private readonly GetUserDetailsQueryHandler _sut;
    private readonly Guid _accountId;

    public GetUserDetailsQueryTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _userProvider = Substitute.For<IUserProvider>();
        _sut = new GetUserDetailsQueryHandler(_unitOfWork, _userProvider);
        _accountId = Guid.NewGuid();

        _userProvider.AccountId().Returns(_accountId);
    }

    [Fact]
    public async Task Handle_WhenAccountExists_ShouldReturnAccountDetails()
    {
        // Arrange
        var account = new Account
        {
            Id = _accountId,
            Username = "testuser",
            Email = "test@email.com",
            Profile = new Profile
            {
                FirstName = "John",
                LastName = "Doe",
                Bio = "Test Bio",
                InstituteName = "Test Institute",
                PhoneNumber = "1234567890",
                ProfileLinks = new List<ProfileLinks>
                {
                    new() { Name = "GitHub", Link = "https://github.com" }
                }
            }
        };

        _unitOfWork.Account.GetWithDetails(_accountId, Arg.Any<CancellationToken>())
            .Returns(account);

        var query = new GetUserDetailsQuery();

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Username.Should().Be("testuser");
        result.Value.Email.Should().Be("test@email.com");
        result.Value.Profile.Should().NotBeNull();
        result.Value.Profile.FirstName.Should().Be("John");
        result.Value.Profile.LastName.Should().Be("Doe");
        result.Value.Profile.BioMarkdown.Should().Be("Test Bio");
        result.Value.Profile.InstituteName.Should().Be("Test Institute");
        result.Value.Profile.PhoneNumber.Should().Be("1234567890");
        result.Value.Profile.ProfileLinks.Should().HaveCount(1);
    }

    [Fact]
    public async Task Handle_WhenAccountNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        _unitOfWork.Account.GetWithDetails(_accountId, Arg.Any<CancellationToken>())
            .Returns((Account?)null);

        var query = new GetUserDetailsQuery();

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task Handle_WhenAccountHasNoProfile_ShouldReturnAccountWithNullProfile()
    {
        // Arrange
        var account = new Account
        {
            Id = _accountId,
            Username = "testuser",
            Email = "test@email.com"
        };

        _unitOfWork.Account.GetWithDetails(_accountId, Arg.Any<CancellationToken>())
            .Returns(account);

        var query = new GetUserDetailsQuery();

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Username.Should().Be("testuser");
        result.Value.Email.Should().Be("test@email.com");
        result.Value.Profile.Should().BeNull();
    }
}