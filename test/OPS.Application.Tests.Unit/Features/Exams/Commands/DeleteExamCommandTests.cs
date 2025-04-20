using ErrorOr;
using FluentAssertions;
using NSubstitute;
using OPS.Application.Features.Exams.Commands;
using OPS.Domain;
using OPS.Domain.Entities.Exam;

namespace OPS.Application.Tests.Unit.Features.Exams.Commands;

public class DeleteExamCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteExamCommandHandler _sut;
    private readonly Examination _unpublishedExam;
    private readonly Examination _publishedExam;
    private readonly Guid _validExamId = Guid.NewGuid();
    private readonly Guid _nonExistentExamId = Guid.NewGuid();

    public DeleteExamCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new DeleteExamCommandHandler(_unitOfWork);

        _unpublishedExam = new Examination
        {
            Id = _validExamId,
            IsPublished = false
        };

        _publishedExam = new Examination
        {
            Id = _validExamId,
            IsPublished = true
        };
    }

    [Fact]
    public async Task Handle_WhenExamExistsAndUnpublished_ShouldDeleteExamAndReturnSuccess()
    {
        // Arrange
        _unitOfWork.Exam.GetAsync(_validExamId, Arg.Any<CancellationToken>())
            .Returns(_unpublishedExam);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        var command = new DeleteExamCommand(_validExamId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);
        _unitOfWork.Exam.Received(1).Remove(_unpublishedExam);
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenExamDoesNotExist_ShouldReturnNotFoundError()
    {
        // Arrange
        _unitOfWork.Exam.GetAsync(_nonExistentExamId, Arg.Any<CancellationToken>())
            .Returns((Examination?)null);

        var command = new DeleteExamCommand(_nonExistentExamId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        _unitOfWork.Exam.DidNotReceive().Remove(Arg.Any<Examination>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenExamIsPublished_ShouldReturnValidationError()
    {
        // Arrange
        _unitOfWork.Exam.GetAsync(_validExamId, Arg.Any<CancellationToken>())
            .Returns(_publishedExam);

        var command = new DeleteExamCommand(_validExamId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
        _unitOfWork.Exam.DidNotReceive().Remove(Arg.Any<Examination>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenCommitFails_ShouldReturnUnexpectedError()
    {
        // Arrange
        _unitOfWork.Exam.GetAsync(_validExamId, Arg.Any<CancellationToken>())
            .Returns(_unpublishedExam);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(0);

        var command = new DeleteExamCommand(_validExamId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
        _unitOfWork.Exam.Received(1).Remove(_unpublishedExam);
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }
}