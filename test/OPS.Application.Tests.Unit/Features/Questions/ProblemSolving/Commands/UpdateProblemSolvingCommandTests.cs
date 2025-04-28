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

public class UpdateProblemSolvingCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UpdateProblemSolvingCommandHandler _sut;
    private readonly Question _question;
    private readonly Guid _validQuestionId;
    private readonly Guid _nonExistentQuestionId;
    private readonly UpdateProblemSolvingCommandValidator _validator = new();

    public UpdateProblemSolvingCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new UpdateProblemSolvingCommandHandler(_unitOfWork);

        _validQuestionId = Guid.NewGuid();
        _nonExistentQuestionId = Guid.NewGuid();

        _question = new Question
        {
            Id = _validQuestionId,
            ExaminationId = Guid.NewGuid(),
            QuestionTypeId = (int)QuestionType.ProblemSolving,
            StatementMarkdown = "Original question",
            Points = 20,
            DifficultyId = (int)DifficultyType.Medium,
            Examination = new Examination
            {
                Id = Guid.NewGuid(),
                IsPublished = false,
                ProblemSolvingPoints = 20
            },
            TestCases = new List<TestCase>
            {
                new() { Id = Guid.NewGuid(), Input = "1 2 3", ExpectedOutput = "3" },
                new() { Id = Guid.NewGuid(), Input = "4 5 6", ExpectedOutput = "6" }
            }
        };

        // Set up default return values
        _unitOfWork.Question.GetWithTestCases(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Question)null!);
        _unitOfWork.Question.GetWithTestCases(_validQuestionId, Arg.Any<CancellationToken>())
            .Returns(_question);
        _unitOfWork.TestCase.GetAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((TestCase)null!);
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
        var firstTestCaseId = _question.TestCases.First().Id;

        var newTestCases = new List<TestCaseUpdateRequest>
        {
            new(firstTestCaseId, "1 2 3 4", "4"),
            new(null, "7 8 9", "9")
        };
        _unitOfWork.TestCase.GetAsync(firstTestCaseId, Arg.Any<CancellationToken>())
            .Returns(new TestCase { Id = firstTestCaseId });

        var command = new UpdateProblemSolvingCommand(
            _validQuestionId,
            newStatement,
            newPoints,
            newDifficulty,
            newTestCases
        );

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<ProblemQuestionResponse>();
        result.Value.StatementMarkdown.Should().Be(newStatement);
        result.Value.Points.Should().Be(newPoints);
        result.Value.DifficultyType.Should().Be(newDifficulty);
        result.Value.TestCases.Should().HaveCount(3);

        _question.StatementMarkdown.Should().Be(newStatement);
        _question.Points.Should().Be(newPoints);
        _question.DifficultyId.Should().Be((int)newDifficulty);
        _question.Examination.ProblemSolvingPoints.Should().Be(newPoints);
        _question.TestCases.Should().HaveCount(3);

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenQuestionDoesNotExist_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new UpdateProblemSolvingCommand(
            _nonExistentQuestionId,
            "Updated question",
            25,
            DifficultyType.Hard,
            []
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
        var command = new UpdateProblemSolvingCommand(
            _validQuestionId,
            "Updated question",
            25,
            DifficultyType.Hard,
            []
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
        var command = new UpdateProblemSolvingCommand(
            _validQuestionId,
            newStatement,
            null,
            null,
            []
        );

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<ProblemQuestionResponse>();
        result.Value.StatementMarkdown.Should().Be(newStatement);
        result.Value.Points.Should().Be(_question.Points);
        result.Value.DifficultyType.Should().Be((DifficultyType)_question.DifficultyId);
        result.Value.TestCases.Should().HaveCount(2);

        _question.StatementMarkdown.Should().Be(newStatement);
        _question.Points.Should().Be(20);
        _question.DifficultyId.Should().Be((int)DifficultyType.Medium);
        _question.Examination.ProblemSolvingPoints.Should().Be(20);
        _question.TestCases.Should().HaveCount(2);

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new UpdateProblemSolvingCommand(
            _validQuestionId,
            "Valid statement",
            20,
            DifficultyType.Medium,
            [new(_question.TestCases.First().Id, "1 2 3", "3")]
        );

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenQuestionIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateProblemSolvingCommand(
            Guid.Empty,
            "Valid statement",
            20,
            DifficultyType.Medium,
            []
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.QuestionId);
    }

    [Fact]
    public void Validate_WhenPointsIsZero_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateProblemSolvingCommand(
            _validQuestionId,
            "Valid statement",
            0, // Invalid points
            DifficultyType.Medium,
            []
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.Points);
    }

    [Fact]
    public void Validate_WhenPointsIsGreaterThan100_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateProblemSolvingCommand(
            _validQuestionId,
            "Valid statement",
            101, // Invalid points
            DifficultyType.Medium,
            new List<TestCaseUpdateRequest>()
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.Points);
    }

    [Fact]
    public void Validate_WhenDifficultyTypeIsInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateProblemSolvingCommand(
            _validQuestionId,
            "Valid statement",
            20,
            (DifficultyType)999, // Invalid difficulty
            new List<TestCaseUpdateRequest>()
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.DifficultyType);
    }

    [Fact]
    public void Validate_WhenNewTestCaseHasEmptyInput_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateProblemSolvingCommand(
            _validQuestionId,
            "Valid statement",
            20,
            DifficultyType.Medium,
            [new TestCaseUpdateRequest(null, "", "3")]
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("TestCases[0].Input");
    }

    [Fact]
    public void Validate_WhenNewTestCaseHasEmptyOutput_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateProblemSolvingCommand(
            _validQuestionId,
            "Valid statement",
            20,
            DifficultyType.Medium,
            [new TestCaseUpdateRequest(null, "1,2,3", "")]
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("TestCases[0].Output");
    }

    [Fact]
    public void Validate_WhenAllFieldsAreNull_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new UpdateProblemSolvingCommand(
            _validQuestionId,
            null,
            null,
            null,
            []
        );

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }
}