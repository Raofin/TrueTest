using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using OPS.Application.Dtos;
using OPS.Application.Interfaces.Cloud;
using OPS.Application.Services;

namespace OPS.Application.Tests.Unit.Services;

public class CloudFileServiceTests
{
    private readonly IGoogleCloudService _googleCloudService;
    private readonly CloudFileService _sut;

    public CloudFileServiceTests()
    {
        _googleCloudService = Substitute.For<IGoogleCloudService>();
        _sut = new CloudFileService(_googleCloudService);
    }

    [Fact]
    public async Task UploadAsync_ValidFile_ReturnsCloudFile()
    {
        // Arrange
        var formFile = Substitute.For<IFormFile>();
        formFile.FileName.Returns("test.pdf");
        formFile.ContentType.Returns("application/pdf");

        var uploadedFile = new GoogleFile(
            "test-file-id", "test.pdf", "application/pdf", 1024, DateTime.UtcNow
        );

        _googleCloudService.UploadAsync(Arg.Any<Stream>(), formFile.FileName, formFile.ContentType)
            .Returns(uploadedFile);

        // Act
        var result = await _sut.UploadAsync(formFile);

        // Assert
        result.Should().NotBeNull();
        result!.FileId.Should().Be(uploadedFile.CloudFileId);
        result.Name.Should().Be(uploadedFile.Name);
        result.ContentType.Should().Be(uploadedFile.ContentType);
        result.Size.Should().Be(uploadedFile.Size);
    }

    [Fact]
    public async Task UploadAsync_UploadFails_ReturnsNull()
    {
        // Arrange
        var formFile = Substitute.For<IFormFile>();
        formFile.FileName.Returns("test.pdf");
        formFile.ContentType.Returns("application/pdf");

        _googleCloudService.UploadAsync(Arg.Any<Stream>(), formFile.FileName, formFile.ContentType)
            .Returns((GoogleFile)null!);

        // Act
        var result = await _sut.UploadAsync(formFile);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DownloadAsync_ValidFile_ReturnsFileDownloadResponse()
    {
        // Arrange
        var fileId = "test-file-id";
        var googleCloudFile = new GoogleFileDownload(
            "test-file-id", "test.pdf", "application/pdf", 1024, [1, 2, 3], DateTime.UtcNow
        );


        _googleCloudService.DownloadAsync(fileId)
            .Returns(googleCloudFile);

        // Act
        var result = await _sut.DownloadAsync(fileId);

        // Assert
        result.Should().NotBeNull();
        result.FileName.Should().Be(googleCloudFile.Name);
        result.ContentType.Should().Be(googleCloudFile.ContentType);
        result.Size.Should().Be(googleCloudFile.Size);
    }

    [Fact]
    public async Task DeleteAsync_ValidFileId_DeletesFile()
    {
        // Arrange
        var fileId = "test-file-id";

        // Act
        await _sut.DeleteAsync(fileId);

        // Assert
        await _googleCloudService.Received(1).DeleteAsync(fileId);
    }

    [Fact]
    public async Task DeleteAsync_NullFileId_DoesNotDeleteFile()
    {
        // Arrange
        string? fileId = null;

        // Act
        await _sut.DeleteAsync(fileId);

        // Assert
        await _googleCloudService.DidNotReceive().DeleteAsync(Arg.Any<string>());
    }
}