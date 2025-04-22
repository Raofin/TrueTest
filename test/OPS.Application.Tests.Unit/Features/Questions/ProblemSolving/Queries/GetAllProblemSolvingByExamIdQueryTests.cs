using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Dtos;
using OPS.Application.Features.Questions.ProblemSolving.Queries;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Features.Questions.ProblemSolving.Queries;

public class GetAllProblemSolvingByExamIdQueryTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetProblemSolvingByExamQueryHandler _sut;
    private readonly Guid _validExamId;
    private readonly Guid _nonExistentExamId;
    private readonly List<Question> _questions;
    private readonly GetProblemSolvingByExamQueryValidator _validator = new();

    public GetAllProblemSolvingByExamIdQueryTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new GetProblemSolvingByExamQueryHandler(_unitOfWork);

        _validExamId = Guid.NewGuid();
        _nonExistentExamId = Guid.NewGuid();

        _questions =
        [
            new Question
            {
                Id = Guid.NewGuid(),
                ExaminationId = _validExamId,
                QuestionTypeId = (int)QuestionType.ProblemSolving,
                StatementMarkdown = "First problem solving question",
                Points = 20,
                DifficultyId = (int)DifficultyType.Medium,
                TestCases = new List<TestCase>
                {
                    new() { Id = Guid.NewGuid(), Input = "1,2,3", ExpectedOutput = "3" },
                    new() { Id = Guid.NewGuid(), Input = "4,5,6", ExpectedOutput = "6" }
                }
            },

            new Question
            {
                Id = Guid.NewGuid(),
                ExaminationId = _validExamId,
                QuestionTypeId = (int)QuestionType.ProblemSolving,
                StatementMarkdown = "Second problem solving question",
                Points = 25,
                DifficultyId = (int)DifficultyType.Hard,
                TestCases = new List<TestCase>
                {
                    new() { Id = Guid.NewGuid(), Input = "7 8 9", ExpectedOutput = "9" }
                }
            }
        ];

        // Set up default return values
        _unitOfWork.Question.GetProblemSolvingByExamIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns([]);
        _unitOfWork.Question.GetProblemSolvingByExamIdAsync(_validExamId, Arg.Any<CancellationToken>())
            .Returns(_questions);
    }

    [Fact]
    public async Task Handle_WhenExamHasQuestions_ShouldReturnQuestions()
    {
        // Arrange
        var query = new GetProblemSolvingByExamQuery(_validExamId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().HaveCount(2);
        result.Value.Should().AllBeOfType<ProblemQuestionResponse>();
        result.Value.Should().SatisfyRespectively(
            first =>
            {
                first.StatementMarkdown.Should().Be("First problem solving question");
                first.Points.Should().Be(20);
                first.DifficultyType.Should().Be(DifficultyType.Medium);
                first.TestCases.Should().HaveCount(2);
            },
            second =>
            {
                second.StatementMarkdown.Should().Be("Second problem solving question");
                second.Points.Should().Be(25);
                second.DifficultyType.Should().Be(DifficultyType.Hard);
                second.TestCases.Should().HaveCount(1);
            }
        );
    }

    [Fact]
    public async Task Handle_WhenExamHasNoQuestions_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetProblemSolvingByExamQuery(_nonExistentExamId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WhenValidQuery_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var query = new GetProblemSolvingByExamQuery(_validExamId);

        // Act & Assert
        _validator.TestValidate(query).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenExamIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var query = new GetProblemSolvingByExamQuery(Guid.Empty);

        // Act & Assert
        _validator.TestValidate(query).ShouldHaveValidationErrorFor(x => x.ExamId);
    }
}