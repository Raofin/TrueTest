using ErrorOr;
using FluentAssertions;
using NSubstitute;
using OPS.Application.Features.Exams.Commands;
using OPS.Domain;
using OPS.Domain.Entities.Exam;

namespace OPS.Application.Tests.Unit.Features.Exams.Commands;

public class PublishExamCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly PublishExamCommandHandler _sut;
    private readonly Examination _exam;
    private readonly Guid _validExamId = Guid.NewGuid();
    private readonly Guid _nonExistentExamId = Guid.NewGuid();

    public PublishExamCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new PublishExamCommandHandler(_unitOfWork);

        _exam = new Examination
        {
            Id = _validExamId,
            TotalPoints = 100,
            IsPublished = false,
            Questions = new List<Question>
            {
                new() { Points = 40 },
                new() { Points = 60 }
            }
        };
    }

    [Fact]
    public async Task Handle_WhenExamExistsAndPointsMatch_ShouldPublishExamAndReturnSuccess()
    {
        // Arrange
        _unitOfWork.Exam.GetWithQuestionsAsync(_validExamId, Arg.Any<CancellationToken>())
            .Returns(_exam);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        var command = new PublishExamCommand(_validExamId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);
        _exam.IsPublished.Should().BeTrue();
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenExamDoesNotExist_ShouldReturnNotFoundError()
    {
        // Arrange
        _unitOfWork.Exam.GetWithQuestionsAsync(_nonExistentExamId, Arg.Any<CancellationToken>())
            .Returns((Examination?)null);

        var command = new PublishExamCommand(_nonExistentExamId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenPointsDoNotMatch_ShouldReturnConflictError()
    {
        // Arrange
        var examWithMismatchedPoints = new Examination
        {
            Id = _validExamId,
            TotalPoints = 100,
            IsPublished = false,
            Questions = new List<Question>
            {
                new() { Points = 30 },
                new() { Points = 60 }
            }
        };

        _unitOfWork.Exam.GetWithQuestionsAsync(_validExamId, Arg.Any<CancellationToken>())
            .Returns(examWithMismatchedPoints);

        var command = new PublishExamCommand(_validExamId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
        result.FirstError.Description.Should().Be("Total points of questions do not match the exam total points.");
        examWithMismatchedPoints.IsPublished.Should().BeFalse();
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenExamIsAlreadyPublished_ShouldReturnConflictError()
    {
        // Arrange
        var publishedExam = new Examination
        {
            Id = _validExamId,
            TotalPoints = 100,
            IsPublished = true,
            Questions = new List<Question>
            {
                new() { Points = 40 },
                new() { Points = 60 }
            }
        };

        _unitOfWork.Exam.GetWithQuestionsAsync(_validExamId, Arg.Any<CancellationToken>())
            .Returns(publishedExam);

        var command = new PublishExamCommand(_validExamId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
        result.FirstError.Description.Should().Be("Exam is already published");
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }
}