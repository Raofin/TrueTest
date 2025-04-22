using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Dtos;
using OPS.Application.Features.Review.Queries;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Entities.User;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Features.Review.Queries;

public class GetExamByCandidateQueryTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetExamByCandidateQueryHandler _sut;
    private readonly GetExamByCandidateQueryValidator _validator = new();
    private readonly Guid _validExamId;
    private readonly Guid _validAccountId;

    public GetExamByCandidateQueryTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new GetExamByCandidateQueryHandler(_unitOfWork);
        _validExamId = Guid.NewGuid();
        _validAccountId = Guid.NewGuid();
    }

    [Fact]
    public async Task Handle_WhenExamAndCandidateExist_ShouldReturnExamWithSubmissions()
    {
        // Arrange
        var candidate = new ExamCandidate
        {
            AccountId = _validAccountId,
            Account = new Account
            {
                Username = "John",
                Email = "john.doe@example.com"
            },
            McqScore = 80,
            ProblemSolvingScore = 90,
            WrittenScore = 85
        };

        var exam = new Examination
        {
            Id = _validExamId,
            Title = "Test Exam",
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
                            Score = 90
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
                            Score = 80
                        }
                    }
                }
            }
        };

        _unitOfWork.Exam.GetCandidateAsync(_validExamId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns(candidate);

        _unitOfWork.Exam.GetWithQuesAndSubmissionsAsync(_validExamId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns(exam);

        var query = new GetExamByCandidateQuery(_validExamId, _validAccountId);

        // Act
        ErrorOr<ExamQuesWithSubmissionResponse> result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_WhenCandidateNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        _unitOfWork.Exam.GetCandidateAsync(_validExamId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns((ExamCandidate)null!);

        var query = new GetExamByCandidateQuery(_validExamId, _validAccountId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Description.Should().Be("Candidate not found");
    }

    [Fact]
    public async Task Handle_WhenExamNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var candidate = new ExamCandidate
        {
            AccountId = _validAccountId,
            Account = new Account()
        };

        _unitOfWork.Exam.GetCandidateAsync(_validExamId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns(candidate);

        _unitOfWork.Exam.GetWithQuesAndSubmissionsAsync(_validExamId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns((Examination)null!);

        var query = new GetExamByCandidateQuery(_validExamId, _validAccountId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Description.Should().Be("Exam not found");
    }

    [Fact]
    public void Validate_WhenValidQuery_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var query = new GetExamByCandidateQuery(_validExamId, _validAccountId);

        // Act & Assert
        _validator.TestValidate(query).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenExamIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var query = new GetExamByCandidateQuery(Guid.Empty, _validAccountId);

        // Act & Assert
        _validator.TestValidate(query)
            .ShouldHaveValidationErrorFor("ExamId");
    }
}