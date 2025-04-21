using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Dtos;
using OPS.Application.Features.Questions.Written.Command;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Features.Questions.Written.Commands;

public class UpdateWrittenCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UpdateWrittenCommandHandler _sut;
    private readonly Question _question;
    private readonly Guid _validQuestionId;
    private readonly Guid _nonExistentQuestionId;
    private readonly UpdateWrittenCommandValidator _validator = new();

    public UpdateWrittenCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new UpdateWrittenCommandHandler(_unitOfWork);

        _validQuestionId = Guid.NewGuid();
        _nonExistentQuestionId = Guid.NewGuid();

        _question = new Question
        {
            Id = _validQuestionId,
            ExaminationId = Guid.NewGuid(),
            QuestionTypeId = (int)QuestionType.Written,
            StatementMarkdown = "Original question",
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
    public async Task Handle_WhenQuestionExistsAndExamNotPublished_ShouldUpdateQuestion()
    {
        // Arrange
        var newStatement = "Updated question";
        var newPoints = 25m;
        var newDifficulty = DifficultyType.Hard;
        var newHasLongAnswer = false;

        var command = new UpdateWrittenCommand(
            _validQuestionId,
            newStatement,
            newPoints,
            newHasLongAnswer,
            newDifficulty
        );

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<WrittenQuestionResponse>();
        result.Value.StatementMarkdown.Should().Be(newStatement);
        result.Value.Score.Should().Be(newPoints);
        result.Value.DifficultyType.Should().Be(newDifficulty);
        result.Value.HasLongAnswer.Should().Be(newHasLongAnswer);

        _question.StatementMarkdown.Should().Be(newStatement);
        _question.Points.Should().Be(newPoints);
        _question.DifficultyId.Should().Be((int)newDifficulty);
        _question.HasLongAnswer.Should().Be(newHasLongAnswer);
        _question.Examination.WrittenPoints.Should().Be(newPoints);

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenQuestionDoesNotExist_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new UpdateWrittenCommand(
            _nonExistentQuestionId,
            "Updated question",
            25,
            false,
            DifficultyType.Hard
        );

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
        _question.Examination.IsPublished = true;
        var command = new UpdateWrittenCommand(
            _validQuestionId,
            "Updated question",
            25,
            false,
            DifficultyType.Hard
        );

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
        result.FirstError.Description.Should().Be("Exam of this question is already published");
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenPartialUpdate_ShouldUpdateOnlySpecifiedFields()
    {
        // Arrange
        var newStatement = "Updated question";
        var command = new UpdateWrittenCommand(
            _validQuestionId,
            newStatement,
            null,
            null,
            null
        );

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<WrittenQuestionResponse>();
        result.Value.StatementMarkdown.Should().Be(newStatement);
        result.Value.Score.Should().Be(_question.Points);
        result.Value.DifficultyType.Should().Be((DifficultyType)_question.DifficultyId);
        result.Value.HasLongAnswer.Should().Be(_question.HasLongAnswer);

        _question.StatementMarkdown.Should().Be(newStatement);
        _question.Points.Should().Be(20);
        _question.DifficultyId.Should().Be((int)DifficultyType.Medium);
        _question.HasLongAnswer.Should().BeTrue();
        _question.Examination.WrittenPoints.Should().Be(20);

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new UpdateWrittenCommand(
            _validQuestionId,
            "Valid statement",
            20,
            true,
            DifficultyType.Medium
        );

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenQuestionIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateWrittenCommand(
            Guid.Empty,
            "Valid statement",
            20,
            true,
            DifficultyType.Medium
        );

        // Act & Assert
        _validator.TestValidate(command).ShouldHaveValidationErrorFor(x => x.QuestionId);
    }

    [Fact]
    public void Validate_WhenStatementIsTooShort_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateWrittenCommand(
            _validQuestionId,
            "Short", // Less than 10 characters
            20,
            true,
            DifficultyType.Medium
        );

        // Act & Assert
        _validator.TestValidate(command).ShouldHaveValidationErrorFor(x => x.StatementMarkdown);
    }

    [Fact]
    public void Validate_WhenPointsIsZero_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateWrittenCommand(
            _validQuestionId,
            "Valid statement",
            0,
            true,
            DifficultyType.Medium
        );

        // Act & Assert
        _validator.TestValidate(command).ShouldHaveValidationErrorFor(x => x.Points);
    }

    [Fact]
    public void Validate_WhenPointsIsGreaterThan100_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateWrittenCommand(
            _validQuestionId,
            "Valid statement",
            101,
            true,
            DifficultyType.Medium
        );

        // Act & Assert
        _validator.TestValidate(command).ShouldHaveValidationErrorFor(x => x.Points);
    }

    [Fact]
    public void Validate_WhenDifficultyTypeIsInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateWrittenCommand(
            _validQuestionId,
            "Valid statement",
            20,
            true,
            (DifficultyType)999
        );

        // Act & Assert
        _validator.TestValidate(command).ShouldHaveAnyValidationError();
    }

    [Fact]
    public void Validate_WhenAllFieldsAreNull_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new UpdateWrittenCommand(
            _validQuestionId,
            null,
            null,
            null,
            null
        );

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }
}