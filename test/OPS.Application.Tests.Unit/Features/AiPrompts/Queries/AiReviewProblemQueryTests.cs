using ErrorOr;
using FluentAssertions;
using NSubstitute;
using OPS.Application.Dtos;
using OPS.Application.Features.AiPrompts.Queries;
using OPS.Application.Interfaces;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;

namespace OPS.Application.Tests.Unit.Features.AiPrompts.Queries;

public class AiReviewProblemQueryTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAiService _aiService;
    private readonly AiReviewProblemQueryHandler _sut;
    private readonly AiReviewProblemQueryValidator _validator = new();

    public AiReviewProblemQueryTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _aiService = Substitute.For<IAiService>();
        _sut = new AiReviewProblemQueryHandler(_unitOfWork, _aiService);
    }

    [Fact]
    public async Task Handle_WhenSubmissionExistsAndAiServiceReturnsResponse_ReturnsSuccess()
    {
        // Arrange
        var submissionId = Guid.NewGuid();
        var query = new AiReviewProblemQuery(submissionId);
        var submission = new ProblemSubmission
        {
            Id = submissionId,
            Code = "Test Code",
            Question = new Question
            {
                StatementMarkdown = "Test Question",
                Points = 10
            }
        };
        var expectedResponse = new AiSubmissionReview("Test Review", 8);

        _unitOfWork.ProblemSubmission.GetWithQuestionAsync(submissionId, Arg.Any<CancellationToken>())
            .Returns(submission);
        _aiService.PromptAsync<AiSubmissionReview>(Arg.Any<PromptRequest>())
            .Returns(expectedResponse);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task Handle_WhenSubmissionDoesNotExist_ReturnsNotFoundError()
    {
        // Arrange
        var submissionId = Guid.NewGuid();
        var query = new AiReviewProblemQuery(submissionId);

        _unitOfWork.ProblemSubmission.GetWithQuestionAsync(submissionId, Arg.Any<CancellationToken>())
            .Returns((ProblemSubmission)null!);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task Handle_WhenAiServiceReturnsNull_ReturnsError()
    {
        // Arrange
        var submissionId = Guid.NewGuid();
        var query = new AiReviewProblemQuery(submissionId);
        var submission = new ProblemSubmission
        {
            Id = submissionId,
            Code = "Test Code",
            Question = new Question()
            {
                StatementMarkdown = "Test Question",
                Points = 10
            }
        };

        _unitOfWork.ProblemSubmission.GetWithQuestionAsync(submissionId, Arg.Any<CancellationToken>())
            .Returns(submission);
        _aiService.PromptAsync<AiSubmissionReview>(Arg.Any<PromptRequest>())
            .Returns((AiSubmissionReview)null!);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithValidInput_ShouldNotHaveErrors()
    {
        // Arrange
        var query = new AiReviewProblemQuery(Guid.NewGuid());

        // Act
        var result = _validator.Validate(query);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithInvalidInput_ShouldHaveErrors()
    {
        // Arrange
        var query = new AiReviewProblemQuery(Guid.Empty);

        // Act
        var result = _validator.Validate(query);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}