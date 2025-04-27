using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Dtos;
using OPS.Application.Features.User.Commands;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;
using OPS.Domain.Entities.User;

namespace OPS.Application.Tests.Unit.Features.User.Commands;

public class SaveProfileCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserInfoProvider _userInfoProvider;
    private readonly SaveProfileCommandHandler _sut;
    private readonly SaveProfileCommandValidator _validator = new();
    private readonly Guid _accountId;
    private readonly Guid _imageFileId;

    public SaveProfileCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _userInfoProvider = Substitute.For<IUserInfoProvider>();
        _sut = new SaveProfileCommandHandler(_unitOfWork, _userInfoProvider);
        _accountId = Guid.NewGuid();
        _imageFileId = Guid.NewGuid();

        _userInfoProvider.AccountId().Returns(_accountId);
    }

    [Fact]
    public async Task Handle_WhenCreatingNewProfile_ShouldCreateAndReturnProfile()
    {
        // Arrange
        var profileLinks = new List<ProfileLinkRequest>
        {
            new(Guid.NewGuid(), "GitHub", "https://github.com/raofin")
        };

        var command = new SaveProfileCommand(
            "John",
            "Doe",
            "Test Bio",
            "Test Institute",
            "1234567890",
            _imageFileId,
            profileLinks
        );

        _unitOfWork.CloudFile.IsExistsAsync(_imageFileId, Arg.Any<CancellationToken>())
            .Returns(true);

        _unitOfWork.Profile.GetByAccountId(_accountId, Arg.Any<CancellationToken>())
            .Returns((Profile?)null);

        _unitOfWork.ProfileLink.GetAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(new ProfileLinks());

        _unitOfWork.Profile.GetByAccountId(_accountId, Arg.Any<CancellationToken>())
            .Returns(null as Profile);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();

        _unitOfWork.Profile.Received(1).Add(Arg.Any<Profile>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenUpdatingExistingProfile_ShouldUpdateAndReturnProfile()
    {
        // Arrange
        var existingProfile = new Profile
        {
            Id = Guid.NewGuid(),
            AccountId = _accountId,
            FirstName = "Old",
            LastName = "Name",
            Bio = "Old Bio",
            InstituteName = "Old Institute",
            PhoneNumber = "0000000000",
            ProfileLinks = new List<ProfileLinks>()
        };

        var profileLinks = new List<ProfileLinkRequest>
        {
            new(Guid.NewGuid(), "GitHub", "https://github.com/raofin")
        };

        var command = new SaveProfileCommand(
            "John",
            "Doe",
            "Test Bio",
            "Test Institute",
            "1234567890",
            _imageFileId,
            profileLinks
        );

        _unitOfWork.CloudFile.IsExistsAsync(_imageFileId, Arg.Any<CancellationToken>())
            .Returns(true);

        _unitOfWork.Profile.GetByAccountId(_accountId, Arg.Any<CancellationToken>())
            .Returns(existingProfile);

        var updatedProfile = new Profile
        {
            Id = existingProfile.Id,
            AccountId = _accountId,
            FirstName = "John",
            LastName = "Doe",
            Bio = "Test Bio",
            InstituteName = "Test Institute",
            PhoneNumber = "1234567890",
            ImageFileId = _imageFileId,
            ProfileLinks = new List<ProfileLinks>
            {
                new() { Name = "GitHub", Link = "https://github.com" }
            }
        };

        _unitOfWork.Profile.GetByAccountId(_accountId, Arg.Any<CancellationToken>())
            .Returns(updatedProfile);

        _unitOfWork.ProfileLink.GetAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(new ProfileLinks());

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.FirstName.Should().Be("John");
        result.Value.LastName.Should().Be("Doe");
        result.Value.BioMarkdown.Should().Be("Test Bio");
        result.Value.InstituteName.Should().Be("Test Institute");
        result.Value.PhoneNumber.Should().Be("1234567890");
        result.Value.ImageFile?.CloudFileId.Should().Be(_imageFileId);
        result.Value.ProfileLinks.Should().HaveCount(1);

        _unitOfWork.Profile.DidNotReceive().Add(Arg.Any<Profile>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenImageFileNotFound_ShouldReturnUnexpectedError()
    {
        // Arrange
        var command = new SaveProfileCommand(
            "John",
            "Doe",
            "Test Bio",
            "Test Institute",
            "1234567890",
            _imageFileId,
            new List<ProfileLinkRequest>()
        );

        _unitOfWork.CloudFile.IsExistsAsync(_imageFileId, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
        result.FirstError.Description.Should().Be("Image file not found.");

        _unitOfWork.Profile.DidNotReceive().Add(Arg.Any<Profile>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenUpdatingProfileWithNullValues_ShouldKeepExistingValues()
    {
        // Arrange
        var existingProfile = new Profile
        {
            Id = Guid.NewGuid(),
            AccountId = _accountId,
            FirstName = "John",
            LastName = "Doe",
            Bio = "Bio",
            InstituteName = "Institute",
            PhoneNumber = "1234567890",
            ImageFileId = _imageFileId
        };

        var command = new SaveProfileCommand(
            null,
            null,
            null,
            null,
            null,
            null,
            new List<ProfileLinkRequest>()
        );

        _unitOfWork.Profile.GetByAccountId(_accountId, Arg.Any<CancellationToken>())
            .Returns(existingProfile);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.FirstName.Should().Be("John");
        result.Value.LastName.Should().Be("Doe");
        result.Value.BioMarkdown.Should().Be("Bio");
        result.Value.InstituteName.Should().Be("Institute");
        result.Value.PhoneNumber.Should().Be("1234567890");
        result.Value.ImageFile?.CloudFileId.Should().Be(_imageFileId);

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenAddingNewProfileLinks_ShouldAddLinks()
    {
        // Arrange
        var existingProfile = new Profile
        {
            Id = Guid.NewGuid(),
            AccountId = _accountId,
            ProfileLinks = new List<ProfileLinks>()
        };

        var newLinks = new List<ProfileLinkRequest>
        {
            new(null, "GitHub", "https://github.com"),
            new(null, "LinkedIn", "https://linkedin.com")
        };

        var command = new SaveProfileCommand(
            null,
            null,
            null,
            null,
            null,
            null,
            newLinks
        );

        _unitOfWork.Profile.GetByAccountId(_accountId, Arg.Any<CancellationToken>())
            .Returns(existingProfile);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();

        _unitOfWork.ProfileLink.Received(2).Add(Arg.Any<ProfileLinks>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenUpdatingExistingProfileLinks_ShouldUpdateLinks()
    {
        // Arrange
        var existingProfile = new Profile
        {
            Id = Guid.NewGuid(),
            AccountId = _accountId,
            ProfileLinks = new List<ProfileLinks>
            {
                new() { Id = Guid.NewGuid(), Name = "Old GitHub", Link = "https://old.github.com" }
            }
        };

        var updatedLinks = new List<ProfileLinkRequest>
        {
            new(existingProfile.ProfileLinks.First().Id, "New GitHub", "https://new.github.com")
        };

        var command = new SaveProfileCommand(
            null,
            null,
            null,
            null,
            null,
            null,
            updatedLinks
        );

        _unitOfWork.Profile.GetByAccountId(_accountId, Arg.Any<CancellationToken>())
            .Returns(existingProfile);

        _unitOfWork.ProfileLink.GetAsync(existingProfile.ProfileLinks.First().Id, Arg.Any<CancellationToken>())
            .Returns(existingProfile.ProfileLinks.First());

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.ProfileLinks.Should().HaveCount(1);
        result.Value.ProfileLinks[0].Name.Should().Be("New GitHub");
        result.Value.ProfileLinks[0].Link.Should().Be("https://new.github.com");

        _unitOfWork.ProfileLink.DidNotReceive().Add(Arg.Any<ProfileLinks>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenProfileLinkNotFound_ShouldSkipUpdate()
    {
        // Arrange
        var existingProfile = new Profile
        {
            Id = Guid.NewGuid(),
            AccountId = _accountId,
            ProfileLinks = new List<ProfileLinks>()
        };

        var command = new SaveProfileCommand(
            "John",
            "Doe",
            null,
            null,
            null,
            null,
            new List<ProfileLinkRequest>
            {
                new(Guid.NewGuid(), "GitHub", "https://github.com")
            });

        _unitOfWork.Profile.GetByAccountId(_accountId, Arg.Any<CancellationToken>())
            .Returns(existingProfile);

        // Simulate not found
        _unitOfWork.ProfileLink.GetAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((ProfileLinks?)null);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new SaveProfileCommand(
            "John",
            "Doe",
            "Test Bio",
            "Test Institute",
            "1234567890",
            _imageFileId,
            new List<ProfileLinkRequest>
            {
                new(Guid.NewGuid(), "GitHub", "https://github.com/raofin")
            }
        );

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenFirstNameTooLong_ShouldHaveValidationError()
    {
        // Arrange
        var command = new SaveProfileCommand(
            new string('a', 21),
            "Doe",
            "Test Bio",
            "Test Institute",
            "1234567890",
            _imageFileId,
            new List<ProfileLinkRequest>()
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public void Validate_WhenBioTooLong_ShouldHaveValidationError()
    {
        // Arrange
        var command = new SaveProfileCommand(
            "John",
            "Doe",
            new string('a', 201),
            "Test Institute",
            "1234567890",
            _imageFileId,
            new List<ProfileLinkRequest>()
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.Bio);
    }

    [Fact]
    public void Validate_WhenProfileLinkNameTooLong_ShouldHaveValidationError()
    {
        // Arrange
        var command = new SaveProfileCommand(
            "John",
            "Doe",
            "Test Bio",
            "Test Institute",
            "1234567890",
            _imageFileId,
            new List<ProfileLinkRequest>
            {
                new(null, new string('a', 21), "https://github.com")
            }
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("ProfileLinks[0].Name");
    }

    [Fact]
    public void Validate_WhenProfileLinkLinkTooLong_ShouldHaveValidationError()
    {
        // Arrange
        var command = new SaveProfileCommand(
            "John",
            "Doe",
            "Test Bio",
            "Test Institute",
            "1234567890",
            _imageFileId,
            new List<ProfileLinkRequest>
            {
                new(null, "GitHub", new string('a', 201))
            }
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("ProfileLinks[0].Link");
    }

    [Fact]
    public void Validate_WhenProfileLinkIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new SaveProfileCommand(
            "John",
            "Doe",
            "Test Bio",
            "Test Institute",
            "1234567890",
            _imageFileId,
            new List<ProfileLinkRequest>
            {
                new(Guid.Empty, "GitHub", "https://github.com")
            }
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("ProfileLinks[0].ProfileLinkId");
    }
}