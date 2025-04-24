using ErrorOr;
using FluentAssertions;
using NSubstitute;
using OPS.Application.Features.Exams.Commands;
using OPS.Domain;
using OPS.Domain.Entities.Exam;

namespace OPS.Application.Tests.Unit.Features.Exams.Commands;

public class UpdateExamCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UpdateExamCommandHandler _sut;
    private readonly Examination _existingExam;
    private readonly Guid _validExamId = Guid.NewGuid();
    private readonly Guid _nonExistentExamId = Guid.NewGuid();
    private readonly DateTime _now = DateTime.UtcNow;
    private readonly DateTime _newOpensAt = DateTime.UtcNow.AddDays(2);
    private readonly DateTime _newClosesAt = DateTime.UtcNow.AddDays(3);

    public UpdateExamCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new UpdateExamCommandHandler(_unitOfWork);

        _existingExam = new Examination
        {
            Id = _validExamId,
            Title = "Original Title",
            DescriptionMarkdown = "Original Description",
            DurationMinutes = 60,
            TotalPoints = 100,
            OpensAt = _now.AddDays(1),
            ClosesAt = _now.AddDays(2),
            IsPublished = false
        };
    }

    [Fact]
    public async Task Handle_WhenValidCommand_ShouldUpdateExamAndReturnSuccess()
    {
        // Arrange
        _unitOfWork.Exam.GetAsync(_validExamId, Arg.Any<CancellationToken>())
            .Returns(_existingExam);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        var command = new UpdateExamCommand(
            _validExamId,
            "New Title",
            "New Description",
            90,
            150,
            _newOpensAt,
            _newClosesAt);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Title.Should().Be(command.Title);
        result.Value.Description.Should().Be(command.Description);
        result.Value.DurationMinutes.Should().Be(command.DurationMinutes);
        result.Value.TotalPoints.Should().Be(command.TotalPoints);
        result.Value.OpensAt.Should().Be(command.OpensAt);
        result.Value.ClosesAt.Should().Be(command.ClosesAt);
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenPartialUpdate_ShouldUpdateOnlySpecifiedFields()
    {
        // Arrange
        _unitOfWork.Exam.GetAsync(_validExamId, Arg.Any<CancellationToken>())
            .Returns(_existingExam);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        var command = new UpdateExamCommand(
            _validExamId,
            "New Title",
            null,
            null,
            null,
            null,
            null);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Title.Should().Be(command.Title);
        result.Value.Description.Should().Be(_existingExam.DescriptionMarkdown);
        result.Value.DurationMinutes.Should().Be(_existingExam.DurationMinutes);
        result.Value.TotalPoints.Should().Be(_existingExam.TotalPoints);
        result.Value.OpensAt.Should().Be(_existingExam.OpensAt);
        result.Value.ClosesAt.Should().Be(_existingExam.ClosesAt);
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenExamDoesNotExist_ShouldReturnNotFoundError()
    {
        // Arrange
        _unitOfWork.Exam.GetAsync(_nonExistentExamId, Arg.Any<CancellationToken>())
            .Returns((Examination?)null);

        var command = new UpdateExamCommand(
            _nonExistentExamId,
            "New Title",
            null,
            null,
            null,
            null,
            null);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenExamIsPublished_ShouldReturnConflictError()
    {
        // Arrange
        var publishedExam = new Examination
        {
            Id = _validExamId,
            IsPublished = true
        };

        _unitOfWork.Exam.GetAsync(_validExamId, Arg.Any<CancellationToken>())
            .Returns(publishedExam);

        var command = new UpdateExamCommand(
            _validExamId,
            "New Title",
            null,
            null,
            null,
            null,
            null);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
        result.FirstError.Description.Should().Be("Exam is already published");
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldReturnSuccess()
    {
        // Arrange
        var command = new UpdateExamCommand(
            _validExamId,
            "New Title",
            "New Description",
            90,
            150,
            _newOpensAt,
            _newClosesAt);

        var validator = new UpdateExamCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}