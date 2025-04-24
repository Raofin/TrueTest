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

public class CreateWrittenCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateWrittenCommandHandler _sut;
    private readonly Examination _exam;
    private readonly Guid _validExamId;
    private readonly Guid _nonExistentExamId;
    private readonly List<CreateWrittenQuestionRequest> _validQuestions;
    private readonly CreateWrittenCommandValidator _validator = new();

    public CreateWrittenCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new CreateWrittenCommandHandler(_unitOfWork);

        _validExamId = Guid.NewGuid();
        _nonExistentExamId = Guid.NewGuid();

        _exam = new Examination
        {
            Id = _validExamId,
            IsPublished = false,
            WrittenPoints = 0
        };

        _validQuestions =
        [
            new CreateWrittenQuestionRequest(
                "Explain the concept of Object-Oriented Programming in detail.",
                20,
                DifficultyType.Medium,
                true
            ),

            new CreateWrittenQuestionRequest(
                "What are the SOLID principles? Explain each principle with examples.",
                25,
                DifficultyType.Hard,
                true
            )
        ];

        _unitOfWork.Exam.GetAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Examination)null!);
        _unitOfWork.Exam.GetAsync(_validExamId, Arg.Any<CancellationToken>())
            .Returns(_exam);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);
    }

    [Fact]
    public async Task Handle_WhenExamExistsAndNotPublished_ShouldCreateQuestions()
    {
        // Arrange
        var command = new CreateWrittenCommand(_validExamId, _validQuestions);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().HaveCount(2);
        result.Value.Should().AllSatisfy(q =>
        {
            q.Should().BeOfType<WrittenQuestionResponse>();
            q.StatementMarkdown.Should().BeOneOf(_validQuestions.Select(r => r.StatementMarkdown));
            q.Score.Should().BeOneOf(_validQuestions.Select(r => r.Points));
            q.DifficultyType.Should().BeOneOf(_validQuestions.Select(r => r.DifficultyType));
            q.HasLongAnswer.Should().BeTrue();
        });

        _unitOfWork.Question.Received(1)
            .AddRange(Arg.Is<List<Question>>(questions =>
                questions.Count == 2 &&
                questions.All(q => q.ExaminationId == _validExamId) &&
                questions.All(q => q.QuestionTypeId == (int)QuestionType.Written) &&
                questions.All(q => q.HasLongAnswer)));

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        _exam.WrittenPoints.Should().Be(_validQuestions.Sum(q => q.Points));
    }

    [Fact]
    public async Task Handle_WhenExamDoesNotExist_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new CreateWrittenCommand(_nonExistentExamId, _validQuestions);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        _unitOfWork.Question.DidNotReceive().AddRange(Arg.Any<List<Question>>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenExamIsPublished_ShouldReturnConflictError()
    {
        // Arrange
        _exam.IsPublished = true;
        var command = new CreateWrittenCommand(_validExamId, _validQuestions);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
        result.FirstError.Description.Should().Be("Exam of this question is already published");
        _unitOfWork.Question.DidNotReceive().AddRange(Arg.Any<List<Question>>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenCommitFails_ShouldReturnUnexpectedError()
    {
        // Arrange
        var command = new CreateWrittenCommand(_validExamId, _validQuestions);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(0);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
        _unitOfWork.Question.Received(1).AddRange(Arg.Any<List<Question>>());
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateWrittenCommand(_validExamId, _validQuestions);

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenExamIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateWrittenCommand(Guid.Empty, _validQuestions);

        // Act & Assert
        _validator.TestValidate(command).ShouldHaveValidationErrorFor(x => x.ExamId);
    }

    [Fact]
    public void Validate_WhenWrittenQuestionsIsNull_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateWrittenCommand(_validExamId, null!);

        // Act & Assert
        _validator.TestValidate(command).ShouldHaveValidationErrorFor(x => x.WrittenQuestions);
    }

    [Fact]
    public void Validate_WhenWrittenQuestionsIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateWrittenCommand(_validExamId, new List<CreateWrittenQuestionRequest>());

        // Act & Assert
        _validator.TestValidate(command).ShouldHaveValidationErrorFor(x => x.WrittenQuestions);
    }

    [Fact]
    public void Validate_WhenQuestionHasInvalidStatement_ShouldHaveValidationError()
    {
        // Arrange
        var invalidQuestions = new List<CreateWrittenQuestionRequest>
        {
            new CreateWrittenQuestionRequest(
                "", // Empty statement
                20,
                DifficultyType.Medium,
                true
            )
        };
        var command = new CreateWrittenCommand(_validExamId, invalidQuestions);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("WrittenQuestions[0].StatementMarkdown");
    }

    [Fact]
    public void Validate_WhenQuestionHasInvalidPoints_ShouldHaveValidationError()
    {
        // Arrange
        var invalidQuestions = new List<CreateWrittenQuestionRequest>
        {
            new CreateWrittenQuestionRequest(
                "Valid statement",
                0, // Invalid points
                DifficultyType.Medium,
                true
            )
        };
        var command = new CreateWrittenCommand(_validExamId, invalidQuestions);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("WrittenQuestions[0].Points");
    }

    [Fact]
    public void Validate_WhenQuestionHasInvalidDifficulty_ShouldHaveValidationError()
    {
        // Arrange
        var invalidQuestions = new List<CreateWrittenQuestionRequest>
        {
            new CreateWrittenQuestionRequest(
                "Valid statement",
                20,
                (DifficultyType)999, // Invalid difficulty
                true
            )
        };
        var command = new CreateWrittenCommand(_validExamId, invalidQuestions);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("WrittenQuestions[0].DifficultyType");
    }
}