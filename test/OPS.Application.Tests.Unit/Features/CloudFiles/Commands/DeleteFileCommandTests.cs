using ErrorOr;
using FluentAssertions;
using NSubstitute;
using OPS.Application.Features.CloudFiles.Commands;
using OPS.Application.Interfaces.Cloud;
using OPS.Domain;
using OPS.Domain.Entities.Core;

namespace OPS.Application.Tests.Unit.Features.CloudFiles.Commands;

public class DeleteFileCommandTests
{
    private readonly ICloudFileService _cloudFileService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteFileCommandHandler _sut;
    private readonly DeleteFileCommandValidator _validator;

    public DeleteFileCommandTests()
    {
        _cloudFileService = Substitute.For<ICloudFileService>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new DeleteFileCommandHandler(_cloudFileService, _unitOfWork);
        _validator = new DeleteFileCommandValidator();
    }

    [Fact]
    public async Task Handle_ValidFile_ReturnsSuccess()
    {
        // Arrange
        var cloudFileId = Guid.NewGuid();
        var command = new DeleteFileCommand(cloudFileId);
        var cloudFile = new CloudFile { Id = cloudFileId, FileId = "test-file-id" };

        _unitOfWork.CloudFile.GetAsync(cloudFileId, Arg.Any<CancellationToken>())
            .Returns(cloudFile);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        _unitOfWork.CloudFile.Received(1).Remove(Arg.Any<CloudFile>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        await _cloudFileService.Received(1).DeleteAsync(cloudFile.FileId);
    }

    [Fact]
    public async Task Handle_FileNotFound_ReturnsNotFoundError()
    {
        // Arrange
        var cloudFileId = Guid.NewGuid();
        var command = new DeleteFileCommand(cloudFileId);

        _unitOfWork.CloudFile.GetAsync(cloudFileId, Arg.Any<CancellationToken>())
            .Returns((CloudFile)null!);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task Handle_CommitFails_StillDeletesCloudFile()
    {
        // Arrange
        var cloudFileId = Guid.NewGuid();
        var command = new DeleteFileCommand(cloudFileId);
        var cloudFile = new CloudFile { Id = cloudFileId, FileId = "test-file-id" };

        _unitOfWork.CloudFile.GetAsync(cloudFileId, Arg.Any<CancellationToken>())
            .Returns(cloudFile);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(0);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        _unitOfWork.CloudFile.Received(1).Remove(Arg.Any<CloudFile>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        await _cloudFileService.Received(1).DeleteAsync(cloudFile.FileId);
    }

    [Fact]
    public void Validate_EmptyCloudFileId_ReturnsError()
    {
        // Arrange
        var command = new DeleteFileCommand(Guid.Empty);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CloudFileId");
    }
}