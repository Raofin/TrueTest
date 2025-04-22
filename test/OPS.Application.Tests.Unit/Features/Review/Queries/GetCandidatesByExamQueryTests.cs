using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.Review.Queries;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.User;

namespace OPS.Application.Tests.Unit.Features.Review.Queries;

public class GetCandidatesByExamQueryTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetCandidatesByExamQueryHandler _sut;
    private readonly GetCandidatesByExamQueryValidator _validator = new();
    private readonly Guid _validExamId;

    public GetCandidatesByExamQueryTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new GetCandidatesByExamQueryHandler(_unitOfWork);
        _validExamId = Guid.NewGuid();
    }

    [Fact]
    public async Task Handle_WhenExamExists_ShouldReturnExamWithCandidates()
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
                    AccountId = Guid.NewGuid(),
                    Account = new Account
                    {
                        Username = "John",
                        Email = "john.doe@example.com"
                    },
                    McqScore = 80,
                    ProblemSolvingScore = 90,
                    WrittenScore = 85
                },
                new()
                {
                    AccountId = Guid.NewGuid(),
                    Account = new Account
                    {
                        Username = "Jane",
                        Email = "jane.smith@example.com"
                    },
                    McqScore = 75,
                    ProblemSolvingScore = 85,
                    WrittenScore = 80
                }
            }
        };

        _unitOfWork.Exam.GetResultsAsync(_validExamId, Arg.Any<CancellationToken>())
            .Returns(exam);

        var query = new GetCandidatesByExamQuery(_validExamId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();

        var firstCandidate = result.Value.Candidates[0];
        firstCandidate.Account.Username.Should().Be("John");
    }

    [Fact]
    public async Task Handle_WhenExamNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        _unitOfWork.Exam.GetResultsAsync(_validExamId, Arg.Any<CancellationToken>())
            .Returns((Examination)null!);

        var query = new GetCandidatesByExamQuery(_validExamId);

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
        var query = new GetCandidatesByExamQuery(_validExamId);

        // Act & Assert
        _validator.TestValidate(query).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenExamIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var query = new GetCandidatesByExamQuery(Guid.Empty);

        // Act & Assert
        _validator.TestValidate(query)
            .ShouldHaveValidationErrorFor("ExamId");
    }
}