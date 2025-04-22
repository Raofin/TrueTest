using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.Review.Queries;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Features.Review.Queries;

public class GetProblemQuesWithSubmissionQueryTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetProblemQuesWithSubmissionQueryHandler _sut;
    private readonly GetProblemQuesWithSubmissionQueryValidator _validator = new();
    private readonly Guid _validExamId;
    private readonly Guid _validAccountId;

    public GetProblemQuesWithSubmissionQueryTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new GetProblemQuesWithSubmissionQueryHandler(_unitOfWork);
        _validExamId = Guid.NewGuid();
        _validAccountId = Guid.NewGuid();
    }

    [Fact]
    public async Task Handle_WhenQuestionsExist_ShouldReturnQuestionsWithSubmissions()
    {
        // Arrange
        var questions = new List<Question>
        {
            new()
            {
                Id = Guid.NewGuid(),
                StatementMarkdown = "Problem Question 1",
                ProblemSubmissions = new List<ProblemSubmission>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        AccountId = _validAccountId,
                        Code = "print('Hello')",
                        LanguageId = LanguageId.python.ToString(),
                        Score = 80,
                        TestCaseOutputs = new List<TestCaseOutput>
                        {
                            new() { IsAccepted = true, ReceivedOutput = "Hello" },
                            new() { IsAccepted = false, ReceivedOutput = "Error" }
                        }
                    }
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                StatementMarkdown = "Problem Question 2",
                ProblemSubmissions = new List<ProblemSubmission>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        AccountId = _validAccountId,
                        Code = "console.log('Hello')",
                        LanguageId = LanguageId.javascript.ToString(),
                        Score = 90,
                        TestCaseOutputs = new List<TestCaseOutput>
                        {
                            new() { IsAccepted = true, ReceivedOutput = "Hello" },
                            new() { IsAccepted = true, ReceivedOutput = "Hello" }
                        }
                    }
                }
            }
        };

        _unitOfWork.ProblemSubmission.GetAllProblemsWithSubmission(_validExamId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns(questions);

        var query = new GetProblemQuesWithSubmissionQuery(_validExamId, _validAccountId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WhenNoQuestionsExist_ShouldReturnEmptyList()
    {
        // Arrange
        _unitOfWork.ProblemSubmission.GetAllProblemsWithSubmission(_validExamId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns(new List<Question>());

        var query = new GetProblemQuesWithSubmissionQuery(_validExamId, _validAccountId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WhenValidQuery_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var query = new GetProblemQuesWithSubmissionQuery(_validExamId, _validAccountId);

        // Act & Assert
        _validator.TestValidate(query).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenExamIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var query = new GetProblemQuesWithSubmissionQuery(Guid.Empty, _validAccountId);

        // Act & Assert
        _validator.TestValidate(query)
            .ShouldHaveValidationErrorFor("ExamId");
    }

    [Fact]
    public void Validate_WhenAccountIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var query = new GetProblemQuesWithSubmissionQuery(_validExamId, Guid.Empty);

        // Act & Assert
        _validator.TestValidate(query)
            .ShouldHaveValidationErrorFor("AccountId");
    }
}