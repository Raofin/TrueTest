using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.User.Commands;
using OPS.Domain;
using OPS.Domain.Entities.User;

namespace OPS.Application.Tests.Unit.Features.User.Commands;

public class DeleteProfileLinkCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteProfileLinkCommandHandler _sut;
    private readonly DeleteProfileLinkCommandValidator _validator = new();

    public DeleteProfileLinkCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new DeleteProfileLinkCommandHandler(_unitOfWork);
    }

    [Fact]
    public async Task Handle_WhenProfileLinkExists_ShouldDeleteAndReturnSuccess()
    {
        // Arrange
        var profileLinkId = Guid.NewGuid();
        var profileLink = new ProfileLinks { Id = profileLinkId };

        _unitOfWork.ProfileLink.GetAsync(profileLinkId, Arg.Any<CancellationToken>())
            .Returns(profileLink);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        var command = new DeleteProfileLinkCommand(profileLinkId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        _unitOfWork.ProfileLink.Received(1).Remove(profileLink);
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenProfileLinkNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var profileLinkId = Guid.NewGuid();
        _unitOfWork.ProfileLink.GetAsync(profileLinkId, Arg.Any<CancellationToken>())
            .Returns((ProfileLinks?)null);

        var command = new DeleteProfileLinkCommand(profileLinkId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);

        _unitOfWork.ProfileLink.DidNotReceive().Remove(Arg.Any<ProfileLinks>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenCommitFails_ShouldReturnUnexpectedError()
    {
        // Arrange
        var profileLinkId = Guid.NewGuid();
        var profileLink = new ProfileLinks { Id = profileLinkId };

        _unitOfWork.ProfileLink.GetAsync(profileLinkId, Arg.Any<CancellationToken>())
            .Returns(profileLink);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(0);

        var command = new DeleteProfileLinkCommand(profileLinkId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);

        _unitOfWork.ProfileLink.Received(1).Remove(profileLink);
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new DeleteProfileLinkCommand(Guid.NewGuid());

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenProfileLinkIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new DeleteProfileLinkCommand(Guid.Empty);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.ProfileLinkId);
    }
}