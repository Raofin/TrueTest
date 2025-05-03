using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using OPS.Application.Features.CloudFiles.Commands;
using OPS.Application.Interfaces.Auth;
using OPS.Application.Interfaces.Cloud;
using OPS.Domain;
using OPS.Domain.Entities.Core;

namespace OPS.Application.Tests.Unit.Features.CloudFiles.Commands;

public class UploadFileCommandTests
{
    private readonly ICloudFileService _cloudFileService;
    private readonly IUserProvider _userProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UploadFileCommandHandler _sut;
    private readonly UploadFileCommandValidator _validator;

    public UploadFileCommandTests()
    {
        _cloudFileService = Substitute.For<ICloudFileService>();
        _userProvider = Substitute.For<IUserProvider>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new UploadFileCommandHandler(_cloudFileService, _userProvider, _unitOfWork);
        _validator = new UploadFileCommandValidator();
    }

    [Fact]
    public async Task Handle_ValidFile_ReturnsSuccess()
    {
        // Arrange
        var fileMock = Substitute.For<IFormFile>();
        fileMock.Length.Returns(1024); // 1KB file
        var command = new UploadFileCommand(fileMock);
        var cloudFile = new CloudFile { Id = Guid.NewGuid(), FileId = "test-file-id" };
        var accountId = Guid.NewGuid();

        _cloudFileService.UploadAsync(Arg.Any<IFormFile>(), Arg.Any<CancellationToken>())
            .Returns(cloudFile);
        _userProvider.TryGetAccountId().Returns(accountId);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.CloudFileId.Should().Be(cloudFile.Id);
        result.Value.FileId.Should().Be(cloudFile.FileId);
        _unitOfWork.CloudFile.Received(1).Add(Arg.Any<CloudFile>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_UploadFails_ReturnsError()
    {
        // Arrange
        var fileMock = Substitute.For<IFormFile>();
        fileMock.Length.Returns(1024);
        var command = new UploadFileCommand(fileMock);

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
        fileMock.Length.Returns(1024);
        var command = new UploadFileCommand(fileMock);
        var cloudFile = new CloudFile { Id = Guid.NewGuid(), FileId = "test-file-id" };

        _cloudFileService.UploadAsync(Arg.Any<IFormFile>(), Arg.Any<CancellationToken>())
            .Returns(cloudFile);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(0);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
    }

    [Theory]
    [InlineData(0, "No file uploaded.")]
    [InlineData(102401, "File size exceeds the 100 KB limit.")]
    public void Validate_InvalidFileSize_ReturnsError(long fileSize, string expectedError)
    {
        // Arrange
        var fileMock = Substitute.For<IFormFile>();
        fileMock.Length.Returns(fileSize);
        var command = new UploadFileCommand(fileMock);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == expectedError);
    }
}