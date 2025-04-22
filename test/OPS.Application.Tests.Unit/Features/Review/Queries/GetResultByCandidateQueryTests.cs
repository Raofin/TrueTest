using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.Review.Queries;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Entities.User;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Features.Review.Queries;

public class GetResultByCandidateQueryTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetResultsByExamQueryHandler _sut;
    private readonly GetResultByCandidateQueryValidator _validator = new();
    private readonly Guid _validExamId;
    private readonly Guid _validAccountId;

    public GetResultByCandidateQueryTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new GetResultsByExamQueryHandler(_unitOfWork);
        _validExamId = Guid.NewGuid();
        _validAccountId = Guid.NewGuid();
    }

    [Fact]
    public async Task Handle_WhenExamAndCandidateExist_ShouldReturnExamResult()
    {
        // Arrange
        var exam = new Examination
        {
            Id = _validExamId,
            Title = "Test Exam",
            ExamCandidates = new List<ExamCandidate>
            {
                new()
                {
                    AccountId = _validAccountId,
                    Account = new Account
                    {
                        Username = "John",
                        Email = "john.doe@example.com"
                    },
                    StartedAt = DateTime.Now,
                    McqScore = 80,
                    ProblemSolvingScore = 90,
                    WrittenScore = 85
                }
            },
            Questions = new List<Question>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    QuestionTypeId = (int)QuestionType.ProblemSolving,
                    StatementMarkdown = "Problem Question",
                    ProblemSubmissions = new List<ProblemSubmission>
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            AccountId = _validAccountId,
                            Code = "print('Hello')",
                            LanguageId = LanguageId.python.ToString(),
                            Score = 90,
                            TestCaseOutputs = new List<TestCaseOutput>
                            {
                                new() { IsAccepted = true, ReceivedOutput = "Hello" }
                            }
                        }
                    }
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    QuestionTypeId = (int)QuestionType.Written,
                    StatementMarkdown = "Written Question",
                    WrittenSubmissions = new List<WrittenSubmission>
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            AccountId = _validAccountId,
                            Answer = "Sample answer",
                            Score = 85
                        }
                    }
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    QuestionTypeId = (int)QuestionType.MCQ,
                    StatementMarkdown = "MCQ Question",
                    McqSubmissions = new List<McqSubmission>
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            AccountId = _validAccountId,
                            Score = 80,
                            // AnswerOptions = new List<Guid> { Guid.NewGuid() }
                        }
                    }
                }
            }
        };

        _unitOfWork.Exam.GetWithQuesAndSubmissionsAsync(_validExamId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns(exam);

        var query = new GetResultByCandidateQuery(_validExamId, _validAccountId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Exam.ExamId.Should().Be(_validExamId);
        result.Value.Exam.Title.Should().Be("Test Exam");
        result.Value.Account.Username.Should().Be("John");
        result.Value.Result.Should().NotBeNull();
        result.Value.Result!.McqScore.Should().Be(80);
        result.Value.Result.ProblemSolvingScore.Should().Be(90);
        result.Value.Result.WrittenScore.Should().Be(85);
    }

    [Fact]
    public async Task Handle_WhenExamNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        _unitOfWork.Exam.GetWithQuesAndSubmissionsAsync(_validExamId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns((Examination)null!);

        var query = new GetResultByCandidateQuery(_validExamId, _validAccountId);

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
        var query = new GetResultByCandidateQuery(_validExamId, _validAccountId);

        // Act & Assert
        _validator.TestValidate(query).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenExamIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var query = new GetResultByCandidateQuery(Guid.Empty, _validAccountId);

        // Act & Assert
        _validator.TestValidate(query)
            .ShouldHaveValidationErrorFor("ExamId");
    }

    [Fact]
    public void Validate_WhenAccountIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var query = new GetResultByCandidateQuery(_validExamId, Guid.Empty);

        // Act & Assert
        _validator.TestValidate(query)
            .ShouldHaveValidationErrorFor("AccountId");
    }
}