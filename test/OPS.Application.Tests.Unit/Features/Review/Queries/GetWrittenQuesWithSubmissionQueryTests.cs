using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.Review.Queries;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Features.Review.Queries;

public class GetWrittenQuesWithSubmissionQueryTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetWrittenQuesWithSubmissionQueryHandler _sut;
    private readonly GetWrittenQuesWithSubmissionQueryValidator _validator = new();
    private readonly Guid _validExamId;
    private readonly Guid _validAccountId;

    public GetWrittenQuesWithSubmissionQueryTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new GetWrittenQuesWithSubmissionQueryHandler(_unitOfWork);
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
                StatementMarkdown = "Written Question 1",
                QuestionTypeId = (int)QuestionType.Written,
                WrittenSubmissions = new List<WrittenSubmission>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        AccountId = _validAccountId,
                        Answer = "First answer",
                        Score = 80,
                        IsFlagged = true,
                        FlagReason = "Suspicious answer"
                    }
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                QuestionTypeId = (int)QuestionType.Written,
                StatementMarkdown = "Written Question 2",
                WrittenSubmissions = new List<WrittenSubmission>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        AccountId = _validAccountId,
                        Answer = "Second answer",
                        Score = 90,
                        IsFlagged = false,
                        FlagReason = null
                    }
                }
            }
        };

        _unitOfWork.WrittenSubmission.GetQuesWithSubmission(_validExamId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns(questions);

        var query = new GetWrittenQuesWithSubmissionQuery(_validExamId, _validAccountId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(2);

        var firstQuestion = result.Value[0];
        firstQuestion.Should().NotBeNull();
        firstQuestion!.StatementMarkdown.Should().Be("Written Question 1");

        var secondQuestion = result.Value[1];
        secondQuestion.Should().NotBeNull();
        secondQuestion!.StatementMarkdown.Should().Be("Written Question 2");
    }

    [Fact]
    public async Task Handle_WhenNoQuestionsExist_ShouldReturnEmptyList()
    {
        // Arrange
        _unitOfWork.WrittenSubmission.GetQuesWithSubmission(_validExamId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns(new List<Question>());

        var query = new GetWrittenQuesWithSubmissionQuery(_validExamId, _validAccountId);

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
        var query = new GetWrittenQuesWithSubmissionQuery(_validExamId, _validAccountId);

        // Act & Assert
        _validator.TestValidate(query).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenExamIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var query = new GetWrittenQuesWithSubmissionQuery(Guid.Empty, _validAccountId);

        // Act & Assert
        _validator.TestValidate(query)
            .ShouldHaveValidationErrorFor("ExamId");
    }

    [Fact]
    public void Validate_WhenAccountIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var query = new GetWrittenQuesWithSubmissionQuery(_validExamId, Guid.Empty);

        // Act & Assert
        _validator.TestValidate(query)
            .ShouldHaveValidationErrorFor("AccountId");
    }
}