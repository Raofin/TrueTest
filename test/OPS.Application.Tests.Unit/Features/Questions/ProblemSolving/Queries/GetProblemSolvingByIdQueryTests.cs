using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Dtos;
using OPS.Application.Features.Questions.ProblemSolving.Queries;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Features.Questions.ProblemSolving.Queries;

public class GetProblemSolvingByIdQueryTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetProblemSolvingByIdQueryHandler _sut;
    private readonly Question _question;
    private readonly Guid _validQuestionId;
    private readonly Guid _nonExistentQuestionId;
    private readonly GetProblemSolvingByIdQueryValidator _validator = new();

    public GetProblemSolvingByIdQueryTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new GetProblemSolvingByIdQueryHandler(_unitOfWork);

        _validQuestionId = Guid.NewGuid();
        _nonExistentQuestionId = Guid.NewGuid();

        _question = new Question
        {
            Id = _validQuestionId,
            ExaminationId = Guid.NewGuid(),
            QuestionTypeId = (int)QuestionType.ProblemSolving,
            StatementMarkdown = "Problem solving question",
            Points = 20,
            DifficultyId = (int)DifficultyType.Medium,
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
    }

    [Fact]
    public async Task Handle_WhenQuestionExists_ShouldReturnQuestion()
    {
        // Arrange
        var query = new GetProblemSolvingByIdQuery(_validQuestionId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<ProblemQuestionResponse>();
        result.Value.StatementMarkdown.Should().Be("Problem solving question");
        result.Value.Points.Should().Be(20);
        result.Value.DifficultyType.Should().Be(DifficultyType.Medium);
        result.Value.TestCases.Should().HaveCount(2);
        result.Value.TestCases.Should().SatisfyRespectively(
            first =>
            {
                first.Input.Should().Be("1 2 3");
                first.Output.Should().Be("3");
            },
            second =>
            {
                second.Input.Should().Be("4 5 6");
                second.Output.Should().Be("6");
            }
        );
    }

    [Fact]
    public async Task Handle_WhenQuestionDoesNotExist_ShouldReturnNotFoundError()
    {
        // Arrange
        var query = new GetProblemSolvingByIdQuery(_nonExistentQuestionId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public void Validate_WhenValidQuery_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var query = new GetProblemSolvingByIdQuery(_validQuestionId);

        // Act & Assert
        _validator.TestValidate(query).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenQuestionIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var query = new GetProblemSolvingByIdQuery(Guid.Empty);

        // Act & Assert
        _validator.TestValidate(query)
            .ShouldHaveValidationErrorFor(x => x.QuestionId);
    }
}