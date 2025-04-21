using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Dtos;
using OPS.Application.Features.Questions.ProblemSolving.Commands;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Features.Questions.ProblemSolving.Commands;

public class CreateProblemSolvingCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateProblemSolvingCommandHandler _sut;
    private readonly Examination _exam;
    private readonly Guid _validExamId;
    private readonly Guid _nonExistentExamId;
    private readonly List<CreateProblemQuestionRequest> _validQuestions;
    private readonly CreateProblemSolvingCommandValidator _validator = new();

    public CreateProblemSolvingCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new CreateProblemSolvingCommandHandler(_unitOfWork);

        _validExamId = Guid.NewGuid();
        _nonExistentExamId = Guid.NewGuid();

        _exam = new Examination
        {
            Id = _validExamId,
            IsPublished = false,
            ProblemSolvingPoints = 0
        };

        _validQuestions =
        [
            new CreateProblemQuestionRequest(
                "Write a function to find the maximum element in an array.",
                20,
                DifficultyType.Medium,
                [
                    new TestCaseRequest("1 2 3 4 5", "5"),
                    new TestCaseRequest("10 20 30", "30")
                ]
            ),

            new CreateProblemQuestionRequest(
                "Implement a function to check if a string is a palindrome.",
                25,
                DifficultyType.Hard,
                [
                    new TestCaseRequest("racecar", "true"),
                    new TestCaseRequest("hello", "false")
                ]
            )
        ];

        // Set up default return values
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
        var command = new CreateProblemSolvingCommand(_validExamId, _validQuestions);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().HaveCount(2);
        result.Value.Should().AllSatisfy(q =>
        {
            q.Should().BeOfType<ProblemQuestionResponse>();
            q.StatementMarkdown.Should().BeOneOf(_validQuestions.Select(r => r.StatementMarkdown));
            q.Points.Should().BeOneOf(_validQuestions.Select(r => r.Points));
            q.DifficultyType.Should().BeOneOf(_validQuestions.Select(r => r.DifficultyType));
            q.TestCases.Should().HaveCount(2);
        });

        _unitOfWork.Question.Received(1)
            .AddRange(Arg.Is<List<Question>>(questions =>
                questions.Count == 2 &&
                questions.All(q => q.ExaminationId == _validExamId) &&
                questions.All(q => q.QuestionTypeId == (int)QuestionType.ProblemSolving) &&
                questions.All(q => q.TestCases.Count == 2)));

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        _exam.ProblemSolvingPoints.Should().Be(_validQuestions.Sum(q => q.Points));
    }

    [Fact]
    public async Task Handle_WhenExamDoesNotExist_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new CreateProblemSolvingCommand(_nonExistentExamId, _validQuestions);

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
        var command = new CreateProblemSolvingCommand(_validExamId, _validQuestions);

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
        var command = new CreateProblemSolvingCommand(_validExamId, _validQuestions);
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
        var command = new CreateProblemSolvingCommand(_validExamId, _validQuestions);

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenExamIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateProblemSolvingCommand(Guid.Empty, _validQuestions);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.ExamId);
    }

    [Fact]
    public void Validate_WhenProblemQuestionsIsNull_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateProblemSolvingCommand(_validExamId, null!);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.ProblemQuestions);
    }

    [Fact]
    public void Validate_WhenProblemQuestionsIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateProblemSolvingCommand(_validExamId, []);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.ProblemQuestions);
    }

    [Fact]
    public void Validate_WhenQuestionHasInvalidStatement_ShouldHaveValidationError()
    {
        // Arrange
        var invalidQuestions = new List<CreateProblemQuestionRequest>
        {
            new(
                "", // Empty statement
                20,
                DifficultyType.Medium,
                [new("1 2 3", "3")]
            )
        };
        var command = new CreateProblemSolvingCommand(_validExamId, invalidQuestions);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("ProblemQuestions[0].StatementMarkdown");
    }

    [Fact]
    public void Validate_WhenQuestionHasInvalidPoints_ShouldHaveValidationError()
    {
        // Arrange
        var invalidQuestions = new List<CreateProblemQuestionRequest>
        {
            new(
                "Valid question",
                0, // Invalid points
                DifficultyType.Medium,
                [new("1,2,3", "3")]
            )
        };
        var command = new CreateProblemSolvingCommand(_validExamId, invalidQuestions);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("ProblemQuestions[0].Points");
    }

    [Fact]
    public void Validate_WhenQuestionHasInvalidDifficulty_ShouldHaveValidationError()
    {
        // Arrange
        var invalidQuestions = new List<CreateProblemQuestionRequest>
        {
            new(
                "Valid question",
                20,
                (DifficultyType)999, // Invalid difficulty
                [new TestCaseRequest("1,2,3", "3")]
            )
        };
        var command = new CreateProblemSolvingCommand(_validExamId, invalidQuestions);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("ProblemQuestions[0].DifficultyType");
    }

    [Fact]
    public void Validate_WhenQuestionHasNoTestCases_ShouldHaveValidationError()
    {
        // Arrange
        var invalidQuestions = new List<CreateProblemQuestionRequest>
        {
            new(
                "Valid question",
                20,
                DifficultyType.Medium,
                [] // Empty test cases
            )
        };
        var command = new CreateProblemSolvingCommand(_validExamId, invalidQuestions);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("ProblemQuestions[0].TestCases");
    }

    [Fact]
    public void Validate_WhenTestCaseHasEmptyInput_ShouldHaveValidationError()
    {
        // Arrange
        var invalidQuestions = new List<CreateProblemQuestionRequest>
        {
            new(
                "Valid question",
                20,
                DifficultyType.Medium,
                [new TestCaseRequest("", "3")]
            )
        };
        var command = new CreateProblemSolvingCommand(_validExamId, invalidQuestions);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("ProblemQuestions[0].TestCases[0].Input");
    }

    [Fact]
    public void Validate_WhenTestCaseHasEmptyOutput_ShouldHaveValidationError()
    {
        // Arrange
        var invalidQuestions = new List<CreateProblemQuestionRequest>
        {
            new(
                "Valid question",
                20,
                DifficultyType.Medium,
                [new TestCaseRequest("1,2,3", "")]
            )
        };
        var command = new CreateProblemSolvingCommand(_validExamId, invalidQuestions);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("ProblemQuestions[0].TestCases[0].Output");
    }
}