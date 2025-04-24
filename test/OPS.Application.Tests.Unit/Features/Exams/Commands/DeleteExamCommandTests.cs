using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.Exams.Commands;
using OPS.Domain;
using OPS.Domain.Entities.Exam;

namespace OPS.Application.Tests.Unit.Features.Exams.Commands;

public class DeleteExamCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteExamCommandHandler _sut;
    private readonly DeleteExamCommandValidator _validator = new();
    private readonly Examination _existingExam;

    public DeleteExamCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new DeleteExamCommandHandler(_unitOfWork);

        _existingExam = new Examination
        {
            Id = Guid.NewGuid(),
            Title = "Test Exam",
            IsPublished = false
        };
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldDeleteExam()
    {
        // Arrange
        var command = new DeleteExamCommand(_existingExam.Id);

        _unitOfWork.Exam.GetAsync(command.ExamId, Arg.Any<CancellationToken>())
            .Returns(_existingExam);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        _unitOfWork.Exam.Received(1).Remove(_existingExam);
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenExamNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new DeleteExamCommand(Guid.NewGuid());

        _unitOfWork.Exam.GetAsync(command.ExamId, Arg.Any<CancellationToken>())
            .Returns((Examination?)null);

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
        var publishedExam = new Examination
        {
            Id = Guid.NewGuid(),
            Title = "Published Exam",
            IsPublished = true
        };

        var command = new DeleteExamCommand(publishedExam.Id);

        _unitOfWork.Exam.GetAsync(command.ExamId, Arg.Any<CancellationToken>())
            .Returns(publishedExam);

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
        var command = new DeleteExamCommand(_existingExam.Id);

        _unitOfWork.Exam.GetAsync(command.ExamId, Arg.Any<CancellationToken>())
            .Returns(_existingExam);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(0);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);

        _unitOfWork.Exam.Received(1).Remove(_existingExam);
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new DeleteExamCommand(Guid.NewGuid());

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenExamIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new DeleteExamCommand(Guid.Empty);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.ExamId);
    }
}