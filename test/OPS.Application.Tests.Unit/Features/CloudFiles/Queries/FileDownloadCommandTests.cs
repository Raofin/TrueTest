using ErrorOr;
using FluentAssertions;
using NSubstitute;
using OPS.Application.Dtos;
using OPS.Application.Features.CloudFiles.Queries;
using OPS.Application.Services.CloudService;

namespace OPS.Application.Tests.Unit.Features.CloudFiles.Queries;

public class FileDownloadCommandTests
{
    private readonly ICloudFileService _cloudFileService;
    private readonly FileDownloadCommandHandler _sut;
    private readonly FileDownloadCommandValidator _validator;

    public FileDownloadCommandTests()
    {
        _cloudFileService = Substitute.For<ICloudFileService>();
        _sut = new FileDownloadCommandHandler(_cloudFileService);
        _validator = new FileDownloadCommandValidator();
    }

    [Fact]
    public async Task Handle_ValidFile_ReturnsSuccess()
    {
        // Arrange
        var fileId = "test-file-id";
        var command = new FileDownloadCommand(fileId);
        var expectedResponse = new FileDownloadResponse(
            FileName: "test.pdf",
            ContentType: "application/pdf",
            Size: 1024,
            Bytes: [1, 2, 3]
        );

        _cloudFileService.DownloadAsync(fileId)
            .Returns(expectedResponse);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task Handle_FileNotFound_ReturnsNotFoundError()
    {
        // Arrange
        var fileId = "non-existent-file-id";
        var command = new FileDownloadCommand(fileId);

        _cloudFileService.DownloadAsync(fileId)
            .Returns((FileDownloadResponse)null!);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public void Validate_EmptyFileId_ReturnsError()
    {
        // Arrange
        var command = new FileDownloadCommand(string.Empty);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "FileId");
    }

    [Fact]
    public void Validate_NullFileId_ReturnsError()
    {
        // Arrange
        var command = new FileDownloadCommand(null!);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "FileId");
    }

    [Fact]
    public void Validate_WhitespaceFileId_ReturnsError()
    {
        // Arrange
        var command = new FileDownloadCommand("   ");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "FileId");
    }
}