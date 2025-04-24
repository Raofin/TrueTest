using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.Review.Queries;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;

namespace OPS.Application.Tests.Unit.Features.Review.Queries;

public class GetMcqQuesWithSubmissionQueryTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetMcqQuesWithSubmissionQueryHandler _sut;
    private readonly GetMcqQuesWithSubmissionQueryValidator _validator = new();
    private readonly Guid _validExamId;
    private readonly Guid _validAccountId;

    public GetMcqQuesWithSubmissionQueryTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new GetMcqQuesWithSubmissionQueryHandler(_unitOfWork);
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
                StatementMarkdown = "MCQ Question 1",
                McqSubmissions = new List<McqSubmission>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        AccountId = _validAccountId,
                        Score = 80,
                    }
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                StatementMarkdown = "MCQ Question 2",
                McqSubmissions = new List<McqSubmission>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        AccountId = _validAccountId,
                        Score = 90,
                    }
                }
            }
        };

        _unitOfWork.McqSubmission.GetMcqQuesWithSubmission(_validExamId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns(questions);

        var query = new GetMcqQuesWithSubmissionQuery(_validExamId, _validAccountId);

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
        _unitOfWork.McqSubmission.GetMcqQuesWithSubmission(_validExamId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns(new List<Question>());

        var query = new GetMcqQuesWithSubmissionQuery(_validExamId, _validAccountId);

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
        var query = new GetMcqQuesWithSubmissionQuery(_validExamId, _validAccountId);

        // Act & Assert
        _validator.TestValidate(query).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenExamIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var query = new GetMcqQuesWithSubmissionQuery(Guid.Empty, _validAccountId);

        // Act & Assert
        _validator.TestValidate(query)
            .ShouldHaveValidationErrorFor("ExamId");
    }

    [Fact]
    public void Validate_WhenAccountIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var query = new GetMcqQuesWithSubmissionQuery(_validExamId, Guid.Empty);

        // Act & Assert
        _validator.TestValidate(query)
            .ShouldHaveValidationErrorFor("AccountId");
    }
}