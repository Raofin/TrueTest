using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.Questions.Written.Command;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Features.Questions.Written.Commands;

public class DeleteWrittenCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteWrittenCommandHandler _sut;
    private readonly Question _question;
    private readonly Guid _validQuestionId;
    private readonly Guid _nonExistentQuestionId;
    private readonly DeleteWrittenCommandValidator _validator = new();

    public DeleteWrittenCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new DeleteWrittenCommandHandler(_unitOfWork);

        _validQuestionId = Guid.NewGuid();
        _nonExistentQuestionId = Guid.NewGuid();

        _question = new Question
        {
            Id = _validQuestionId,
            ExaminationId = Guid.NewGuid(),
            QuestionTypeId = (int)QuestionType.Written,
            StatementMarkdown = "Sample written question",
            Points = 20,
            DifficultyId = (int)DifficultyType.Medium,
            HasLongAnswer = true,
            Examination = new Examination
            {
                Id = Guid.NewGuid(),
                IsPublished = false,
                WrittenPoints = 20
            }
        };

        // Set up default return values
        _unitOfWork.Question.GetWithExamAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Question)null!);
        _unitOfWork.Question.GetWithExamAsync(_validQuestionId, Arg.Any<CancellationToken>())
            .Returns(_question);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);
    }

    [Fact]
    public async Task Handle_WhenQuestionExistsAndExamNotPublished_ShouldDeleteQuestion()
    {
        // Arrange
        var command = new DeleteWrittenCommand(_validQuestionId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);
        _unitOfWork.Question.Received(1).Remove(_question);
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        _question.Examination.WrittenPoints.Should().Be(0);
    }

    [Fact]
    public async Task Handle_WhenQuestionDoesNotExist_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new DeleteWrittenCommand(_nonExistentQuestionId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        _unitOfWork.Question.DidNotReceive().Remove(Arg.Any<Question>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenExamIsPublished_ShouldReturnConflictError()
    {
        // Arrange
        _question.Examination.IsPublished = true;
        var command = new DeleteWrittenCommand(_validQuestionId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
        _unitOfWork.Question.DidNotReceive().Remove(Arg.Any<Question>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenCommitFails_ShouldReturnUnexpectedError()
    {
        // Arrange
        var command = new DeleteWrittenCommand(_validQuestionId);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(0);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
        _unitOfWork.Question.Received(1).Remove(_question);
    }

    [Fact]
    public void Validate_WhenValidId_ShouldReturnSuccess()
    {
        // Arrange
        var command = new DeleteWrittenCommand(Guid.Empty);

        // Act & Assert
        _validator.TestValidate(command).ShouldHaveAnyValidationError();
    }
}