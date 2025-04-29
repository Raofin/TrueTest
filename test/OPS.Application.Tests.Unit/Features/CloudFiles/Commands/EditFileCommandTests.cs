using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using OPS.Application.Features.CloudFiles.Commands;
using OPS.Application.Interfaces.Cloud;
using OPS.Domain;
using OPS.Domain.Entities.Core;

namespace OPS.Application.Tests.Unit.Features.CloudFiles.Commands;

public class EditFileCommandTests
{
    private readonly ICloudFileService _cloudFileService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly EditFileCommandHandler _sut;
    private readonly EditFileCommandValidator _validator;

    public EditFileCommandTests()
    {
        _cloudFileService = Substitute.For<ICloudFileService>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new EditFileCommandHandler(_cloudFileService, _unitOfWork);
        _validator = new EditFileCommandValidator();
    }

    [Fact]
    public async Task Handle_ValidFile_ReturnsSuccess()
    {
        // Arrange
        var fileMock = Substitute.For<IFormFile>();
        var cloudFileId = Guid.NewGuid();
        var command = new EditFileCommand(cloudFileId, fileMock);
        var newCloudFile = new CloudFile { Id = Guid.NewGuid(), FileId = "new-file-id" };
        var oldCloudFile = new CloudFile { Id = cloudFileId, FileId = "old-file-id" };

        _cloudFileService.UploadAsync(Arg.Any<IFormFile>(), Arg.Any<CancellationToken>())
            .Returns(newCloudFile);
        _unitOfWork.CloudFile.GetAsync(cloudFileId, Arg.Any<CancellationToken>())
            .Returns(oldCloudFile);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.CloudFileId.Should().Be(newCloudFile.Id);
        result.Value.FileId.Should().Be(newCloudFile.FileId);
        _unitOfWork.CloudFile.Received(1).Add(Arg.Any<CloudFile>());
        _unitOfWork.CloudFile.Received(1).Remove(Arg.Any<CloudFile>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        await _cloudFileService.Received(1).DeleteAsync(oldCloudFile.FileId);
    }

    [Fact]
    public async Task Handle_UploadFails_ReturnsError()
    {
        // Arrange
        var fileMock = Substitute.For<IFormFile>();
        var cloudFileId = Guid.NewGuid();
        var command = new EditFileCommand(cloudFileId, fileMock);

        _cloudFileService.UploadAsync(Arg.Any<IFormFile>(), Arg.Any<CancellationToken>())
            .Returns((CloudFile)null!);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_CommitFails_ReturnsError()
    {
        // Arrange
        var fileMock = Substitute.For<IFormFile>();
        var cloudFileId = Guid.NewGuid();
        var command = new EditFileCommand(cloudFileId, fileMock);
        var newCloudFile = new CloudFile { Id = Guid.NewGuid(), FileId = "new-file-id" };
        var oldCloudFile = new CloudFile { Id = cloudFileId, FileId = "old-file-id" };

        _cloudFileService.UploadAsync(Arg.Any<IFormFile>(), Arg.Any<CancellationToken>())
            .Returns(newCloudFile);
        _unitOfWork.CloudFile.GetAsync(cloudFileId, Arg.Any<CancellationToken>())
            .Returns(oldCloudFile);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(0);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_OldFileNotFound_StillSucceeds()
    {
        // Arrange
        var fileMock = Substitute.For<IFormFile>();
        var cloudFileId = Guid.NewGuid();
        var command = new EditFileCommand(cloudFileId, fileMock);
        var newCloudFile = new CloudFile { Id = Guid.NewGuid(), FileId = "new-file-id" };

        _cloudFileService.UploadAsync(Arg.Any<IFormFile>(), Arg.Any<CancellationToken>())
            .Returns(newCloudFile);
        _unitOfWork.CloudFile.GetAsync(cloudFileId, Arg.Any<CancellationToken>())
            .Returns((CloudFile)null!);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.CloudFileId.Should().Be(newCloudFile.Id);
        _unitOfWork.CloudFile.Received(1).Add(Arg.Any<CloudFile>());
        _unitOfWork.CloudFile.DidNotReceive().Remove(Arg.Any<CloudFile>());
    }

    [Fact]
    public void Validate_EmptyCloudFileId_ReturnsError()
    {
        // Arrange
        var fileMock = Substitute.For<IFormFile>();
        var command = new EditFileCommand(Guid.Empty, fileMock);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CloudFileId");
    }
}