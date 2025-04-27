using ErrorOr;
using FluentAssertions;
using NSubstitute;
using OPS.Application.Features.CloudFiles.Queries;
using OPS.Application.Mappers;
using OPS.Domain;
using OPS.Domain.Entities.Core;

namespace OPS.Application.Tests.Unit.Features.CloudFiles.Queries;

public class GetFileDetailsQueryTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetFileDetailsQueryHandler _sut;
    private readonly GetFileDetailsQueryValidator _validator;

    public GetFileDetailsQueryTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new GetFileDetailsQueryHandler(_unitOfWork);
        _validator = new GetFileDetailsQueryValidator();
    }

    [Fact]
    public async Task Handle_ValidFile_ReturnsSuccess()
    {
        // Arrange
        var cloudFileId = Guid.NewGuid();
        var query = new GetFileDetailsQuery(cloudFileId);
        var cloudFile = new CloudFile
        {
            Id = cloudFileId,
            FileId = "test-file-id",
            Name = "test.pdf",
            ContentType = "application/pdf",
            Size = 1024
        };

        _unitOfWork.CloudFile.GetAsync(cloudFileId, Arg.Any<CancellationToken>())
            .Returns(cloudFile);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(cloudFile.MapToDto());
    }

    [Fact]
    public async Task Handle_FileNotFound_ReturnsNotFoundError()
    {
        // Arrange
        var cloudFileId = Guid.NewGuid();
        var query = new GetFileDetailsQuery(cloudFileId);

        _unitOfWork.CloudFile.GetAsync(cloudFileId, Arg.Any<CancellationToken>())
            .Returns((CloudFile)null!);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public void Validate_EmptyCloudFileId_ReturnsError()
    {
        // Arrange
        var query = new GetFileDetailsQuery(Guid.Empty);

        // Act
        var result = _validator.Validate(query);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CloudFileId");
    }
}